using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.SaleCampaign
{
    public class UpdateSaleCampaignDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double Value { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }

        public List<string>? CampaignProductIds { get; set; }
    }
}
