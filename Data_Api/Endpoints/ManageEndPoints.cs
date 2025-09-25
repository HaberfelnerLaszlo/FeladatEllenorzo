using Data_Api.Services;

namespace Data_Api.Endpoints
{
    public static class ManageEndPoints
    {
        public static void AddManageEndpoints(this WebApplication app)
        {
            IWebHostEnvironment hostingEnv = app.Environment;
            string FilePath = Path.Combine(hostingEnv.ContentRootPath, "Mentesek");
            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);
            else {
                var filesCount=Directory.EnumerateFiles(FilePath).Count();
                if (filesCount > 5)
                {
                    var files = Directory.GetFiles(FilePath);
                    foreach (var file in files.OrderByDescending(f => File.GetCreationTime(f)).Skip(5))
                    {
                        File.Delete(file);
                    }
                }
            }
            #region Manage
            app.MapGet("/save", async (DataSaving saving, Settings settings) =>
            {
                if(settings.LastModify>settings.LastSaved)
                {
                    try 
                    {
                        await saving.SaveToJsonAsync(Path.Combine(FilePath,$"mentes-{DateTime.Today.ToString("s")}.json"));
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
            app.MapGet("/load", async (DataSaving saving, Settings settings) =>
            {
                try
                {
                    var files = Directory.GetFiles(FilePath);
                    if (files.Length == 0)
                        return Results.Ok("Nincs mentett adatfájl.");
                    await saving.LoadFromJsonAsync(files.OrderByDescending(f => File.GetCreationTime(f)).First().ToString());
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
            app.MapGet("/load/{fileName}", async (string fileName, DataSaving saving, Settings settings) =>
            {
                try
                {
                    var filePath = Path.Combine(FilePath, fileName);
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
            app.MapGet("/delete/{fileName}", (string fileName) =>
            {
                try
                {
                    var filePath = Path.Combine(FilePath, fileName);
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
            app.MapGet(FilePath + "/{fileName}", (string fileName) =>
            {
                var filePath = Path.Combine(FilePath, fileName);
                if (!File.Exists(filePath))
                    return Results.NotFound("A megadott fájl nem található.");
                var mimeType = "application/json";
                return Results.File(filePath, mimeType, fileName);
            });
            app.MapGet(FilePath + "/latest", () =>
            {
                var files = Directory.GetFiles(FilePath);
                if (files.Length == 0)
                    return Results.NotFound("Nincs mentett adatfájl.");
                var latestFile = files.OrderByDescending(f => File.GetCreationTime(f)).First();
                var mimeType = "application/json";
                return Results.File(latestFile, mimeType, Path.GetFileName(latestFile));
            });
            app.MapGet("/clear", async (DataSaving saving) =>
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
        }
    }
}
