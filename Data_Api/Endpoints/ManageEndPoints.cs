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
        }
    }
}
