using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Models.Documents
{
    public class Room
    {
        public string _id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double Width { get; set; }
        public double Height { get; set; }
        public double Length { get; set; }
        public List<Furniture> Furnitures { get; set; }
    }
}
