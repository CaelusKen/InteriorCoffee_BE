using InteriorCoffee.Domain.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Floor
{
    public class UpdateFloorDTO
    {
        public string Name { get; set; } = null!;
        public string DesignTemplateId { get; set; } = null!;
        public List<Room> Rooms { get; set; }
    }
}
