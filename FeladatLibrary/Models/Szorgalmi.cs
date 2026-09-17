using System.ComponentModel.DataAnnotations;

namespace FeladatLibrary.Models
{
    public class Szorgalmi
    {
        public int Id { get; set; }
        public Guid TanuloId { get; set; }
        public required string Osztaly { get; set; }
        public DateTime Datum { get; set; } = DateTime.UtcNow;
        public int Feladatok_szama { get; set; }
        public int Pont { get; set; }
        public required string Jegyzet { get; set; }= string.Empty;
    }
}
