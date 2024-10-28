using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Models
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id {  get; set; } = null!;
        public string PaymentMethod { get; set; } = null!;
        public DateTime TransactionDate { get; set; }
        public double TotalAmount { get; set; }
        public string Currency {  get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string AccountId { get; set; } = null!;
        public string OrderId { get; set; } = null!;
    }
}
