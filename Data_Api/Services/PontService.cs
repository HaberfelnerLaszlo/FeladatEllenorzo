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
                var i = await context.SaveChangesAsync();
                if (i == 0)
                {
                    response.ErrorMessage = "Nincs módosítva a tanuló pontja.";
                    response.IsSuccess = false;
                    return response;
                }
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
            var pontok = await context.Pontok.Where(p => p.TanuloId == id && p.IsDeleted == false).ToListAsync();
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
            var tanulok = await context.Tanulok
                .Where(t => t.Osztaly == osztaly)
                .Include(t => t.Pontok) // Correctly specify the navigation property to include
                .ToListAsync();

            if (tanulok == null || !tanulok.Any())
            {
                response.ErrorMessage = "Nincs pont tárolva.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                tanulok.ForEach(t => t.Pontok = [.. t.Pontok.Where(p => p.IsDeleted == false).TakeLast(25)]);
                response.IsSuccess = true;
                response.Content = tanulok;
                return response;
            }
        }
        public async Task<MainResponse> GetAllPonts()
        {
            response.Clear();
            var pontok = await context.Pontok.Where(p => p.IsDeleted == false).ToListAsync();
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
        public async Task<MainResponse> UpdatePont(Pont pont)
        {
            response.Clear();
            var existingPont = await context.Pontok.FindAsync(pont.Id);
            if (existingPont == null)
            {
                response.ErrorMessage = "Pont not found.";
                response.IsSuccess = false;
                return response;
            }
            var pontLog = new PontLog
            {
                PontId = existingPont.Id,
                Datum = DateTime.Now,
                Valtozas = $"Pont változott eredeti: {existingPont}, új: {pont}"
            };
            var valtozasPontSzam = pont.PontSzam - existingPont.PontSzam;
            // Update fields
            existingPont.Datum = pont.Datum;
            existingPont.PontSzam = pont.PontSzam;
            existingPont.Jegyzet = pont.Jegyzet;
            existingPont.PontTipus = pont.PontTipus;
            existingPont.IsDeleted = pont.IsDeleted;
            var i=await context.SaveChangesAsync();
            if(i==0)
            {
                response.ErrorMessage = "Nincs módosítva a pont.";
                response.IsSuccess = false;
                return response;
            }
            response.IsSuccess = true;
            response.Content = existingPont;
            settings.LastModify = DateTime.Now;
            var tanulo = await context.Tanulok.FindAsync(existingPont.TanuloId);
            if (tanulo != null)
            {
                tanulo.Pont += valtozasPontSzam;
                i = await context.SaveChangesAsync();
                if (i == 0)
                {
                    response.ErrorMessage = "Nincs módosítva a tanuló pontja. A változás csak a pontnál történt meg.";
                    response.IsSuccess = false;
                }
            }
            context.PontLogok.Add(pontLog);
            i = await context.SaveChangesAsync();
            if (i == 0)
            {
                response.ErrorMessage = "Nincs naplózva a pont módosítás.";
                response.IsSuccess = false;
                return response;
            }
            return response;
        }
        public async Task<bool> DeletePont(int id)
        {
            var pont = await context.Pontok.FindAsync(id);
            if (pont == null) return false;
            pont.IsDeleted = true;
            var tanulo = await context.Tanulok.FindAsync(pont.TanuloId);
            if (tanulo != null)
            {
                tanulo.Pont -= pont.PontSzam;
            }
            var i = await context.SaveChangesAsync();
            return i > 0;
        }
    }
}
