using System;
using System.Collections.Generic;

namespace InteriorCoffee.Application.DTOs.Account
{
    /// <summary>
    /// DTO for account response.
    /// </summary>
    public class AccountResponseDTO
    {
        /// <summary>
        /// Current page number.
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// Current page size (default is 12).
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// The size of list response when DB returns.
        /// </summary>
        public int ListSize { get; set; }

        /// <summary>
        /// The current page size that the list is using (less or equal to page size).
        /// </summary>
        public int CurrentPageSize { get; set; }

        /// <summary>
        /// Size of the list after applying pagination, sorting, or filters.
        /// </summary>
        public int ListSizeAfter { get; set; }

        /// <summary>
        /// Total pages that the list needs.
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// The sorting options currently applied.
        /// </summary>
        public AccountOrderByDTO OrderBy { get; set; }

        /// <summary>
        /// The filter options currently applied.
        /// </summary>
        public AccountFilterDTO Filter { get; set; }

        /// <summary>
        /// The keyword used for searching.
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// List of account response items.
        /// </summary>
        public List<AccountResponseItemDTO> Accounts { get; set; }
    }

    /// <summary>
    /// DTO for sorting options.
    /// </summary>
    public class AccountOrderByDTO
    {
        /// <summary>
        /// Element that the list will be sorted by.
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Check if sorting is ascending or not.
        /// </summary>
        public bool IsAscending { get; set; }
    }

    /// <summary>
    /// DTO for filter options.
    /// </summary>
    public class AccountFilterDTO
    {
        /// <summary>
        /// Status filter.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Role ID filter.
        /// </summary>
        public string RoleId { get; set; }
    }

    /// <summary>
    /// DTO for account response items.
    /// </summary>
    public class AccountResponseItemDTO
    {
        /// <summary>
        /// Account ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Username.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Account status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Avatar URL.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Date when the account was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the account was last updated.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Merchant ID.
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// Role ID.
        /// </summary>
        public string RoleId { get; set; }
    }
}
