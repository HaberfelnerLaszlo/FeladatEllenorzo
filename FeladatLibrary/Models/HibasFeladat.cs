namespace FeladatLibrary.Models
{
    public class HibasFeladat
    {
        public int Id { get; set; }
        public Guid TanuloId { get; set; }
        public DateOnly Datum { get; set; }
        public required string Leiras { get; set; }
        public required string Osztaly { get; set; }
    }
}
