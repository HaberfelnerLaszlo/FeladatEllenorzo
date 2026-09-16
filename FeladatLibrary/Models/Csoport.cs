using System;
using System.Collections.Generic;
using System.Text;

namespace FeladatLibrary.Models
{
    public class Csoport
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Tanulo> Tanulok { get; set; } = [];
    }
}
