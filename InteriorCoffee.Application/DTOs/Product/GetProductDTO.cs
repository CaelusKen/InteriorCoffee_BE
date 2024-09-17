using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Product
{
    public class GetProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public GetProductDTO(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
