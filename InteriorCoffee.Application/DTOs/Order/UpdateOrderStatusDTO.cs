﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Order
{
    public class UpdateOrderStatusDTO
    {
        public string Status { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
