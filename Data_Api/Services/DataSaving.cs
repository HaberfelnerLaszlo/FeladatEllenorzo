using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

using System.Text.Json;

namespace Data_Api.Services
{
    public class DataSaving(FeladatSQL db, Settings settings)
    {
        public async Task SaveToJsonAsync(string filePath)
        {
            SaveData data = new();
            var tanulok = await db.Tanulok.AsNoTracking().Include(t => t.Csoportok).ToListAsync();
            data.CsoportTagsagok = [.. tanulok.SelectMany(t => t.Csoportok.Select(c => new CsoportTagsag { TanuloId = t.Id, CsoportId = c.Id }))];
            tanulok.ForEach(t => { t.Csoportok = []; t.Pontok = []; t.Hibak = []; t.Hianyok = []; t.Szorgalmik = []; });
            data.Tanulok = tanulok;
            data.Csoportok = await db.Csoportok.AsNoTracking().ToListAsync();
            data.Csoportok.ForEach(c => c.Tanulok = []);
            data.Beiratkozasok = await db.Beiratkozasok.AsNoTracking().ToListAsync();
            data.Pontok = await db.Pontok.AsNoTracking().ToListAsync();
            data.PontLogok = await db.PontLogok.AsNoTracking().ToListAsync();
            data.HibasFeladatok = await db.HibasFeladatok.AsNoTracking().ToListAsync();
            data.FeladatHianyok = await db.FeladatHianyok.AsNoTracking().ToListAsync();
            data.Szorgalmik = await db.Szorgalmik.AsNoTracking().ToListAsync();
            data.Szovegek = await db.Szovegek.AsNoTracking().ToListAsync();

            var options = new JsonSerializerOptions { WriteIndented = true };
            // Előbb ideiglenes fájlba írunk, hogy egy félbeszakadt mentés ne rontsa el a meglévőt.
            var tmpPath = filePath + ".tmp";
            await using (FileStream createStream = File.Create(tmpPath))
            {
                await JsonSerializer.SerializeAsync(createStream, data, options);
            }
            File.Move(tmpPath, filePath, overwrite: true);
        }

        public async Task LoadFromJsonAsync(string filePath)
        {
            if (!File.Exists(filePath)) return;
            SaveData? data;
            await using (FileStream openStream = File.OpenRead(filePath))
            {
                data = await JsonSerializer.DeserializeAsync<SaveData>(openStream);
            }
            if (data == null) return;

            // Minden egy tranzakcióban: ha a visszatöltés elszáll, a régi adatok megmaradnak.
            await using var transaction = await db.Database.BeginTransactionAsync();
            await TorlesAsync(teljes: true);
            await db.SaveChangesAsync();
            db.ChangeTracker.Clear();

            data.Tanulok.ForEach(t => { t.Csoportok = []; t.Pontok = []; t.Hibak = []; t.Hianyok = []; t.Szorgalmik = []; });
            data.Csoportok.ForEach(c => c.Tanulok = []);
            db.Tanulok.AddRange(data.Tanulok);
            db.Csoportok.AddRange(data.Csoportok);
            foreach (var tagsag in data.CsoportTagsagok)
            {
                var tanulo = data.Tanulok.FirstOrDefault(t => t.Id == tagsag.TanuloId);
                var csoport = data.Csoportok.FirstOrDefault(c => c.Id == tagsag.CsoportId);
                if (tanulo is not null && csoport is not null) tanulo.Csoportok.Add(csoport);
            }
            db.Beiratkozasok.AddRange(data.Beiratkozasok);
            db.Szovegek.AddRange(data.Szovegek);
            db.FeladatHianyok.AddRange(data.FeladatHianyok);
            db.HibasFeladatok.AddRange(data.HibasFeladatok);
            db.Szorgalmik.AddRange(data.Szorgalmik);
            db.Pontok.AddRange(data.Pontok);
            db.PontLogok.AddRange(data.PontLogok);
            await db.SaveChangesAsync();
            await transaction.CommitAsync();

            settings.LastModify = data.LastSaved;
            settings.LastSaved = data.LastSaved;
        }

        public async Task ClearDatabaseAsync()
        {
            await TorlesAsync(teljes: false);
            await db.SaveChangesAsync();
            settings.LastModify = DateTime.Now;
        }

        /// <param name="teljes">Igaz esetén a szövegsablonok, csoportok és naplók is törlődnek (visszatöltéshez).</param>
        private async Task TorlesAsync(bool teljes)
        {
            db.Pontok.RemoveRange(await db.Pontok.ToListAsync());
            db.HibasFeladatok.RemoveRange(await db.HibasFeladatok.ToListAsync());
            db.FeladatHianyok.RemoveRange(await db.FeladatHianyok.ToListAsync());
            db.Szorgalmik.RemoveRange(await db.Szorgalmik.ToListAsync());
            db.Tanulok.RemoveRange(await db.Tanulok.Include(t => t.Csoportok).ToListAsync());
            if (teljes)
            {
                db.Szovegek.RemoveRange(await db.Szovegek.ToListAsync());
                db.Csoportok.RemoveRange(await db.Csoportok.ToListAsync());
                db.Beiratkozasok.RemoveRange(await db.Beiratkozasok.ToListAsync());
                db.PontLogok.RemoveRange(await db.PontLogok.ToListAsync());
            }
        }

        public async Task ClearDatabaseNewYearAsync()
        {
            db.Pontok.RemoveRange(db.Pontok);
            db.HibasFeladatok.RemoveRange(db.HibasFeladatok);
            db.FeladatHianyok.RemoveRange(db.FeladatHianyok);
            db.Szorgalmik.RemoveRange(db.Szorgalmik);
            await db.SaveChangesAsync();
            settings.LastModify = DateTime.Now;
        }
    }
    public class SaveData
    {
        public DateTime LastSaved { get; set; } = DateTime.Now;
        public List<Tanulo> Tanulok { get; set; } = [];
        public List<Csoport> Csoportok { get; set; } = [];
        public List<CsoportTagsag> CsoportTagsagok { get; set; } = [];
        public List<Beiratkozas> Beiratkozasok { get; set; } = [];
        public List<Pont> Pontok { get; set; } = [];
        public List<PontLog> PontLogok { get; set; } = [];
        public List<HibasFeladat> HibasFeladatok { get; set; } = [];
        public List<FeladatHiany> FeladatHianyok { get; set; } = [];
        public List<Szorgalmi> Szorgalmik { get; set; } = [];
        public List<Szoveg> Szovegek { get; set; } = [];
    }
    public class CsoportTagsag
    {
        public Guid TanuloId { get; set; }
        public Guid CsoportId { get; set; }
    }
}
