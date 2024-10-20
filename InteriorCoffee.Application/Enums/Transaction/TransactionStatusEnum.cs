using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Enums.Transaction
{
    public enum TransactionStatusEnum
    {
        PENDING = 1,
        COMPLETED = 2,
        FAILED = 3,
        CANCELLED = 4,
        REFUNDED = 5,
    }
}
