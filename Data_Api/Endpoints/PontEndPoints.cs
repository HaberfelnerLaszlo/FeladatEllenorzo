using Data_Api.Data;
using Data_Api.Services;

namespace Data_Api.Endpoints
{
    public static class PontEndPoints
    {
        public static void AddPontEndPoints(this WebApplication app)
        {
            app.MapPost("/pont", async (Pont pont, PontService pontService) =>
            {
                var createdPont = await pontService.CreatePont(pont);
                var newPont = (Pont)createdPont.Content;
                if (createdPont.IsSuccess == false)
                {
                    return Results.BadRequest(createdPont.ErrorMessage);
                }
                if (newPont is null)
                {
                    return Results.BadRequest("Hiba történt a pont létrehozásakor.");
                }
                return Results.Created($"/pont/{newPont.Id}", createdPont);
            });
            //Az id az osztályneve és nem az osztály id-je
            app.MapGet("/pont/osztaly/{id}", async (string id,PontService pontService) =>
            {
                var pontok = await pontService.GetPontsByOsztaly(id);
                return Results.Ok(pontok);
            });
            app.MapGet("/pont/{id}", async (int id, PontService pontService) =>
            {
                var pont = await pontService.GetPontById(id);
                return pont is not null ? Results.Ok(pont) : Results.NotFound();
            });
            app.MapGet("/pont/tanulo/{id}", async (Guid id, PontService pontService) =>
            {
                var pontok = await pontService.GetAllPontsByTanuloId(id);
                return Results.Ok(pontok);
            });
            //app.MapDelete("/pont/{id}", async (int id, PontService pontService) =>
            //{
            //    var deleted = await pontService.DeletePont(id);
            //    return deleted ? Results.NoContent() : Results.NotFound();
            //});
        }
    }
}
