using Data_Api.Data;
using Data_Api.Services;

namespace Data_Api.Endpoints
{
    public static class TanuloEndPoints
    {
        public static void AddTanuloEndpoints(this WebApplication app)
        {
            #region Tanulo
            app.MapGet("/tanulo", async (TanuloService tanuloService) =>
            {
                return await tanuloService.GetTanulok();
            });
            app.MapGet("/tanulo/{id}", async (TanuloService tanuloService, Guid id) =>
            {
                return await tanuloService.GetTanulo(id);
            });
            app.MapGet("/tanulokids", async (TanuloService tanuloService) =>
            {
                return await tanuloService.GetTanulokToIds();
            });
            app.MapGet("/tanulokids/{osztaly}", async (TanuloService tanuloService, string osztaly) =>
            {
                return await tanuloService.GetTanulokToIds(osztaly);
            });
            app.MapPost("/tanulo", async (TanuloService tanuloService, Tanulo tanulo) =>
            {
                return await tanuloService.CreateTanulo(tanulo);
            });
            app.MapPut("/tanulo/{id}", async (TanuloService tanuloService, Guid id, Tanulo tanulo) =>
            {
                return await tanuloService.UpdateTanulo(id, tanulo);
            });
            app.MapDelete("/tanulo/{id}", async (TanuloService tanuloService, Guid id) =>
            {
                return await tanuloService.DeleteTanulo(id);
            });
            #endregion
        }
    }
}
