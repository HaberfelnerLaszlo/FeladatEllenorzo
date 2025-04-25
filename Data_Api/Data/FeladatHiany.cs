using System.ComponentModel.DataAnnotations;

namespace Data_Api.Data
{
	public class FeladatHiany
	{
		[Required]
		[Key]
        public int Id { get; set; }
		public required  Guid TanuloId { get; set; }
		public required string Osztaly { get; set; }
		public Guid FeladatId { get; set; }
        public DateOnly Datum { get; set; }
        public bool Hianyzik { get; set; }
		public FeladatHiany Create(Guid id, string osztaly, string datum) 
		{
			return new FeladatHiany() { TanuloId = id, Osztaly = osztaly, Datum=DateOnly.Parse(datum),Hianyzik=false };
		}
    }
}
