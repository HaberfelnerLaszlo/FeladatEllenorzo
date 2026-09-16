using Data_Api.Data;
using Data_Api.Services;

namespace Data_Api.Endpoints
{
    public static class RootEndpoints
    {
        public static void AddEndpoints(this WebApplication app)
        {
<<<<<<< Updated upstream
            app.MapGet("/", () => { return "version:6.6.3 2026.01.17"; });
=======
            app.MapGet("/", () => { return "version:7.1.1 2026.09.11"; });
>>>>>>> Stashed changes
        }
    }
}
