namespace Data_Api.Data
{
    public class TanuloData
    {
        public Guid Id { get; set; }
        public int PontSzam { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public double Atlag { get; set; }
        public List<Pont> Pontok { get; set; } = [];
    }
}
