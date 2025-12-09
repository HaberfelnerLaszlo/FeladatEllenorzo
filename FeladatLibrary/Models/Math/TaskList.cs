using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeladatLibrary.Models.Math
{
	public class TaskList
	{
		[Key]
		public int Id { get; set; }
		/// <summary>
		/// Feladatkör leírása
		/// </summary>
		[MaxLength(255,ErrorMessage ="Nem lehet több 255 karakternél")]
		public string Description { get; set; } = string.Empty;
		/// <summary>
		/// A feladat témaköre
		/// </summary>
		[MaxLength(30, ErrorMessage ="Nem lehet több 30 karakternél")]
		public string Topic { get; set; } = string.Empty;
		//public IList<Operation>? Operations { get; set; }
		[ForeignKey("TaskID")]
		public Sablon? Sablon { get; set; }
	}
}
