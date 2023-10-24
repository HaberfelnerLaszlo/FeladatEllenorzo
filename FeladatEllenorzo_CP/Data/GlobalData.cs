
using Microsoft.Graph.Models;

namespace FeladatEllenorzo_CP.Data
{
	public class GlobalData
	{
		public static string AccessToken = null;
		public string ClassId { get; set; }= string.Empty;
		public string AssignmentId { get; set; }= string.Empty;
		public string SubmissionId { get; set; }= string.Empty;
		public string UserId { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public List<MemberData> Members { get; set; } = new();
		public List<DataSzoveg> Szovegek { get; set; } = new();
		public List<DataFeladat> Feladatok = new();
	}
	public record DataFeladat()
	{
		private EducationAssignment feladat = new();

		public EducationAssignment Feladat { get => feladat; set => feladat = value; }
		public int SubmittedCount { get; set; }
		public int WorkingCount { get; set; }
    }
}
