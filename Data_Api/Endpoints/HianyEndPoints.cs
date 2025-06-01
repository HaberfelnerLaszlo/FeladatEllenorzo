using Data_Api.Data;
using Data_Api.Services;

namespace Data_Api.Endpoints
{
    public static class HianyEndPoints
    {
        public static void AddHianyEndPoints(this WebApplication app)
        {
            app.MapGet("/maihianyok", async (HianyService hianyService) =>
            {
                return await hianyService.GetFeladatHianyByMa();
            });
            app.MapGet("/hianyok/{date}", async (string date, HianyService hianyService) =>
            {
                return await hianyService.GetFeladatHianytByDatum(date);
            });
            app.MapGet("/hiany/{feladatId}", (string feladatId, HianyService hianyService) =>
            {
                return hianyService.GetFeladatHianyByFeladat(Guid.Parse(feladatId));
            });
            app.MapPost("/hiany", async (FeladatHiany hiany, HianyService hianyService) =>
            {
                return await hianyService.CreateFeladatHiany(hiany);
            });
            app.MapPut("/hiany/{id}", async (int id, FeladatHiany inputHiany, HianyService hianyService) =>
            {
                return await hianyService.UpdateFeladatHiany(id, inputHiany);
            });
            app.MapDelete("/hiany/{id}", async (int id, HianyService hianyService) =>
            {
                return await hianyService.DeleteFeladatHiany(id);
            });
            app.MapDelete("/allhiany", async (HianyService hianyService) =>
            {
                return await hianyService.DeleteAllFeladatHiany();
            });
            app.MapDelete("/classhiany/{osztaly}/{date}", async (string osztaly, string date, HianyService hianyService) =>
            {
                return await hianyService.DeleteClassFeladatHiany(osztaly, date);
            });
        }
    }
}
