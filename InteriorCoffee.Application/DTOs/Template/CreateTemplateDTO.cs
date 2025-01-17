﻿using InteriorCoffee.Application.DTOs.Floor;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Domain.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Template
{
    public class CreateTemplateDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }

        public List<FloorDTO>? Floors { get; set; }
        public List<ProductList>? Products { get; set; }

        public string AccountId { get; set; } = null!;
        public string? MerchantId { get; set; }
        public string StyleId { get; set; } = null!;

        public List<string> Categories { get; set; }
    }
}
