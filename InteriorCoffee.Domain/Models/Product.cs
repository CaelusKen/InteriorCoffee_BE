using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteriorCoffee.Domain.Models.Documents;

namespace InteriorCoffee.Domain.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id {  get; set; } = null!;
        public List<string> CategoryIds { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public ProductImages Images { get; set; } = null!;
        public double SellingPrice { get; set; }
        public int Discount { get; set; }
        public double TruePrice { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; } = null!;
        public string Dimensions { get; set; } = null!;
        public List<string> Materials { get; set; } = null!;
        public string ModelTextureUrl { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string CampaignId { get; set; }
        public string MerchantId { get; set; } = null!;
    }
}
