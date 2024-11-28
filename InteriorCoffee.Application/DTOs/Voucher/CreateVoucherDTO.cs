using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Voucher
{
    public class CreateVoucherDTO
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUse { get; set; }
        public double MinOrderValue { get; set; }

        public string Type { get; set; } = null!;
    }
}
