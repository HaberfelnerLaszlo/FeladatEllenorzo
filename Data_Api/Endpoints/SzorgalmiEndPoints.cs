using Data_Api.Data;
using Data_Api.Services;

namespace Data_Api.Endpoints
{
    public static class SzorgalmiEndPoints
    {
        public static void AddSzorgalmiEndPoints(this WebApplication app)
        {
            app.MapGet("/szorgalmik/{osztaly}", async (string osztaly, SzorgalmiService szorgalmiService) =>
            {
                return await szorgalmiService.GetSzorgalmik(osztaly);
            });
            app.MapGet("/szorgalmi_nev/{nev}", async (string nev, SzorgalmiService szorgalmiService) =>
            {
                return await szorgalmiService.GetSzorgalmikByNev(nev);
            });
            app.MapPost("/szorgalmi", (Szorgalmi szorgalmi, SzorgalmiService szorgalmiService) =>
            {
                return szorgalmiService.CreateSzorgalmi(szorgalmi);
            });
            app.MapPut("/szorgalmi/{id}", (int id, Szorgalmi inputSzorgalmi, SzorgalmiService szorgalmiService) =>
            {
                return szorgalmiService.UpdateSzorgalmi(id, inputSzorgalmi);
            });
            app.MapDelete("/szorgalmi/{id}", (int id, SzorgalmiService szorgalmiService) =>
            {
                return szorgalmiService.DeleteSzorgalmi(id);
            });
        }
    }
}
