using System.ComponentModel.DataAnnotations;

namespace Data_Api.Data
{
	public class Szoveg
	{
		[Key]
        public int Id { get; set; }
		public string Text { get; set; } = string.Empty;
		public string Type { get; set; } = string.Empty;
    }
}
