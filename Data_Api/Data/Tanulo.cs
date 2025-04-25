using System.ComponentModel.DataAnnotations;

namespace Data_Api.Data
{
    public class Tanulo
    {
        [Required]
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Osztaly { get; set; }
        public int Pont { get; set; } =40; //INFO: Kezdő pontszám, ami a tanulóhoz tartozik
        public DateTime LastModify { get; set; } = DateTime.Now;
        public List<Szorgalmi> Szorgalmik { get; set; } = [];
        public List<FeladatHiany> Hianyok { get; set; } = [];
        public List<HibasFeladat> Hibak { get; set; } = [];
        public List<Pont> Pontok { get; set; } = [];
    }
}
