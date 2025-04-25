using Data_Api.Data;
using Data_Api.Services;

namespace Data_Api.Endpoints
{
    public static class SzovegEndPoints
    {
        public static void AddSzovegEndPoints(this WebApplication app)
        {
            app.MapGet("/szoveg", async (SzovegService szovegService) =>
            {
                return await szovegService.GetSzovegek();
            });
            app.MapPost("/szoveg", async (Szoveg szoveg, SzovegService szovegService) =>
            {
                return await szovegService.CreateSzoveg(szoveg);
            });
            app.MapPut("/szoveg/{id}", async (int id, Szoveg szoveg, SzovegService szovegService) =>
            {
                return await szovegService.UpdateSzoveg(id, szoveg);
            });
            app.MapDelete("/szoveg/{id}", async (int id, SzovegService szovegService) =>
            {
                return await szovegService.DeleteSzoveg(id);
            });
        }
    }
}
