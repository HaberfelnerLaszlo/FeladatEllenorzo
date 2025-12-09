namespace FeladatLibrary.ViewModels
{
	public class OperationView
	{
		/// <summary>
		/// A számítás azonosítója
		/// </summary>
		public int OperationId { get; set; } = 1;
		/// <summary>
		/// műveletsor (SVG formátumban mentve)
		/// </summary>
		public string Sequence { get; set; } = string.Empty;
		/// <summary>
		/// A tanuló válasza
		/// </summary>
		public string Result { get; set; } = string.Empty;
		public List<string>? Tipps { get; set; }
	}
}