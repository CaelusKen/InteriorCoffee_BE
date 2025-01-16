using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Paginate
{
    public interface IPaginate<TResult>
    {
        int PageSize { get; }
        int PageNo { get; }
        long TotalItems { get; }
        int TotalPages { get; }
        IList<TResult> Items { get; }
    }
}
