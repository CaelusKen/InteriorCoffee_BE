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
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id {  get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public double VAT { get; set; }
        public double FeeAmount { get; set; }
        public double TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderProducts> OrderProducts { get; set; }

        public string VoucherId { get; set; }
        public string AccountId { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
