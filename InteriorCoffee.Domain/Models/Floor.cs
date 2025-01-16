using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Domain.Models
{
    public class Floor
    {
        public string _id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string DesignTemplateId { get; set; } = null!;
        public List<Room> Rooms { get; set; }
    }
}
