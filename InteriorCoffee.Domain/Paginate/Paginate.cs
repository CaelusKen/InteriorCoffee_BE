using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Paginate
{
    public class Paginate<TResult> : IPaginate<TResult>
    {
        public int PageSize { get; set; }
        public int PageNo { get; set; }
        public long TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IList<TResult> Items { get; set; }

        public Paginate(IEnumerable<TResult> source, int page, int size, int firstPage)
        {
            var enumerable = source as TResult[] ?? source.ToArray();

            if (firstPage > page) throw new ArgumentException($"Page ({page}) must be greater or equal than firstPage ({firstPage})");

            if(source is IQueryable<TResult> queryable)
            {
                PageNo = page;
                PageSize = size;
                TotalItems = queryable.Count();
                Items = queryable.Skip((page - firstPage) * size).Take(size).ToList();
                TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
            }
            else
            {
                PageNo = page;
                PageSize = size;
                TotalItems = enumerable.Length;
                Items = enumerable.Skip((page - firstPage) * size).Take(size).ToList();
                TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);
            }
        }

        public Paginate()
        {
            Items = Array.Empty<TResult>();
        }
    }
}
