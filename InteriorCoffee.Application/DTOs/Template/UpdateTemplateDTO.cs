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
    public class UpdateTemplateDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public string? Image { get; set; }

        public List<FloorDTO>? Floors { get; set; }
        public List<ProductList>? Products { get; set; }

        public string? StyleId { get; set; }

        public List<string>? Categories { get; set; }
    }
}
