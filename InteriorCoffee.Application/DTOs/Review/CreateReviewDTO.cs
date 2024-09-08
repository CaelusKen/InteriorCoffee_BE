using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Review
{
    public class CreateReviewDTO
    {
        public string Comment { get; set; }
        public float Rating { get; set; }

        public string ProductId { get; set; }
        public string AccountId { get; set; }
    }
}
