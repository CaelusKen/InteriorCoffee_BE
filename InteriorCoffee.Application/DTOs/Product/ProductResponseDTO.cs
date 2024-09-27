using InteriorCoffee.Domain.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Product
{
    public class ProductResponseDTO
    {
        public int PageNo { get; set; }                     // current page number //         
        public int PageSize { get; set; }                   // current page size (default is 12)//
        public int ListSize { get; set; }                   // the size of list response when DB returns //
        public int CurrentPageSize { get; set; }            // the currect page size that the list using (less or equal than page size)//
        public int ListSizeAfter { get; set; }              // size of the list after apply pagination, order/sorting or filters or all of them //
        public int TotalPage { get; set; }                  // total pages that the list need //
        public decimal MinPrice { get; set; }               // minimum price of the product in the list (get truePrice) //
        public decimal MaxPrice { get; set; }               // maximum price of the product in the list (get truePrice) //
        public int InStock { get; set; }                    // in stock counting //
        public int OutOfStock { get; set; }                 // out of stock counting //
        public OrderByDTO OrderBy { get; set; }             // get the sorting options list currently apply //
        public FilterDTO Filter { get; set; }               // get the filter options list currently apply //
        public string Keyword { get; set; }                 // get the keyword that user search for //
        public List<ProductResponseItemDTO> Products { get; set; }  // list of product response item //
    }

    public class OrderByDTO
    {
        public string SortBy { get; set; }                  // Element that list will be sorted by //
        public bool isAscending { get; set; }               // Check ascending or not //
    }

    public class FilterDTO                                  // Below are elements that will use by filters //
    {
        public string Status { get; set; }                  
        public string CategoryId { get; set; }
        public string MerchantId { get; set; }
        public bool? IsAvailability { get; set; }
    }

    public class ProductResponseItemDTO                     // Below are what will show in product list (not show all, just some items) //
    {
        // test response object
        public string _id { get; set; } = null!;
        public List<string> CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ProductImages Images { get; set; }
        public double SellingPrice { get; set; }
        public int Discount { get; set; }
        public double TruePrice { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string Dimensions { get; set; }
        public List<string> Materials { get; set; }
        public string ModelTextureUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public string CampaignId { get; set; }
        public string MerchantId { get; set; }
    }

}
