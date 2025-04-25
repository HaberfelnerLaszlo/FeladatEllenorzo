using FeladatEllenorzo_CP.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatEllenorzo_CP.Data
{
    public class MemberData :Tanulo
    {
        public string SubmissionId { get; set; } = string.Empty;
        public string Status { get; set;} = "Nincs";
        public bool Selected { get; set; }=false;
        public int ResourceCount { get; set; } = 0;
    }
}
