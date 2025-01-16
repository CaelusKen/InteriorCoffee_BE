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

        public List<string> ProductIds { get; set; } = null!;
        public string AccountId { get; set; } = null!;
    }
}
