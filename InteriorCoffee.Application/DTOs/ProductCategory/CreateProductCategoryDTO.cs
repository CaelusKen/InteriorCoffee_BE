using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.ProductCategory
{
    public class CreateProductCategoryDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}

