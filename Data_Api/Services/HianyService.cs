using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Data_Api.Services
{
    public class HianyService(FeladatDb db, Settings settings)
    {
        MainResponse response = new MainResponse();

        public async Task<List<FeladatHiany>> GetFeladatHianyok()
        {
            return await db.FeladatHianyok.ToListAsync();
        }
        public List<FeladatHiany> GetFeladatHianyByFeladat(Guid feladatId)
        {
            return [.. db.FeladatHianyok.Where(h => h.FeladatId == feladatId)];
        }
        public async Task<MainResponse> GetFeladatHianyByOsztaly_Ma(string osztaly)
        {
            response.Clear();
            var hianyok = await db.Tanulok.Where(t=>t.Osztaly==osztaly)
                                                            .Include(h=>h.Hianyok.Where(h=>h.Datum==DateOnly.Parse(DateTime.Today.ToShortDateString())))
                                                            .ToListAsync();
            if (hianyok == null)
            {
                response.ErrorMessage = "Nincs hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = hianyok;
            }
            return response;
        }
        public async Task<MainResponse> GetFeladatHianyByMa()
        {
            response.Clear();
            var hianyok = await db.Tanulok.Include(h=>h.Hianyok.Where(h=>h.Datum==DateOnly.Parse(DateTime.Today.ToShortDateString()))).ToListAsync();
            if (hianyok == null)
            {
                response.ErrorMessage = "Nincs hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = hianyok;
            }
            return response;
        }
        public async Task<MainResponse> GetFeladatHianyByOsztaly_Datum(string osztaly, string date)
        {
            response.Clear();
            var hianyok = await db.Tanulok.Where(t=>t.Osztaly==osztaly)
                                                            .Include(h=>h.Hianyok.Where(h=>h.Datum==DateOnly.Parse(date)))
                                                            .ToListAsync();
            if (hianyok == null)
            {
                response.ErrorMessage = "Nincs hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = hianyok;
            }
            return response;
        }
        public async Task<MainResponse> GetFeladatHianytByDatum(string date)
        {
            response.Clear();
            var hianyok = await db.Tanulok.Include(h=>h.Hianyok.Where(h=>h.Datum==DateOnly.Parse(date))).ToListAsync();
            if (hianyok == null)
            {
                response.ErrorMessage = "Nincs hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = hianyok;
            }
            return response;
        }
        public async Task<MainResponse> CreateFeladatHiany(FeladatHiany hiany)
        {
            response.Clear();
            try
            {
                db.FeladatHianyok.Add(hiany);
                db.Pontok.Add(new Pont() { TanuloId = hiany.TanuloId, Jegyzet = hiany.FeladatId.ToString(), PontSzam = -2,PontTipus=PontTipus.Lecke });
                var tanulo = db.Tanulok.FirstOrDefault(t => t.Id == hiany.TanuloId);
                if (tanulo != null) tanulo.Pont -= 2;
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = hiany;
                settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public async Task<MainResponse> UpdateFeladatHiany(int id, FeladatHiany hiany)
        {
            try
            {
                response.Clear();
                if (id != hiany.Id)
                {
                    response.ErrorMessage = "Nem egyezik az azonosító.";
                    response.IsSuccess = false;
                    return response;
                }
                db.Entry(hiany).State = EntityState.Modified;
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = hiany;
                settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public async Task<MainResponse> DeleteFeladatHiany(int id)
        {
            response.Clear();
            try
            {
                var feladatHiany = await db.FeladatHianyok.FindAsync(id);
                if (feladatHiany == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                db.FeladatHianyok.Remove(feladatHiany);
                db.Pontok.Add(new Pont() { TanuloId = feladatHiany.TanuloId, Jegyzet = "Lecke hiány törlés ID: "+ feladatHiany.FeladatId.ToString(), PontSzam = 2, PontTipus = PontTipus.Lecke });
                var tanulo= db.Tanulok.FirstOrDefault(t => t.Id == feladatHiany.TanuloId);
                if (tanulo != null) tanulo.Pont += 2;
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public async Task<MainResponse> DeleteFeladatHianyByTanulo(Guid id)
        {
            response.Clear();
            try
            {
                var hianyok = await db.FeladatHianyok.Where(h => h.TanuloId == id).ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                db.FeladatHianyok.RemoveRange(hianyok);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public async Task<MainResponse> DeleteFeladatHianyByOsztaly(string osztaly)
        {
            response.Clear();
            try
            {
                var hianyok = await db.FeladatHianyok.Where(h => h.Osztaly == osztaly).ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                db.FeladatHianyok.RemoveRange(hianyok);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public async Task<MainResponse> DeleteFeladatHianyByDatum(string date)
        {
            response.Clear();
            try
            {
                var hianyok = await db.FeladatHianyok.Where(h => h.Datum == DateOnly.Parse(date)).ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                db.FeladatHianyok.RemoveRange(hianyok);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public async Task<MainResponse> DeleteFeladatHianyByOsztalyDatum(string osztaly, string date)
        {
            response.Clear();
            try
            {
                var hianyok = await db.FeladatHianyok.Where(h => h.Osztaly == osztaly && h.Datum == DateOnly.Parse(date)).ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                db.FeladatHianyok.RemoveRange(hianyok);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public async Task<MainResponse> DeleteAllFeladatHiany()
        {
            response.Clear();
            try
            {
                var hianyok = await db.FeladatHianyok.ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = "Nincsenek hibás feladatok.";
                    response.IsSuccess = false;
                    return response;
                }
                db.FeladatHianyok.RemoveRange(hianyok);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public async Task<MainResponse> DeleteClassFeladatHiany(string osztaly, string date)
        {
            response.Clear();
            try
            {
                var hianyok = await db.FeladatHianyok.Where(h => h.Osztaly == osztaly && h.Datum == DateOnly.Parse(date)).ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = $"Nincsenek a {osztaly} hibás feladatai.";
                    response.IsSuccess = false;
                    return response;
                }
                db.FeladatHianyok.RemoveRange(hianyok);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
    }
}
