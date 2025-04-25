using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Data_Api.Services
{
    public class HianyService
    {
        //private readonly FeladatSQL _db;
        private readonly FeladatDb _db;
        MainResponse response = new MainResponse();
        //public HianyService(FeladatSQL db)
        public HianyService(FeladatDb db)
        {
            _db = db;
        }
        public async Task<List<FeladatHiany>> GetFeladatHianyok()
        {
            return await _db.FeladatHianyok.ToListAsync();
        }
        public async Task<MainResponse> GetFeladatHianyByTanulo(Guid id)
        {
            response.Clear();
            var hianyok = await _db.Tanulok.Where(t=>t.Id==id).Include(h=>h.Hianyok).ToListAsync();
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
        public async Task<MainResponse> GetFeladatHianyByOsztaly_Ma(string osztaly)
        {
            response.Clear();
            var hianyok = await _db.Tanulok.Where(t=>t.Osztaly==osztaly)
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
            var hianyok = await _db.Tanulok.Include(h=>h.Hianyok.Where(h=>h.Datum==DateOnly.Parse(DateTime.Today.ToShortDateString()))).ToListAsync();
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
            var hianyok = await _db.Tanulok.Where(t=>t.Osztaly==osztaly)
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
            var hianyok = await _db.Tanulok.Include(h=>h.Hianyok.Where(h=>h.Datum==DateOnly.Parse(date))).ToListAsync();
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
                _db.FeladatHianyok.Add(hiany);
                await _db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = hiany;
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
                _db.Entry(hiany).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                response.IsSuccess = true;
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
                var feladatHiany = await _db.FeladatHianyok.FindAsync(id);
                if (feladatHiany == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                _db.FeladatHianyok.Remove(feladatHiany);
                await _db.SaveChangesAsync();
                response.IsSuccess = true;
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
                var hianyok = await _db.FeladatHianyok.Where(h => h.TanuloId == id).ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                _db.FeladatHianyok.RemoveRange(hianyok);
                await _db.SaveChangesAsync();
                response.IsSuccess = true;
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
                var hianyok = await _db.FeladatHianyok.Where(h => h.Osztaly == osztaly).ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                _db.FeladatHianyok.RemoveRange(hianyok);
                await _db.SaveChangesAsync();
                response.IsSuccess = true;
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
                var hianyok = await _db.FeladatHianyok.Where(h => h.Datum == DateOnly.Parse(date)).ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                _db.FeladatHianyok.RemoveRange(hianyok);
                await _db.SaveChangesAsync();
                response.IsSuccess = true;
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
                var hianyok = await _db.FeladatHianyok.Where(h => h.Osztaly == osztaly && h.Datum == DateOnly.Parse(date)).ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                _db.FeladatHianyok.RemoveRange(hianyok);
                await _db.SaveChangesAsync();
                response.IsSuccess = true;
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
                var hianyok = await _db.FeladatHianyok.ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = "Nincsenek hibás feladatok.";
                    response.IsSuccess = false;
                    return response;
                }
                _db.FeladatHianyok.RemoveRange(hianyok);
                await _db.SaveChangesAsync();
                response.IsSuccess = true;
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
                var hianyok = await _db.FeladatHianyok.Where(h => h.Osztaly == osztaly && h.Datum == DateOnly.Parse(date)).ToListAsync();
                if (hianyok == null)
                {
                    response.ErrorMessage = $"Nincsenek a {osztaly} hibás feladatai.";
                    response.IsSuccess = false;
                    return response;
                }
                _db.FeladatHianyok.RemoveRange(hianyok);
                await _db.SaveChangesAsync();
                response.IsSuccess = true;
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
