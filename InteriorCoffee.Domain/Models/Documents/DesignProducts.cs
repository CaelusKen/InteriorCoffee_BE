using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Models.Documents
{
    public class DesignProducts
    {
        public string _id { get; set; } = null!;

        public Position Position { get; set; } = null!;
        public Scale Scale { get; set; } = null!;
        public Rotation Rotation { get; set; } = null!;
    }
}
