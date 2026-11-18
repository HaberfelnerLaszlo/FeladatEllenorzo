using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Data_Api.Services
{
    public class PontsService(FeladatSQL context, Settings settings)
    {
        MainResponse response = new MainResponse();

        public async Task<MainResponse> CreatePont(Pont pont)
        {
            response.Clear();
            var tanulo = context.Find<Tanulo>(pont.TanuloId);
            if (tanulo != null)
            {
                // Ugyanazt a lecke-pontot (azonos feladat-jegyzet) ne lehessen kétszer jóváírni.
                if (pont.PontTipus == PontTipus.Lecke && !string.IsNullOrWhiteSpace(pont.Jegyzet))
                {
                    var marVan = await context.Pontok.AnyAsync(p => p.TanuloId == pont.TanuloId && p.PontTipus == PontTipus.Lecke
                                                                 && p.Jegyzet == pont.Jegyzet && !p.IsDeleted);
                    if (marVan)
                    {
                        response.ErrorMessage = "Erre a feladatra a tanuló már kapott lecke pontot.";
                        response.IsSuccess = false;
                        return response;
                    }
                }
                context.Pontok.Add(pont);
                if (!pont.IsDeleted) tanulo.Pont += pont.PontSzam;
                tanulo.LastModify = DateTime.Now;
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
                tanulok.ForEach(t => t.Pontok = [.. t.Pontok.Where(p => p.IsDeleted == false).OrderBy(p => p.Datum).ThenBy(p => p.Id).TakeLast(25)]);
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
            // A tanuló összpontjába csak a nem törölt pontok számítanak bele,
            // így a törlés/visszaállítás (IsDeleted váltás) is korrigálja az összeget.
            var regiErtek = existingPont.IsDeleted ? 0 : existingPont.PontSzam;
            var ujErtek = pont.IsDeleted ? 0 : pont.PontSzam;
            var valtozasPontSzam = ujErtek - regiErtek;
            // Update fields
            existingPont.Datum = pont.Datum;
            existingPont.PontSzam = pont.PontSzam;
            existingPont.Jegyzet = pont.Jegyzet;
            existingPont.PontTipus = pont.PontTipus;
            existingPont.IsDeleted = pont.IsDeleted;
            var tanulo = await context.Tanulok.FindAsync(existingPont.TanuloId);
            if (tanulo != null)
            {
                tanulo.Pont += valtozasPontSzam;
                tanulo.LastModify = DateTime.Now;
            }
            context.PontLogok.Add(pontLog);
            // Egyetlen SaveChanges = egy tranzakció: vagy minden módosul, vagy semmi.
            var i = await context.SaveChangesAsync();
            if (i == 0)
            {
                response.ErrorMessage = "Nincs módosítva a pont.";
                response.IsSuccess = false;
                return response;
            }
            response.IsSuccess = true;
            response.Content = existingPont;
            settings.LastModify = DateTime.Now;
            return response;
        }
        public async Task<bool> DeletePont(int id)
        {
            var pont = await context.Pontok.FindAsync(id);
            // Már törölt pontot nem vonunk le még egyszer.
            if (pont == null || pont.IsDeleted) return false;
            pont.IsDeleted = true;
            var tanulo = await context.Tanulok.FindAsync(pont.TanuloId);
            if (tanulo != null)
            {
                tanulo.Pont -= pont.PontSzam;
                tanulo.LastModify = DateTime.Now;
            }
            context.PontLogok.Add(new PontLog { PontId = pont.Id, Datum = DateTime.Now, Valtozas = $"Pont törölve: {pont}" });
            var i = await context.SaveChangesAsync();
            if (i > 0) settings.LastModify = DateTime.Now;
            return i > 0;
        }
    }
}
