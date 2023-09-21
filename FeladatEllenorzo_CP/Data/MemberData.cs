using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatEllenorzo_CP.Data
{
    public class MemberData
    {
        public string MemberId { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string SubmissionId { get; set; } = string.Empty;
        public string Status { get; set;} = "Nincs";
        public bool Selected { get; set; }=false;
    }
}
