using System;
using System.Collections.Generic;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Application.DTOs.Design
{
    public class CreateDesignDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public List<Floor> Floors { get; set; }
        public string AccountId { get; set; }
        public string TemplateId { get; set; }
        public string StyleId { get; set; }
    }
}
