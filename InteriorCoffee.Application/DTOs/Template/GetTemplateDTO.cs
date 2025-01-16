using InteriorCoffee.Domain.Models.Documents;
using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Template
{
    public class GetTemplateDTO
    {
        public string _id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Status { get; set; } = null!;
        public string Type { get; set; }
        public List<Domain.Models.Floor> Floors { get; set; }
        public List<string> Categories { get; set; }
        public List<ProductList> Products { get; set; }

        public string AccountId { get; set; } = null!;
        public string MerchantId { get; set; } = null!;
        public string StyleId { get; set; } = null!;
    }
}
