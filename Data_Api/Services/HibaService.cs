using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Data_Api.Services
{
    public class HibaService(FeladatDb db, Settings settings)
    {
        MainResponse response = new MainResponse();

        public async Task<List<HibasFeladat>> GetHibasFeladatok()
        {
            return await db.HibasFeladatok.ToListAsync();
        }
        public async Task<MainResponse> GetHibasFeladatByTanulo(Guid id)
        {
            response.Clear();
            var hibasFeladat = await db.Tanulok.Where(t => t.Id == id).Include(h => h.Hibak).ToListAsync();
            if (hibasFeladat == null)
            {
                response.ErrorMessage = "Nincs hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = hibasFeladat;
            }
            return response;
        }
        public async Task<MainResponse> GetHibasFeladatByOsztaly_Ma(string osztaly)
        {
            response.Clear();
            var hibasFeladat = await db.Tanulok.Where(t => t.Osztaly == osztaly)
                                                            .Include(h => h.Hibak.Where(h => h.Datum == DateOnly.Parse(DateTime.Today.ToShortDateString())))
                                                            .ToListAsync();
            if (hibasFeladat == null)
            {
                response.ErrorMessage = "Nincs hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = hibasFeladat;
            }
            return response;
        }
        public async Task<MainResponse> GetHibasFeladatByMa()
        {
            response.Clear();
            var hibasFeladat = await db.Tanulok.Include(h => h.Hibak.Where(h => h.Datum == DateOnly.Parse(DateTime.Today.ToShortDateString()))).ToListAsync();
            if (hibasFeladat == null)
            {
                response.ErrorMessage = "Nincs hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = hibasFeladat;
            }
            return response;
        }
        public async Task<MainResponse> GetHibasFeladatByOsztaly_Datum(string osztaly, string date)
        {
            response.Clear();
            var hibasFeladat = await db.Tanulok.Where(t => t.Osztaly == osztaly)
                                                            .Include(h => h.Hibak.Where(h => h.Datum == DateOnly.Parse(date)))
                                                            .ToListAsync();
            if (hibasFeladat == null)
            {
                response.ErrorMessage = "Nincs hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = hibasFeladat;
            }
            return response;
        }
        public async Task<MainResponse> GetHibasFeladatByDatum(string date)
        {
            response.Clear();
            var hibasFeladat = await db.Tanulok.Include(h => h.Hibak.Where(h => h.Datum == DateOnly.Parse(date))).ToListAsync();
            if (hibasFeladat == null)
            {
                response.ErrorMessage = "Nincs hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.Content = hibasFeladat;
            }
            return response;
        }
        public async Task<MainResponse> CreateHibasFeladat(HibasFeladat hibasFeladat)
        {
            response.Clear();
            try
            {
                db.HibasFeladatok.Add(hibasFeladat);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = hibasFeladat;
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
        public async Task<MainResponse> UpdateHibasFeladat(int id, HibasFeladat hibasFeladat)
        {
            try
            {
                response.Clear();
                if (id != hibasFeladat.Id)
                {
                    response.ErrorMessage = "Nem egyezik az azonosító.";
                    response.IsSuccess = false;
                    return response;
                }
                db.Entry(hibasFeladat).State = EntityState.Modified;
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = hibasFeladat;
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
        public async Task<MainResponse> DeleteHibasFeladat(int id)
        {
            response.Clear();
            try
            {
                var hibasFeladat = await db.HibasFeladatok.FindAsync(id);
                if (hibasFeladat == null)
                {
                    response.ErrorMessage = "Nincs ilyen hibás feladat.";
                    response.IsSuccess = false;
                    return response;
                }
                db.HibasFeladatok.Remove(hibasFeladat);
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
        public async Task<MainResponse> DeleteAllHibasFeladat()
        {
            response.Clear();
            try
            {
                db.HibasFeladatok.RemoveRange(db.HibasFeladatok);
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
        public async Task<MainResponse> DeleteClassHibasFeladat(string osztaly, string date)
        {
            response.Clear();
            try
            {
                var datum = DateOnly.Parse(date);
                var hibak = await db.HibasFeladatok.Where(h => h.Osztaly.Equals(osztaly) && h.Datum == datum).ToListAsync();
                if (hibak.Count == 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = $"Nincsenek az osztálynak {datum} napon rekordjai a hibák között!";
                    return response;
                }
                db.HibasFeladatok.RemoveRange(hibak);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = $"A {osztaly} hibái {datum} napon törlésre kerültek.";
                settings.LastModify = DateTime.Now;
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
    }
}