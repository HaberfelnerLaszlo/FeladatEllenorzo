namespace FeladatLibrary.ViewModels
{
	public class TaskRecive
	{
		public Guid StudentId { get; set; }
		public int OperationId { get; set; }
		/// <summary>
		/// A tanuló válasza
		/// </summary>
		public string Result { get; set; } = string.Empty;
		/// <summary>
		/// Mennyi idő alatt oldotta meg
		/// </summary>
		public int Time { get; set; }
		/// <summary>
		/// Kapott hozzá választási lehetőséget
		/// </summary>
		public bool IsTipp { get; set; }
		/// <summary>
		/// Továbblépést jelző
		/// 0 - marad
		/// 1 - tovább
		/// 2 - vissza
		/// </summary>
		public byte Step { get; set; }=0;
		public int TaskId { get; set; }
	}
}
