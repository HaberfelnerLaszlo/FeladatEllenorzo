using System.ComponentModel.DataAnnotations;

namespace FeladatEllenorzo_CP.Models
{
    public class Tanulo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Osztaly { get; set; }
        public int Pont { get; set; }
        public DateTime LastModify { get; set; } = DateTime.Now;
        public List<Szorgalmi> Szorgalmik { get; set; } = [];
        public List<FeladatHiany> Hianyok { get; set; } = [];
        public List<HibasFeladat> Hibak { get; set; } = [];
        public List<Pont> Pontok { get; set; } = [];

    }
}
