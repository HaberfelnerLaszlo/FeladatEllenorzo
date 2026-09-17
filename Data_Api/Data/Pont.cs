namespace Data_Api.Data
{
    public class Pont
    {
        public int Id { get; set; }
        public Guid TanuloId { get; set; }
        public Guid CsoportId { get; set; }
        public DateTime Datum { get; set; } = DateTime.Now;
        public int PontSzam { get; set; }
        public string Jegyzet { get; set; } = string.Empty;
        public PontTipus PontTipus { get; set; }
        public bool IsDeleted { get; set; } = false;
        public override string ToString()
        {
            return $"TanuloId: {TanuloId}, CsoportId: {CsoportId}, Típus: {PontTipus}, Pontszám: {PontSzam}, Dátum: {Datum.ToShortDateString()}, Jegyzet: {Jegyzet}, Törölt: {(IsDeleted? "igen" : "nem")}";
        }
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
