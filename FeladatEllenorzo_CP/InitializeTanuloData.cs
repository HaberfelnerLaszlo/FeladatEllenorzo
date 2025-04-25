using CommunityToolkit.Maui;

using FeladatEllenorzo_CP.Data;
using FeladatEllenorzo_CP.Models;
using FeladatEllenorzo_CP.Services;

using System.Text.Json;

namespace FeladatEllenorzo_CP
{
    internal class InitializeTanuloData(GlobalData globalData)
    {
        GlobalData _data = globalData;
        public async Task SetupTanuloData()
        {
            TanuloService tanuloService = new TanuloService();
            string path = Path.Combine(FileSystem.AppDataDirectory, "tanulok.json");
            List<Tanulo> tanulok = [];
            string json = "";
            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
                tanulok = JsonSerializer.Deserialize<List<Tanulo>>(json) ?? [];
                if (tanulok.Count == 0)
                {
                    tanulok = await tanuloService.GetTanulok();
                    if (tanulok.Count == 0)
                    {
                        return;
                    }
                }
                else
                {
                    var data = await tanuloService.GetTanulokToIds();
                    foreach (var item in data)
                    {
                        if (tanulok.Exists(t => t.Id == item.Id)) 
                        { 
                            var tanulo = tanulok.Find(t => t.Id == item.Id);
                            if (tanulo.LastModify != item.LastModify)
                            {
                                tanulok.Remove(tanulo);
                                tanulo = await tanuloService.GetTanulo(item.Id);
                                tanulok.Add(tanulo);
                            }
                        }
                        else
                        {
                            var tanulo = await tanuloService.GetTanulo(item.Id);
                            if (tanulo != null)
                            {
                                tanulok.Add(tanulo);
                            }
                        }
                    }
                }
            }
            else
            {
                tanulok = await tanuloService.GetTanulok();
                File.Create(path);
                if (tanulok.Count == 0)
                {
                    return;
                }
            }
            json = JsonSerializer.Serialize(tanulok);
            File.WriteAllText(path, json);
            tanulok.ForEach(tanulo => _data.Members.Add(new MemberData 
                {Id=tanulo.Id,
                Name=tanulo.Name,
                LastModify=tanulo.LastModify, 
                Osztaly=tanulo.Osztaly, 
                Pont=tanulo.Pont })
                );
        }
    }
}