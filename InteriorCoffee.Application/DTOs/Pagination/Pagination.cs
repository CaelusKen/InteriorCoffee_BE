using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Pagination
{
    public class Pagination
    {
        // DEFAULT VALUES
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
