using System;
using System.Collections.Generic;
using InteriorCoffee.Application.DTOs.Floor;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Application.DTOs.Design
{
    public class CreateDesignDTO
    {
        public string Name { get; set; } = null!;    
        public string? Description { get; set; }
        //public DateTime CreatedDate { get; set; } //Already set in mapper
        //public string Status { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public List<FloorDTO> Floors { get; set; }
        public List<ProductList>? Products { get; set; }
        public string AccountId { get; set; } = null!;
        public string? TemplateId { get; set; }
        public string StyleId { get; set; } = null!;

        public List<string> Categories { get; set; }
    }
}
