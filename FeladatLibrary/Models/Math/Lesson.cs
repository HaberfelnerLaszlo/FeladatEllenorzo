using System.ComponentModel.DataAnnotations;

namespace FeladatLibrary.Models.Math
{
	public class Lesson
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public Guid StudentId { get; set; }
		public int TaskId { get; set; }
		public TaskList? TaskList { get; set; }
		/// <summary>
		/// A minimális feladat mennyisség. 0 esetén hiba határok a döntőek.
		/// </summary>
		[Range(0, 10,ErrorMessage ="Az érték 0 és 10 között lehet.")]
		public byte MinCount { get; set; }
		/// <summary>
		/// A maximális feladat mennyisség. 0 esetén hiba határok a döntőek.
		/// </summary>
		[Range(0, 30,ErrorMessage ="Az érték 0 és 30 között lehet.")]
		public byte MaxCount { get; set; }
		/// <summary>
		/// A hibás megoldások száma, mely után eggyel kisebb értékű feladatra vált.
		/// </summary>
		[Range(0, 30, ErrorMessage = "Az érték 0 és 30 között lehet.")]
		public byte WrongCount { get; set; }
		/// <summary>
		/// A faladatok ennyi % jónak kell lennie a tovább haladáshoz.
		/// </summary>
		[Range(0, 100,ErrorMessage ="Az érték 0 és 100 között lehet.")]
		public byte WrongPercent { get; set; }
		public int Time { get; set; }
		public bool IsCompleted { get; set; }
		public bool IsTipp { get; set; }
		public bool IsProcessing { get; set; }
	}
}
