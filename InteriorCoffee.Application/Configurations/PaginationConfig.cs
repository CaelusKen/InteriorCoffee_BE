using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Configurations
{
    public static class PaginationConfig
    {
        public const int DefaultPageNo = 1;
        public const int DefaultPageSize = 12;
        public static bool UseDynamicPageSize { get; set; } = true;
    }
}

