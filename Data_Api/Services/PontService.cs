using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Data_Api.Services
{
    public class PontService
    {
        private readonly FeladatDb _context;
        MainResponse response = new MainResponse();
        public PontService(FeladatDb context)
        {
            _context = context;
        }
        public async Task<MainResponse> CreatePont(Pont pont)
        {
            response.Clear();
            var tanulo = _context.Find<Tanulo>(pont.TanuloId);
            if (tanulo != null)
            {
               _context.Pontok.Add(pont);
                tanulo.Pont += pont.PontSzam; // Adjust logic as needed, e.g., += pont.PontSzam
                await _context.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = pont;
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
            var pontok = await _context.Pontok.Where(p => p.TanuloId == id).ToListAsync();
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
            var pont = await _context.Pontok.FindAsync(id);
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
            var pontok = await _context.Tanulok
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
