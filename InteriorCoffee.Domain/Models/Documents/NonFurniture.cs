using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Models.Documents
{
    public class NonFurniture
    {
        //public string _id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Model { get; set; } = null!;
        public List<double> Position { get; set; } = null!;
        public List<double> Rotation { get; set; } = null!;
        public List<double> Scale { get; set; } = null!;
        public bool Visible { get; set; }
    }
}
