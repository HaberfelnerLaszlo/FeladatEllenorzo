namespace Data_Api.Data
{
    public class PontLog
    {
        public int Id { get; set; }
        public int PontId { get; set; }
        public DateTime Datum { get; set; } = DateTime.Now;
        public string Valtozas { get; set; } = string.Empty;
    }
}
