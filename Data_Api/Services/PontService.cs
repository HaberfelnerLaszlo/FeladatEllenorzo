using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Data_Api.Services
{
    public class PontService(FeladatDb context, Settings settings)
    {
        MainResponse response = new MainResponse();

        public async Task<MainResponse> CreatePont(Pont pont)
        {
            response.Clear();
            var tanulo = context.Find<Tanulo>(pont.TanuloId);
            if (tanulo != null)
            {
               context.Pontok.Add(pont);
                tanulo.Pont += pont.PontSzam; // Adjust logic as needed, e.g., += pont.PontSzam
                await context.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = pont;
                settings.LastModify = DateTime.Now;
                return response;
            }
            else
            {
                response.ErrorMessage = "Tanulo not found.";
                response.IsSuccess = false;
                return response;
            }
        }
        public async Task<MainResponse> GetAllPontsByTanuloId(Guid id)
        {
            response.Clear();
            var pontok = await context.Pontok.Where(p => p.TanuloId == id).ToListAsync();
            if (pontok == null)
            {
                response.ErrorMessage = "Nincs pont tárolva.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = pontok;
                return response;
            }
        }
        public async Task<MainResponse> GetPontById(int id)
        {
            response.Clear();
            var pont = await context.Pontok.FindAsync(id);
            if (pont == null)
            {
                response.ErrorMessage = "Nincs pont tárolva.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = pont;
                return response;
            }
        }
        public async Task<MainResponse> GetPontsByOsztaly(string osztaly)
        {
            response.Clear();
            var pontok = await context.Tanulok
                .Where(t => t.Osztaly == osztaly)
                .Include(t => t.Pontok.Take(25)) // Correctly specify the navigation property to include
                .ToListAsync();

            if (pontok == null || !pontok.Any())
            {
                response.ErrorMessage = "Nincs pont tárolva.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = pontok;
                return response;
            }
        }
        //public async Task<bool> DeletePont(int id)
        //{
        //    var pont = await GetPontById(id);
        //    if (pont == null) return false;
        //    _context.Pontok.Remove(pont);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}
    }
}
