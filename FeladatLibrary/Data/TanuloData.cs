using FeladatLibrary.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeladatLibrary.Data
{
    public class TanuloData
    {
        public Guid Id { get; set; }
        public int PontSzam { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public double Atlag { get; set; }
        public List<Pont> Pontok { get; set; } = [];

    }
}
