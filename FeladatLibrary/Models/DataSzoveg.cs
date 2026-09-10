using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatLibrary.Models
{
	public class DataSzoveg
	{
        public int Id { get; set; }
        public required string Type { get; set; }
		public required string Text { get; set; }
	}
}
