namespace FeladatEllenorzo_CP.Models
{
    public class HibasFeladat
    {
        public int Id { get; set; }
        public Guid TanuloId { get; set; }
        public DateOnly Datum { get; set; }
        public string Leiras { get; set; }
        public string Osztaly { get; set; }
    }
}
