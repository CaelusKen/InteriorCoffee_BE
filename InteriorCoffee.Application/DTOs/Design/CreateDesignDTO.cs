using System;
using System.Collections.Generic;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Application.DTOs.Design
{
    public class CreateDesignDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public DateTime CreatedDate { get; set; } //Already set in mapper
        public string Status { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public List<Floor> Floors { get; set; }
        public string AccountId { get; set; }
        public string TemplateId { get; set; }
        public string StyleId { get; set; }

        public List<string> Categories { get; set; }
    }
}
