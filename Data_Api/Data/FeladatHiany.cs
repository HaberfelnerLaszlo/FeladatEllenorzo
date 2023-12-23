using System.ComponentModel.DataAnnotations;

namespace Data_Api.Data
{
	public class FeladatHiany
	{
		[Required]
		[Key]
        public int Id { get; set; }
		public required  string Name { get; set; }
		public required string Osztaly { get; set; }
		public DateOnly Datum { get; set; }
        public bool Hianyzik { get; set; }
		public FeladatHiany Create(string name, string osztaly, string datum) 
		{
			return new FeladatHiany() { Name = name, Osztaly = osztaly, Datum=DateOnly.Parse(datum),Hianyzik=false };
		}
    }
}
