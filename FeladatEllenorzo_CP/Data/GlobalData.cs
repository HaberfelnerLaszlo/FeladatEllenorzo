
using Microsoft.Graph.Models;
using FeladatEllenorzo_CP.Models;

namespace FeladatEllenorzo_CP.Data
{
	public class GlobalData
	{
		public string AccessToken = null;
        public string ApiUrl { get; set; } = "https://fapi.haberfelner.eu";

        //#if ANDROID
        //    public string ApiUrl { get; set; } = "http://10.0.2.2:7130";
        //#else
        //public string ApiUrl { get; set; } = "http://localhost:7130";

        public string ClassId { get; set; }= string.Empty;
		public string AssignmentId { get; set; }= string.Empty;
		public string SubmissionId { get; set; }= string.Empty;
		public Guid StudentId { get; set; }= Guid.Empty;
        public string UserId { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
        public string Osztalynev { get; set; }=string.Empty;
		public int IsHiany { get; set; } = 0;
        public List<MemberData> Members { get; set; } = [];
		public List<DataSzoveg> Szovegek { get; set; } = [];
		public List<DataFeladat> Feladatok = [];
	}
	public record DataFeladat()
	{
		private EducationAssignment feladat = new();

		public EducationAssignment Feladat { get => feladat; set => feladat = value; }
		public int SubmittedCount { get; set; }
		public int WorkingCount { get; set; }
    }
}
