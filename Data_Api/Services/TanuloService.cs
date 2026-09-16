using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Data_Api.Services
{
    //public class TanuloService(FeladatSQL feladatSQL)
     public class TanuloService(FeladatDb db,Settings settings)
   {
        //readonly FeladatSQL _db = db;
        readonly FeladatDb _db = db;
        readonly Settings _settings = settings;
        readonly MainResponse response = new();
        public async Task<MainResponse> GetTanulok()
        {
            response.Clear();
            try
            {
                var tanulok = await _db.Tanulok.Include(t => t.Pontok).Include(t => t.Hibak).Include(t => t.Hianyok).Include(t => t.Szorgalmik).ToListAsync();
                if (tanulok == null)
                {
                    response.ErrorMessage = "Nincs tanuló tárolva.";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Content = tanulok;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
         public async Task<MainResponse> GetTanulokToIds()
        {
            response.Clear();
            try
            {
                var tanulok = await _db.Tanulok.Select(t => new TanuloSync {Id= t.Id, LastModify= t.LastModify }).ToListAsync();
                if (tanulok == null)
                {
                    response.ErrorMessage = "Nincs tanuló tárolva.";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Content = tanulok;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
         public async Task<MainResponse> GetTanulokToIds(string osztaly)
        {
            response.Clear();
            try
            {
                var tanulok = await _db.Tanulok.Where(t=>t.Osztaly==osztaly).Select(t=> new TanuloSync { Id = t.Id, LastModify = t.LastModify }).ToListAsync();
                if (tanulok == null)
                {
                    response.ErrorMessage = "Nincs tanuló tárolva.";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Content = tanulok;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
       public async Task<MainResponse> GetTanulo(Guid id)
        {
            response.Clear();
            try
            {
                var tanulo = await _db.Tanulok.FindAsync(id);
                if (tanulo == null)
                {
                    response.ErrorMessage = "Nem található a tanuló.";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Content = tanulo;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
       public async Task<MainResponse> GetTanuloData(Guid id)
        {
            response.Clear();
            try
            {
                var tanulo = await _db.Tanulok.FirstOrDefaultAsync(t => t.Id== id);
                if (tanulo == null)
                {
                    response.ErrorMessage = "Nem található a tanuló.";
                    response.IsSuccess = false;
                    return response;
                }
                var osztaly = tanulo.Osztaly;
                var minimumPont = await _db.Tanulok.Where(t => t.Osztaly == osztaly).MinAsync(t => t.Pont);
                var maximumPont = await _db.Tanulok.Where(t => t.Osztaly == osztaly).MaxAsync(t => t.Pont);
                var averagePont = await _db.Tanulok.Where(t => t.Osztaly == osztaly).AverageAsync(t => t.Pont);
                TanuloData tanuloData = new()
                {
                    Id = tanulo.Id,
                    Atlag = averagePont,
                    PontSzam = tanulo.Pont,
                    Min = minimumPont,
                    Max = maximumPont,
                    Pontok = [.. _db.Pontok.Where(p => p.TanuloId == tanulo.Id).Take(10)]
                };
                response.IsSuccess = true;
                response.Content = tanuloData;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public  async Task<MainResponse> TanuloBeiratkozas(string TanuloId, string CsoportId)
        {
            response.Clear();
            try
            {
                _db.Beiratkozasok.Add(new Beiratkozas
                {
                    TanuloId = Guid.Parse(TanuloId),
                    CsoportId = Guid.Parse(CsoportId)
                });
                await _db.SaveChangesAsync();
                response.Content = "Sikeres beiratkozás.";
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
        public async Task<MainResponse> CreateTanulo(Tanulo tanulo)
        {
            response.Clear();
            try
            {
                _db.Tanulok.Add(tanulo);
                if(tanulo.Csoportok != null && tanulo.Csoportok.Count > 0)
                {
                    foreach (var csoport in tanulo.Csoportok)
                    {
                        var beiratkozas = new Beiratkozas
                        {
                            TanuloId = tanulo.Id,
                            CsoportId = csoport.Id
                        };
                        _db.Beiratkozasok.Add(beiratkozas);
                    }
                }
                await _db.SaveChangesAsync();
                _settings.LastModify = DateTime.Now;
                response.Content = "Sikeres mentés.";
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
        public async Task<MainResponse> UpdateTanulo(Guid id, Tanulo tanulo)
        {
            response.Clear();
            try
            {
                if (id != tanulo.Id)
                {
                    response.ErrorMessage = "Az azonosító nem egyezik.";
                    response.IsSuccess = false;
                    return response;
                }
                _db.Entry(tanulo).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                response.Content = "Sikeres módosítás.";
                response.IsSuccess = true;
                _settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
        public async Task<MainResponse> DeleteTanulo(Guid id)
        {
            response.Clear();
            try
            {
                var tanulo = await _db.Tanulok.FindAsync(id);
                if (tanulo == null)
                {
                    response.ErrorMessage = "Nem található a tanuló.";
                    response.IsSuccess = false;
                    return response;
                }
                _db.Tanulok.Remove(tanulo);
                await _db.SaveChangesAsync();
                response.Content = "Sikeres törlés.";
                response.IsSuccess = true;
                _settings.LastModify = DateTime.Now;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        }

        internal async Task<MainResponse> GetTanulokNewYear()
        {
            response.Clear();
            try
            {
                var tanulok = await _db.Tanulok.ToListAsync();
                foreach (var tanulo in tanulok)
                {
                    tanulo.Pont = 20;
                }
                _db.Tanulok.UpdateRange(tanulok);
                _settings.LastModify = DateTime.Now;
                _db.Pontok.RemoveRange(_db.Pontok);
                _db.HibasFeladatok.RemoveRange(_db.HibasFeladatok);
                _db.FeladatHianyok.RemoveRange(_db.FeladatHianyok);
                _db.Szorgalmik.RemoveRange(_db.Szorgalmik);
                await _db.SaveChangesAsync();

                var resp = await _db.SaveChangesAsync();
                if(resp > 0)
                {
                    _settings.LastModify = DateTime.Now;
                    response.Content = "Sikeres új tanév kezdése.";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Nem volt adatváltozás.";
                    response.IsSuccess = false;
                    return response;
                }
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
