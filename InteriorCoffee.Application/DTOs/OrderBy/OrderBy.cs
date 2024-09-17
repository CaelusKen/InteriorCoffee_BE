using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.OrderBy
{
    public class OrderBy
    {
        public string SortBy { get; set; }
        public bool Ascending { get; set; }

        public OrderBy(string sortBy, bool ascending)
        {
            SortBy = sortBy;
            Ascending = ascending;
        }
    }
}
