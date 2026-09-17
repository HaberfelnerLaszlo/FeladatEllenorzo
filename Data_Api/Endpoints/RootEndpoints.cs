using Data_Api.Data;
using Data_Api.Services;

namespace Data_Api.Endpoints
{
    public static class RootEndpoints
    {
        public static void AddEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => { return "version:7.0.0 2026.09.10"; });
        }
    }
}
