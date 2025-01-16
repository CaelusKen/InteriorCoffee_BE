using InteriorCoffee.Domain.Models.Documents;
using System;
using System.Collections.Generic;

namespace InteriorCoffee.Application.DTOs.Merchant
{
    public class MerchantResponseDTO
    {
        public int PageNo { get; set; }                      // current page number         
        public int PageSize { get; set; }                    // current page size (default is 12)
        public int ListSize { get; set; }                    // the size of list response when DB returns
        public int CurrentPageSize { get; set; }             // the current page size that the list is using (less or equal to page size)
        public int ListSizeAfter { get; set; }               // size of the list after applying pagination, sorting, or filters
        public int TotalPage { get; set; }                   // total pages that the list needs
        public MerchantOrderByDTO OrderBy { get; set; }      // the sorting options currently applied
        public MerchantFilterDTO Filter { get; set; }        // the filter options currently applied
        public string Keyword { get; set; }                  // Return keyword that user search for
        public List<MerchantResponseItemDTO> Merchants { get; set; }  // list of merchant response items
    }

    public class MerchantOrderByDTO
    {
        public string SortBy { get; set; }                   // Element that the list will be sorted by
        public bool IsAscending { get; set; }                // Check if sorting is ascending or not
    }

    public class MerchantFilterDTO                            // Elements used by filters
    {
        public string Status { get; set; }
    }

    public class MerchantResponseItemDTO                      // Elements to show in merchant list (not all, just some items)
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string LogoUrl { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string MerchantCode { get; set; }
        public string PolicyDocument { get; set; }
        public string Website { get; set; }
        public List<OrderIncome> OrderIncomes { get; set; }
    }
}
