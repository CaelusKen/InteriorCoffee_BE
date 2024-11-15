using InteriorCoffee.Domain.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Template
{
    public class CreateTemplateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }

        public List<Floor> Floors { get; set; }

        public string AccountId { get; set; }
        public string MerchantId { get; set; }
        public string StyleId { get; set; }

        public List<string> Categories { get; set; }
    }
}
