using Data_Api.Data;

using Microsoft.EntityFrameworkCore;

using System.Text.Json;

namespace Data_Api.Services
{
    public class DataSaving(FeladatDb db, Settings settings)
    {
        public async Task SaveToJsonAsync(string filePath)
        {
            SaveData data = new SaveData();
            data.Tanulok = await db.Tanulok.ToListAsync();
            data.Pontok = await db.Pontok.ToListAsync();
            data.HibasFeladatok = await db.HibasFeladatok.ToListAsync();
            data.FeladatHianyok = await db.FeladatHianyok.ToListAsync();
            data.Szorgalmik = await db.Szorgalmik.ToListAsync();
            data.Szovegek = await db.Szovegek.ToListAsync();

            var options = new JsonSerializerOptions { WriteIndented = true };
            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, data, options);
        }

        public async Task LoadFromJsonAsync(string filePath)
        {
            if (!File.Exists(filePath)) return;
            using FileStream openStream = File.OpenRead(filePath);
            var data = await JsonSerializer.DeserializeAsync<SaveData>(openStream);
            if (data != null)
            {
                settings.LastModify = data.LastSaved;
                settings.LastSaved = data.LastSaved;
                db.Tanulok.AddRange(data.Tanulok);
                db.Pontok.AddRange(data.Pontok);
                db.HibasFeladatok.AddRange(data.HibasFeladatok);
                db.FeladatHianyok.AddRange(data.FeladatHianyok);
                db.Szorgalmik.AddRange(data.Szorgalmik);
                db.Szovegek.AddRange(data.Szovegek);
                await db.SaveChangesAsync();
            }
        }
        public async Task ClearDatabaseAsync()
        {
            settings.LastModify = DateTime.Now;
            db.Tanulok.RemoveRange(db.Tanulok);
            db.Pontok.RemoveRange(db.Pontok);
            db.HibasFeladatok.RemoveRange(db.HibasFeladatok);
            db.FeladatHianyok.RemoveRange(db.FeladatHianyok);
            db.Szorgalmik.RemoveRange(db.Szorgalmik);
            await db.SaveChangesAsync();
        }
    }
    public class SaveData
    {
        public DateTime LastSaved { get; set; } = DateTime.Now;
        public List<Tanulo> Tanulok { get; set; } = [];
        public List<Pont> Pontok { get; set; } = [];
        public List<HibasFeladat> HibasFeladatok { get; set; } = [];
        public List<FeladatHiany> FeladatHianyok { get; set; } = [];
        public List<Szorgalmi> Szorgalmik { get; set; } = [];
        public List<Szoveg > Szovegek { get; set; } = [];
    }
}
