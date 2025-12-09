using System.ComponentModel.DataAnnotations;

namespace FeladatLibrary.Models.Math
{
	public class Sablon
	{
		[Key]
		public int TaskId { get; set; }
		public TaskList? TaskList { get; set; }
		/// <summary>
		/// A minimális feladat mennyisség. 0 esetén hiba határok a döntőek.
		/// </summary>
		[Range(0, 10, ErrorMessage = "Az érték 0 és 10 között lehet.")]
		public int MinCount { get; set; }
		/// <summary>
		/// A maximális feladat mennyisség. 0 esetén hiba határok a döntőek.
		/// </summary>
		[Range(0, 30, ErrorMessage = "Az érték 0 és 30 között lehet.")]
		public int MaxCount { get; set; }
		/// <summary>
		/// A hibás megoldások száma, mely után eggyel kisebb értékű feladatra vált.
		/// </summary>
		[Range(0, 30, ErrorMessage = "Az érték 0 és 30 között lehet.")]
		public int WrongCount { get; set; }
		/// <summary>
		/// A faladatok ennyi % jónak kell lennie a tovább haladáshoz.
		/// </summary>
		[Range(0, 100, ErrorMessage = "Az érték 0 és 100 között lehet.")]
		public int WrongPercent { get; set; }
		public int Time { get; set; }
	}
}
