
namespace FeladatEllenorzo_CP.Data
{
	public class GlobalData
	{
		public string ClassId { get; set; }= string.Empty;
		public string AssignmentId { get; set; }= string.Empty;
		public string SubmissionId { get; set; }= string.Empty;
		public string UserId { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public List<MemberData> Members { get; set; } = new();
		public List<string> pSzoveg { get; set; }= new();
		public List<string> nSzoveg { get; set; }=new();
	public Dictionary<string, string> SubmittedMembers { get; set; } = new();
		public Dictionary<string,string> ReturnedMembers { get; set; }= new();
	}
}
