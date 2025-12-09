namespace FeladatLibrary.ViewModels
{
	public class TaskView
	{
		/// <summary>
		/// A feladat azonosító kód
		/// </summary>
		public int TaskId { get; set; }
		/// <summary>
		/// A feladat leírása
		/// </summary>
		public string Title { get; set; }=string.Empty;
		/// <summary>
		/// A számítások listája elvileg MaxCount db
		/// </summary>
		public List<OperationView> Operations { get; set; } = null!;
		public byte MinCount { get; set; }
		public byte MaxCount { get; set; }
		public byte WrongCount { get; set; }
		public byte WrongPercent { get; set; }
		public int Time { get; set; }
		/// <summary>
		/// Tört értéket vár a program
		/// </summary>
		public bool IsFraction { get; set; }
	}
}
