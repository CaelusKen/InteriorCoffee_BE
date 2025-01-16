using InteriorCoffee.Application.Configurations;
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
        public int PageNo { get; set; } = PaginationConfig.DefaultPageNo;
        public int PageSize { get; set; } = PaginationConfig.DefaultPageSize;
    }
}
