using System.ComponentModel.DataAnnotations;

namespace FeladatEllenorzo_CP.Models
{
    public class FeladatHiany
    {
        [Key]
        public int Id { get; set; }
        public Guid TanuloId { get; set; }
        public string Osztaly { get; set; }
        public Guid FeladatId { get; set; }

        public DateOnly Datum { get; set; }
        public bool Hianyzik { get; set; }
    }
}
