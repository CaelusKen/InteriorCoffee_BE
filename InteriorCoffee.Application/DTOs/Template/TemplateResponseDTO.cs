using InteriorCoffee.Domain.Models.Documents;
using System;
using System.Collections.Generic;

namespace InteriorCoffee.Application.DTOs.Template
{
    public class TemplateResponseDTO
    {
        public int PageNo { get; set; }                      // current page number         
        public int PageSize { get; set; }                    // current page size (default is 12)
        public int ListSize { get; set; }                    // the size of list response when DB returns
        public int CurrentPageSize { get; set; }             // the current page size that the list is using (less or equal to page size)
        public int ListSizeAfter { get; set; }               // size of the list after applying pagination, sorting, or filters
        public int TotalPage { get; set; }                   // total pages that the list needs
        public TemplateOrderByDTO OrderBy { get; set; }      // the sorting options currently applied
        public TemplateFilterDTO Filter { get; set; }        // the filter options currently applied
        public string Keyword { get; set; }                  // Return keyword that user search for
        public List<TemplateResponseItemDTO> Templates { get; set; }  // list of template response items
    }

    public class TemplateOrderByDTO
    {
        public string SortBy { get; set; }                   // Element that the list will be sorted by
        public bool IsAscending { get; set; }                // Check if sorting is ascending or not
    }

    public class TemplateFilterDTO                            // Elements used by filters
    {
        public string Status { get; set; }
        public string Type { get; set; }
        public List<string> Categories { get; set; }
    }

    public class TemplateResponseItemDTO                      // Elements to show in template list (not all, just some items)
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public List<string> Categories { get; set; }
        public List<ProductList> Products { get; set; }
        public string AccountId { get; set; }
        public string MerchantId { get; set; }
        public string StyleId { get; set; }
    }
}
