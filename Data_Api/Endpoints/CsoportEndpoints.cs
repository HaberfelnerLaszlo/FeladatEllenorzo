using Data_Api.Data;
using Data_Api.Services;
namespace Data_Api.Endpoints
{
    public static class CsoportEndpoints
    {
        public static void AddCsoportEndpoints(this WebApplication app)
        {
            #region Csoport
            app.MapGet("/csoport", async (CsoportService csoportService) =>
            {
                return await csoportService.GetCsoportok();
            });
            app.MapGet("/csoport/{id}", async (CsoportService csoportService, Guid id) =>
            {
                return await csoportService.GetCsoport(id);
            });
            app.MapPost("/csoport", async (CsoportService csoportService, Csoport csoport) =>
            {
                return await csoportService.CreateCsoport(csoport);
            });
            app.MapPut("/csoport/{id}", async (CsoportService csoportService, Guid id, Csoport csoport) =>
            {
                return await csoportService.UpdateCsoport(id, csoport);
            });
            app.MapDelete("/csoport/{id}", async (CsoportService csoportService, Guid id) =>
            {
                return await csoportService.DeleteCsoport(id);
            });
            #endregion
        }
    }
}
