namespace Data_Api.Data
{
	public class HibasFeladat
	{
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
		public DateOnly Datum { get; set; }=DateOnly.Parse(DateTime.Today.ToString());
        public string Leiras { get; set; } = string.Empty;
        public string Osztaly { get; set; }= string.Empty;
    }
}
