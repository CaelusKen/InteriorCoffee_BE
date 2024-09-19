using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Domain.Paginate
{
    public static class PaginateExtension
    {
        public static async Task<IPaginate<TResult>> ToPaginateAsync<T, TResult>(this IFindFluent<T, TResult> mongoCollection, int page, int size, int firstPage = 1)
        {
            if (firstPage > page)
                throw new ArgumentException($"page ({page}) must greater or equal than firstPage ({firstPage})");
            var total = await mongoCollection.CountDocumentsAsync();
            var items = await mongoCollection.Skip((page - firstPage) * size).Limit(size).ToListAsync();
            var totalPages = (int)Math.Ceiling(total / (double)size);
            return new Paginate<TResult>
            {
                Page = page,
                Size = size,
                TotalItems = total,
                Items = items,
                TotalPages = totalPages
            };
        }
    }
}
