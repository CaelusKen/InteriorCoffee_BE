using System;
using System.Collections.Generic;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Application.DTOs.Product
{
    public class CreateProductDTO
    {
        public List<string> CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductImages Images { get; set; }
        public double SellingPrice { get; set; }
        public int Discount { get; set; }
        public double TruePrice { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Dimensions { get; set; }
        public List<string> Materials { get; set; }
        public string ModelTextureUrl { get; set; }
        public string CampaignId { get; set; }
        public string MerchantId { get; set; }
    }
}
