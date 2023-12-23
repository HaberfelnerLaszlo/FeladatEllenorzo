using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<FeladatDb>(opt => opt.UseInMemoryDatabase("Feladat"));
        //builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.MapGet("/", () => { return "version:3"; });
        #region Feladathiány
        MainResponse response = new MainResponse();
        app.MapGet("/maihianyok", async (FeladatDb db) =>
        {
            response.Clear();
            var list = await db.FeladatHianyok.Where(h => h.Datum == DateOnly.Parse(DateTime.Today.ToShortDateString())).ToListAsync();
            if (list == null)
            {
                response.ErrorMessage = "Nincs mai napra hányzás";
                response.IsSuccess = false;
                return response;
            }
            response.IsSuccess = true;
            response.Content = list;
            return response;
        });
        app.MapGet("/hianyok/{date}", async (string date, FeladatDb db) =>
        {
            response.Clear();
            try
            {
                var datum = DateOnly.Parse(date);
                var list = await db.FeladatHianyok.Where(h => h.Datum == datum).ToListAsync();
                response.IsSuccess = true;
                response.Content = list;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        });
        app.MapPost("/hiany", async (FeladatHiany hiany, FeladatDb db) =>
        {
            response.Clear();
            try
            {
                db.FeladatHianyok.Add(hiany);
                await db.SaveChangesAsync();
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
        });
        app.MapPut("/hiany/{id}", async (int id, FeladatHiany inputHiany, FeladatDb db) =>
        {
            try
            {
                var hiany = await db.FeladatHianyok.FindAsync(id);

                if (hiany is null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Nincs ilyen rekord a hiányok között!";
                    return response;
                }

                hiany.Name = inputHiany.Name;
                hiany.Osztaly = inputHiany.Osztaly;
                hiany.Datum = inputHiany.Datum;
                hiany.Hianyzik = inputHiany.Hianyzik;

                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = "A hiány módosításra került.";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        });
        app.MapDelete("/hiany/{id}", async (int id, FeladatDb db) =>
        {
            response.Clear();
            try
            {
                var hiany = await db.FeladatHianyok.FindAsync(id);
                if (hiany is null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Nincs ilyen rekord a hiányok között!";
                    return response;
                }
                db.FeladatHianyok.Remove(hiany);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = "A hiány törlésre került.";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        });
        app.MapDelete("/allhiany", async (FeladatDb db) =>
        {
            response.Clear();
            try
            {
                var hianyok = await db.FeladatHianyok.ToListAsync();
                if (hianyok.Count == 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Nincsenek rekordok a hiányok között!";
                    return response;
                }
                db.FeladatHianyok.RemoveRange(hianyok);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = "A hiányok törlésre kerültek.";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        });
        app.MapDelete("/classhiany/{osztaly}/{date}", async (string osztaly, string date, FeladatDb db) =>
        {
            response.Clear();
            try
            {
                var datum = DateOnly.Parse(date);
                var hianyok = await db.FeladatHianyok.Where(h => h.Osztaly.Equals(osztaly) && h.Datum == datum).ToListAsync();
                if (hianyok.Count == 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = $"Nincsenek az osztálynak {datum} napon rekordjai a hiányok között!";
                    return response;
                }
                db.FeladatHianyok.RemoveRange(hianyok);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = $"A {osztaly} hiányai {datum} napon törlésre kerültek.";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        });
        #endregion
        #region Hibás feladat
        app.MapGet("/maihibak", async (FeladatDb db) =>
        {
            response.Clear();
            var list = await db.HibasFeladatok.Where(h => h.Datum == DateOnly.Parse(DateTime.Today.ToShortDateString())).ToListAsync();
            if (list == null)
            {
                response.ErrorMessage = "Nincs a mai napra hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            response.IsSuccess = true;
            response.Content = list;
            return response;

        });
        app.MapGet("/hibak/{date}", async (string date, FeladatDb db) =>
        {
            response.Clear();
            var datum = DateOnly.Parse(date);
            var list = await db.HibasFeladatok.Where(h => h.Datum == datum).ToListAsync();
            if (list == null)
            {
                response.ErrorMessage = "Nincs az adott napra hibás feladat.";
                response.IsSuccess = false;
                return response;
            }
            response.IsSuccess = true;
            response.Content = list;
            return response;

        });
        app.MapPost("/hiba", async (HibasFeladat hiba, FeladatDb db) =>
        {
            response.Clear();
            try
            {
                db.HibasFeladatok.Add(hiba);
                await db.SaveChangesAsync();
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

        });
        app.MapPut("/hiba/{id}", async (int id, HibasFeladat inputHiba, FeladatDb db) =>
        {
            response.Clear();
            try
            {
                var hiba = await db.HibasFeladatok.FindAsync(id);

                if (hiba is null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Nincs ilyen rekord a hibák között!";
                    return response;
                }

                hiba.Name = inputHiba.Name;
                hiba.Osztaly = inputHiba.Osztaly;
                hiba.Datum = inputHiba.Datum;
                hiba.Leiras = inputHiba.Leiras;

                await db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Content = "A hiány módosításra került.";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        });
        app.MapDelete("/hiba/{id}", async (int id, FeladatDb db) =>
        {
            response.Clear();
            try
            {
                var hiba = await db.HibasFeladatok.FindAsync(id);
                if (hiba is null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Nincs ilyen rekord a hibák között!";
                    return response;
                }
                db.HibasFeladatok.Remove(hiba);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = "A hiba törlésre került.";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        });
        app.MapDelete("/allhiba", async (FeladatDb db) =>
        {
            response.Clear();
            try
            {
                var hibak = await db.HibasFeladatok.Where(h => h.Datum.CompareTo(DateOnly.FromDateTime(DateTime.Today.AddMonths(-1))) < 0).ToArrayAsync();
                if (hibak.Count() == 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Nincs ilyen rekord a hibák között!";
                    return response;
                }
                db.HibasFeladatok.RemoveRange(hibak);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = "A hibák törlésre kerültek.";
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        });
        app.MapDelete("/classhiba/{osztaly}/{date}", async (string osztaly, string date, FeladatDb db) =>
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
                return response;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        });
        #endregion
        #region szöveg
        app.MapGet("/szoveg", async (FeladatDb db) =>
        {
            response.Clear();
            try
            {
                var list = await db.Szovegek.ToListAsync();
                response.IsSuccess = true;
                response.Content = list;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
                return response;
            }
        });
        app.MapPost("/szoveg", async (Szoveg szoveg, FeladatDb db) =>
        {
            response.Clear();
            try
            {
                db.Szovegek.Add(szoveg);
                await db.SaveChangesAsync();
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
        });
        app.MapPut("/szoveg/{id}", async (int id, Szoveg szoveg, FeladatDb db) =>
        {
            response.Clear();
            try
            {
                var text = await db.Szovegek.FindAsync(id);

                if (text is null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Nincs ilyen rekord a szövegek között!";
                    return response;
                }
                text.Text = szoveg.Text;
                text.Type = szoveg.Type;

                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = "A szoveg módosításra került.";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        });
        app.MapDelete("/szoveg/{id}", async (int id, FeladatDb db) =>
        {
            response.Clear();
            try
            {
                var szoveg = await db.Szovegek.FindAsync(id);
                if (szoveg is null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "Nincs ilyen rekord a szövegek között!";
                    return response;
                }
                db.Szovegek.Remove(szoveg);
                await db.SaveChangesAsync();
                response.IsSuccess = true;
                response.Content = "A szöveg törlésre került.";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = ex.Message;
                return response;
            }
        });
        #endregion
        app.Run();
    }
}