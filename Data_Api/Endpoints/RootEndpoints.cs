using Data_Api.Data;
using Data_Api.Services;

namespace Data_Api.Endpoints
{
    public static class RootEndpoints
    {
        public static void AddEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => { return "version:6.5.1 2025.09.23"; });
        }
    }
}
