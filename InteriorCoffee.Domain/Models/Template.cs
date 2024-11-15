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
    public class Template
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Status { get; set; } = null!;
        public string Type { get; set; }
        public List<Floor> Floors { get; set; }
        public List<string> Categories { get; set; }

        public string AccountId { get; set; } = null!;
        public string MerchantId { get; set; } = null!;
        public string StyleId { get; set; } = null!;
    }
}
