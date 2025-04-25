using System.ComponentModel.DataAnnotations;

namespace Data_Api.Data
{
    public class Szorgalmi
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public required Guid TanuloId { get; set; }
        public required string Osztaly { get; set; }
        public DateOnly Datum { get; set; } = DateOnly.Parse(DateTime.Today.ToString());
        public int Feladatok_szama { get; set; }
        public int Pont { get; set; }
        public string Jegyzet { get; set; }= string.Empty;
        public Tipus FeladatTipus { get; set; } = Tipus.Egyéb;
    }
    public enum Tipus
    {
        Órai,
        Házi,
        Program,
        Egyéb
    }
}
