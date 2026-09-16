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
        public int Pont { get; set; } =20; //INFO: Kezdő pontszám, ami a tanulóhoz tartozik
        public DateTime LastModify { get; set; } = DateTime.Now;
        public virtual List<Csoport> Csoportok { get; set; } = [];
        public virtual List<Szorgalmi> Szorgalmik { get; set; } = [];
        public virtual List<FeladatHiany> Hianyok { get; set; } = [];
        public virtual List<HibasFeladat> Hibak { get; set; } = [];
        public virtual List<Pont> Pontok { get; set; } = [];
    }
}
