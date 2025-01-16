﻿using System;
using System.Collections.Generic;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Application.DTOs.Product
{
    public class CreateProductDTO
    {
        public List<string> CategoryIds { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public ProductImages Images { get; set; } = null!;
        //public double SellingPrice { get; set; } //Calculate in code
        public int Discount { get; set; }
        public double TruePrice { get; set; }
        public int Quantity { get; set; }
        //public string Status { get; set; }
        public string Dimensions { get; set; }
        public List<string> Materials { get; set; }
        public string ModelTextureUrl { get; set; }
        public string CampaignId { get; set; }
        public string MerchantId { get; set; } = null!;
    }
}
