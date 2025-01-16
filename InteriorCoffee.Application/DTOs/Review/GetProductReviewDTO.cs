using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Review
{
    public class GetProductReviewDTO
    {
        public string _id { get; set; } = null!;
        public string Comment { get; set; }
        public float Rating { get; set; }

        public string ProductId { get; set; } = null!;
        public string AccountId { get; set; } = null!;
        public string AccountName { get; set; } = null!;
    }
}
