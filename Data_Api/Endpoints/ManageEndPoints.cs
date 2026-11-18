using Data_Api.Services;

using Microsoft.AspNetCore.Http;

namespace Data_Api.Endpoints
{
    public static class ManageEndPoints
    {
        private const int MegtartottMentesek = 5;

        public static void AddManageEndpoints(this WebApplication app)
        {
            IWebHostEnvironment hostingEnv = app.Environment;
            string FilePath = Path.GetFullPath(Path.Combine(hostingEnv.ContentRootPath, "Mentesek"));
            Directory.CreateDirectory(FilePath);
            RegiMentesekTorlese(FilePath);

            #region Manage
            app.MapGet("/save", async (DataSaving saving, Settings settings) =>
            {
                if(settings.LastModify>settings.LastSaved)
                {
                    try
                    {
                        await saving.SaveToJsonAsync(Path.Combine(FilePath,$"mentes-{DateTime.Today.ToString("s").Replace(':', '_')}.json"));
                        RegiMentesekTorlese(FilePath);
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(ex.Message);
                    }
                    settings.LastSaved = DateTime.Now;
                    return Results.Ok("Adatok mentése megtörtént.");
                }
                else
                {
                    return Results.Ok("Nem volt adatváltozás.");
                }
            });
            app.MapPost("/load", async (DataSaving saving, Settings settings) =>
            {
                try
                {
                    var latest = LegujabbMentes(FilePath);
                    if (latest is null)
                        return Results.Ok("Nincs mentett adatfájl.");
                    await saving.LoadFromJsonAsync(latest);
                    settings.LastModify = DateTime.Now;
                    return Results.Ok("Adatok sikeresen visszatöltve.");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });
            #endregion
            app.MapGet("/status", (Settings settings) =>
            {
                return Results.Ok(new { settings.LastModify, settings.LastSaved });
            });
            app.MapGet("/files", () =>
            {
                var files = Directory.GetFiles(FilePath).Select(f => new
                {
                    FileName = Path.GetFileName(f),
                    CreationTime = File.GetCreationTime(f)
                }).OrderByDescending(f => f.CreationTime).ToList();
                return Results.Ok(files);
            });
            app.MapPost("/load/{fileName}", async (string fileName, DataSaving saving, Settings settings) =>
            {
                try
                {
                    var filePath = BiztonsagosUtvonal(FilePath, fileName);
                    if (filePath is null)
                        return Results.BadRequest("Érvénytelen fájlnév.");
                    if (!File.Exists(filePath))
                        return Results.NotFound("A megadott fájl nem található.");
                    await saving.LoadFromJsonAsync(filePath);
                    settings.LastModify = DateTime.Now;
                    return Results.Ok("Adatok sikeresen visszatöltve.");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });
            app.MapDelete("/delete/{fileName}", (string fileName) =>
            {
                try
                {
                    var filePath = BiztonsagosUtvonal(FilePath, fileName);
                    if (filePath is null)
                        return Results.BadRequest("Érvénytelen fájlnév.");
                    if (!File.Exists(filePath))
                        return Results.NotFound("A megadott fájl nem található.");
                    File.Delete(filePath);
                    return Results.Ok("Fájl sikeresen törölve.");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });
            app.MapGet("/mentesek/latest", () =>
            {
                var latestFile = LegujabbMentes(FilePath);
                if (latestFile is null)
                    return Results.NotFound("Nincs mentett adatfájl.");
                return Results.File(latestFile, "application/json", Path.GetFileName(latestFile));
            });
            app.MapGet("/mentesek/{fileName}", (string fileName) =>
            {
                var filePath = BiztonsagosUtvonal(FilePath, fileName);
                if (filePath is null)
                    return Results.BadRequest("Érvénytelen fájlnév.");
                if (!File.Exists(filePath))
                    return Results.NotFound("A megadott fájl nem található.");
                return Results.File(filePath, "application/json", Path.GetFileName(filePath));
            });
            app.MapPost("/clear", async (DataSaving saving) =>
            {
                try
                {
                    await saving.ClearDatabaseAsync();
                    return Results.Ok("Adatok sikeresen törölve.");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });
            app.MapPost("/clearnewyear", async (DataSaving saving) =>
            {
                try
                {
                    await saving.ClearDatabaseNewYearAsync();
                    return Results.Ok("Adatok sikeresen törölve.");
                }
                catch (Exception ex)
                {
                    return Results.Problem(ex.Message);
                }
            });
            app.MapPost("/upload", async (HttpRequest request) =>
            {
                if (!request.HasFormContentType)
                    return Results.BadRequest("Invalid file.");
                var form = await request.ReadFormAsync();
                if (form.Files.Count > 0)
                {
                    var file = form.Files[0];
                    var filePath = BiztonsagosUtvonal(FilePath, file.FileName);
                    if (filePath is null)
                        return Results.BadRequest("Csak .json fájl tölthető fel, érvényes névvel.");

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Results.Ok(new { FileName = Path.GetFileName(filePath) });
                }
                return Results.BadRequest("Invalid file.");
            });
        }

        /// <summary>
        /// A fájlnévből a mentési mappán belüli teljes útvonalat adja, vagy null-t, ha a név érvénytelen
        /// (könyvtárrészt tartalmaz, kilépne a mappából, vagy nem .json).
        /// </summary>
        private static string? BiztonsagosUtvonal(string mappa, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return null;
            var nev = Path.GetFileName(fileName);
            if (nev != fileName || nev is "." or "..") return null;
            if (!string.Equals(Path.GetExtension(nev), ".json", StringComparison.OrdinalIgnoreCase)) return null;
            var teljes = Path.GetFullPath(Path.Combine(mappa, nev));
            var gyoker = mappa.EndsWith(Path.DirectorySeparatorChar) ? mappa : mappa + Path.DirectorySeparatorChar;
            return teljes.StartsWith(gyoker, StringComparison.Ordinal) ? teljes : null;
        }

        private static string? LegujabbMentes(string mappa)
        {
            return Directory.GetFiles(mappa, "*.json").OrderByDescending(File.GetLastWriteTime).FirstOrDefault();
        }

        private static void RegiMentesekTorlese(string mappa)
        {
            foreach (var file in Directory.GetFiles(mappa, "*.json").OrderByDescending(File.GetLastWriteTime).Skip(MegtartottMentesek))
            {
                File.Delete(file);
            }
        }
    }
}
