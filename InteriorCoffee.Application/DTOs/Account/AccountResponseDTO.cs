using InteriorCoffee.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.DTOs.Account
{
    public class AccountResponseDTO
    {
        public int PageNo { get; set; }                     // current page number //         
        public int PageSize { get; set; }                   // current page size (default is 12)//
        public int ListSize { get; set; }                   // the size of list response when DB returns //
        public int CurrentPageSize { get; set; }            // the current page size that the list is using (less or equal to page size)//
        public int ListSizeAfter { get; set; }              // size of the list after applying pagination, sorting, or filters //
        public int TotalPage { get; set; }                  // total pages that the list needs //
        public AccountOrderByDTO OrderBy { get; set; }             // the sorting options currently applied //
        public AccountFilterDTO Filter { get; set; }               // the filter options currently applied //
        public string Keyword { get; set; }                 // Return keyword that user search for //
        public List<AccountResponseItemDTO> Accounts { get; set; }  // list of account response items //
    }

    public class AccountOrderByDTO
    {
        public string SortBy { get; set; }                  // Element that the list will be sorted by //
        public bool isAscending { get; set; }               // Check if sorting is ascending or not //
    }

    public class AccountFilterDTO                                  // Elements used by filters //
    {
        public string Status { get; set; }
        public string RoleId { get; set; }
    }

    public class AccountResponseItemDTO                     // Elements to show in account list (not all, just some items) //
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string MerchantId { get; set; }
        public string RoleId { get; set; }
    }
}
