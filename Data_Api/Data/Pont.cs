namespace Data_Api.Data
{
    public class Pont
    {
        public int Id { get; set; }
        public Guid TanuloId { get; set; }
        public DateTime Datum { get; set; } = DateTime.Now;
        public int PontSzam { get; set; }
        public string Jegyzet { get; set; } = string.Empty;
        public PontTipus PontTipus { get; set; }
    }
    public enum PontTipus
    {
        Lecke,
        Pótlás,
        Szorgalmi,
        Szaktanári,
        Gyakorló,
        _50,
        _100,
        Kvíz,
        Javítás,
        Mértékváltás
    }
}
