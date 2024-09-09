using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Enums.Order
{
    public enum OrderStatusEnum
    {
        PENDING,
        PROCESSING,
        SHIPPED,
        DELIVERED,
        CANCELLED,
        RETURNED
    }
}
