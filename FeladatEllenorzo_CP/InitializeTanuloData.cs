using CommunityToolkit.Maui;

using FeladatEllenorzo_CP.Data;
using FeladatEllenorzo_CP.Models;
using FeladatEllenorzo_CP.Services;

using Microsoft.Graph.Models;

using System.Text.Json;
using System.Threading.Tasks;

namespace FeladatEllenorzo_CP
{
    internal class InitializeTanuloData(GlobalData globalData, IGraphService graphService)
    {
        readonly GlobalData _data = globalData;
        readonly IGraphService _graphService = graphService;
        private List<Tanulo> tanulok = [];
        private async Task InitTanuloData(List<EducationClass> classes) 
        {
            if (classes is null || classes.Count == 0)
            {
                return;
            }
            foreach (var item in classes)
            {
                var tagok = await _graphService.GetMembers(item.Id);
                if (tagok != null)
                {
                    foreach (var tag in tagok.Value.Where(t => t.PrimaryRole != EducationUserRole.Teacher))
                    {
                        tanulok.Add(new Tanulo
                        {
                            Id = Guid.Parse(tag.Id),
                            Name = tag.DisplayName,
                            Osztaly = item.Id,
                            Pont = 50,
                            LastModify = DateTime.Now
                        });
                    }
                }
            }

        }
        private async Task CreateTanulok()
        {
            TanuloService tanuloService = new();
            foreach (var item in tanulok)
            {
                var response = await tanuloService.CreateTanulo(item);
                if (!response.IsSuccess)
                {
                    Console.WriteLine($"Hiba: {response.ErrorMessage} tanuloId: {item.Id}, név: {item.Name}");
                }
            }
        }
        public async Task SetupTanuloData(List<EducationClass> classes)
        {
            TanuloService tanuloService = new TanuloService();
            string path = Path.Combine(FileSystem.AppDataDirectory, "tanulok.json");
            string json = "";
            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
                if (string.IsNullOrEmpty(json)) //INFO: Üres fájl
                {
                    await InitTanuloData(classes);
                }
                else 
                { 
                    tanulok = JsonSerializer.Deserialize<List<Tanulo>>(json) ?? [];
                    if (tanulok.Count == 0)
                    {
                        tanulok = await tanuloService.GetTanulok();
                        if (tanulok.Count == 0)
                        {
                            await InitTanuloData(classes);
                            await CreateTanulok();
                        }
                    }
                    else
                    {
                        var data = await tanuloService.GetTanulokToIds();
                        if (data == null || data.Count == 0)
                        {
                            await CreateTanulok();
                            data = await tanuloService.GetTanulokToIds();
                            if (data == null || data.Count == 0)
                            {
                                Console.WriteLine("Hiba: Nem tudok felvenni tanulot az adatbázisba!");
                                return;
                            }
                        }
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
            }
            else
            {
                tanulok = await tanuloService.GetTanulok();
                File.Create(path);
                if (tanulok.Count == 0)
                {
                    await InitTanuloData(classes);
                    await CreateTanulok();
                }
            }
            json = JsonSerializer.Serialize(tanulok);
            if (string.IsNullOrEmpty(json))
            {
                Console.WriteLine($"JSON serialization failed. TanuloData: tanulok száma: {tanulok.Count} fő");
                return;
            }
            File.WriteAllText(path, json);
        }
    }
}