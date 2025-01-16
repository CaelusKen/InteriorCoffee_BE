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
    public class Merchant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id {  get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } = null!;
        public string MerchantCode { get; set; } = null!;
        public string PolicyDocument { get; set; } = null!;
        public string Website {  get; set; }
        
        public List<OrderIncome> OrderIncomes { get; set; }
    }
}
