using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Models
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id {  get; set; } = null!;
        public string Comment { get; set; }
        public float Rating { get; set; }

        public string ProductId { get; set; } = null!;
        public string AccountId { get; set; } = null!;
    }
}
