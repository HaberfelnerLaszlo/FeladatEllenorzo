using Data_Api.Data;
using Data_Api.Services;

namespace Data_Api.Endpoints
{
    public static class HibaEndPoints
    {
        public static void AddHibaEndPoints(this WebApplication app)
        {
            app.MapGet("/maihibak", async (HibaService hibaService) =>
            {
                return await hibaService.GetHibasFeladatByMa();
            });
            app.MapGet("/hibak/{date}", async (string date, HibaService hibaService) =>
            {
                return await hibaService.GetHibasFeladatByDatum(date);
            });
            app.MapPost("/hiba", async (HibasFeladat hiba, HibaService hibaService) =>
            {
                return await hibaService.CreateHibasFeladat(hiba);
            });
            app.MapPut("/hiba/{id}", async (int id, HibasFeladat inputHiba, HibaService hibaService) =>
            {
                return await hibaService.UpdateHibasFeladat(id, inputHiba);
            });
            app.MapDelete("/hiba/{id}", async (int id, HibaService hibaService) =>
            {
                return await hibaService.DeleteHibasFeladat(id);
            });
            app.MapDelete("/allhiba", async (HibaService hibaService) =>
            {
                return await hibaService.DeleteAllHibasFeladat();
            });
            app.MapDelete("/classhiba/{osztaly}/{date}", async (string osztaly, string date, HibaService hibaService) =>
            {
                return await hibaService.DeleteClassHibasFeladat(osztaly, date);
            });
        }
    }
}
