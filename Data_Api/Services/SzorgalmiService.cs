using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

namespace Data_Api.Services
{
	public class SzorgalmiService
	{
		//private readonly FeladatSQL _db;
		private readonly FeladatDb _db;
		MainResponse response = new MainResponse();

		//public SzorgalmiService(FeladatSQL db)
		public SzorgalmiService(FeladatDb db)
		{
			_db = db;
		}
		public async Task<MainResponse> GetSzorgalmik(string osztaly)
		{
			response.Clear();
			var list = await _db.Tanulok.Where(h => h.Osztaly == osztaly)
												   .Include(h => h.Szorgalmik)
												   .ToListAsync();
			if (list == null)
			{
				response.ErrorMessage = "Nincs az osztályban szorgalmi.";
				response.IsSuccess = false;
				return response;
			}
			response.IsSuccess = true;
			response.Content = list;
			return response;
		}
		public async Task<MainResponse> GetSzorgalmikByNev(string nev)
		{
			response.Clear();
			var list = await _db.Tanulok.Where(h => h.Name == nev)
												   .Include(h => h.Szorgalmik)
												   .ToListAsync();
			if (list == null)
			{
				response.ErrorMessage = "Nincs az osztályban szorgalmi.";
				response.IsSuccess = false;
				return response;
			}
			response.IsSuccess = true;
			response.Content = list;
			return response;
		}
		public MainResponse CreateSzorgalmi(Szorgalmi szorgalmi)
		{
            response.Clear();
			try
			{
            _db.Szorgalmik.Add(szorgalmi);
                var db = _db.SaveChanges();
				if (db > 0)
				{
                    _db.Pontok.Add(new Pont
                    {
                        TanuloId = szorgalmi.TanuloId,
                        PontSzam = szorgalmi.Pont,
                        Jegyzet = "Feladatok száma: "+ szorgalmi.Feladatok_szama +" Jegyzet: " + szorgalmi.Jegyzet,
                        PontTipus = PontTipus.Szorgalmi
                    });
                    var tanulo = _db.Tanulok.FirstOrDefault(t => t.Id == szorgalmi.TanuloId);
                    if (tanulo != null) { tanulo.Pont += szorgalmi.Pont; }
                    else { response.ErrorMessage = "Nem található a tanuló."; response.IsSuccess = false; return response; }
                    db=_db.SaveChanges();
                    if (db <= 0) { response.ErrorMessage = "Nem sikerült a pont hozzáadása."; response.IsSuccess = false; return response; }
                    response.IsSuccess = true;
					return response;
                }
                else
                {
                    response.ErrorMessage = "Nem sikerült a szorgalmi feladat hozzáadása.";
                    response.IsSuccess = false;
                    return response;
                }
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Nem sikerült a szorgalmi feladat hozzáadása. "+ ex.Message;
                response.IsSuccess = false;
                return response;
            }
		}
        public MainResponse UpdateSzorgalmi(int id, Szorgalmi szorgalmi)
        {
            response.Clear();
            if (id != szorgalmi.Id)
            {
                response.ErrorMessage = "Nem egyezik az azonosító.";
                response.IsSuccess = false;
                return response;
            }
            try
            {
                var existingSzorgalmi = _db.Szorgalmik.Find(id);
                var ujPont = szorgalmi.Pont - existingSzorgalmi?.Pont?? 0;
                if (ujPont != 0)
                {
                    _db.Pontok.Add(new Pont
                    {
                        TanuloId = szorgalmi.TanuloId,
                        PontSzam = ujPont,
                        Jegyzet = "Szorgalmi módosítás ID: " + szorgalmi.Id,
                        PontTipus = PontTipus.Javítás
                    });
                    var tanulo = _db.Tanulok.FirstOrDefault(t => t.Id == szorgalmi.TanuloId);
                    if (tanulo != null) tanulo.Pont += ujPont;
                }
                _db.Szorgalmik.Update(szorgalmi);
                if (_db.SaveChanges() > 0)
                {
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Nem sikerült a szorgalmi feladat módosítása.";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Nem sikerült a szorgalmi feladat módosítása. " + ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        public MainResponse DeleteSzorgalmi(int id)
        {
            response.Clear();
            var szorgalmi = _db.Szorgalmik.Find(id);
            if (szorgalmi == null)
            {
                response.ErrorMessage = "Nem található a szorgalmi feladat.";
                response.IsSuccess = false;
                return response;
            }
            try
            {
                _db.Szorgalmik.Remove(szorgalmi);
                if (_db.SaveChanges() > 0)
                {
                    _db.Pontok.Add(new Pont(){TanuloId = szorgalmi.TanuloId, Jegyzet ="Szorgalmi törlés ID: " + szorgalmi.Id, PontSzam=szorgalmi.Pont*(-1), PontTipus=PontTipus.Javítás});
                    var tanulo = _db.Tanulok.FirstOrDefault(t => t.Id == szorgalmi.TanuloId);
                    if (tanulo != null) tanulo.Pont -= szorgalmi.Pont;
                    _db.SaveChanges();
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Nem sikerült a szorgalmi feladat törlése.";
                    response.IsSuccess = false;
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Nem sikerült a szorgalmi feladat törlése. " + ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
    }
}
