using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Data_Api.Services
{
   // public class SzovegService(FeladatSQL db)
    public class SzovegService(FeladatDb db)
    {
        //readonly FeladatSQL _db = db;
        readonly FeladatDb _db = db;
        readonly MainResponse response = new MainResponse();
        public async Task<MainResponse> GetSzovegek()
        {
            response.Clear();
            try
            {
                var szovegek = await _db.Szovegek.ToListAsync();
                if (szovegek == null)
                {
                    response.ErrorMessage = "Nincs szöveg tárolva.";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    response.IsSuccess = true;
                    response.Content = szovegek;
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
        public async Task<MainResponse> CreateSzoveg(Szoveg szoveg)
        {
            response.Clear();
            try
            {
                _db.Szovegek.Add(szoveg);
                await _db.SaveChangesAsync();
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
        public async Task<MainResponse> UpdateSzoveg(int id, Szoveg szoveg)
        {
            response.Clear();
            try
            {
                if (id != szoveg.Id)
                {
                    response.ErrorMessage = "Nem egyezik az azonosító.";
                    response.IsSuccess = false;
                    return response;
                }
                var sz = await _db.Szovegek.FindAsync(szoveg.Id);
                if (sz == null)
                {
                    response.ErrorMessage = "Nincs ilyen szöveg.";
                    response.IsSuccess = false;
                    return response;
                }
                else
                {
                    sz.Text = szoveg.Text;
                    sz.Type = szoveg.Type;
                    await _db.SaveChangesAsync();
                    response.Content = "Sikeres módosítás.";
                    response.IsSuccess = true;
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
        public async Task<MainResponse> DeleteSzoveg(int id)
        {
            response.Clear();
            var szoveg = await _db.Szovegek.FindAsync(id);
            if (szoveg == null)
            {
                response.ErrorMessage = "Nincs ilyen szöveg.";
                response.IsSuccess = false;
                return response;
            }
            else
            {
                _db.Szovegek.Remove(szoveg);
                await _db.SaveChangesAsync();
                response.Content = "Sikeres törlés.";
                response.IsSuccess = true;
                return response;
            }
        }
    }
}
