using Data_Api.Data;

namespace Data_Api.Services
{
    public class CsoportService(FeladatSQL db)
    {
        readonly MainResponse response = new();

        public async Task<MainResponse> GetCsoportok()
        {
            try
            {
                var csoportok = db.Csoportok.ToList();
                response.IsSuccess = true;
                response.Content = csoportok;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Hiba történt a csoportok lekérése során. hiba: " + ex.Message;
                return response;
            }
        }
        public async Task<MainResponse> GetCsoport(Guid id)
        {
            try
            {
                var csoport = db.Csoportok.FirstOrDefault(c => c.Id == id);
                if (csoport == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "A csoport nem található.";
                    return response;
                }
                response.IsSuccess = true;
                response.Content = csoport;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Hiba történt a csoport lekérése során. hiba: " + ex.Message;
                return response;
            }
        }
        public async Task<MainResponse> CreateCsoport(Csoport csoport)
        {
            try
            {
                db.Csoportok.Add(csoport);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = csoport;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Hiba történt a csoport létrehozása során. hiba: " + ex.Message;
                return response;
            }
        }
        public async Task<MainResponse> UpdateCsoport(Guid id, Csoport csoport)
        {
            try
            {
                var existingCsoport = db.Csoportok.FirstOrDefault(c => c.Id == id);
                if (existingCsoport == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "A csoport nem található.";
                    return response;
                }
                existingCsoport.Name = csoport.Name;
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = existingCsoport;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Hiba történt a csoport frissítése során. hiba: " + ex.Message;
                return response;
            }
        }
        public async Task<MainResponse> DeleteCsoport(Guid id)
        {
            try
            {
                var existingCsoport = db.Csoportok.FirstOrDefault(c => c.Id == id);
                if (existingCsoport == null)
                {
                    response.IsSuccess = true;
                    response.Content = true;
                    return response;
                }
                db.Csoportok.Remove(existingCsoport);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = true;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Hiba történt a csoport törlése során. hiba: " + ex.Message;
                return response;
            }
        }
    }
}
