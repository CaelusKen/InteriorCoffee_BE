using InteriorCoffee.Domain.Models.Documents;
using System;
using System.Collections.Generic;

namespace InteriorCoffee.Application.DTOs.Product
{
    /// <summary>
    /// DTO for product response.
    /// </summary>
    public class ProductResponseDTO
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
        /// Minimum price of the product in the list (get truePrice).
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Maximum price of the product in the list (get truePrice).
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// In stock counting.
        /// </summary>
        public int InStock { get; set; }

        /// <summary>
        /// Out of stock counting.
        /// </summary>
        public int OutOfStock { get; set; }

        /// <summary>
        /// The sorting options currently applied.
        /// </summary>
        public ProductOrderByDTO OrderBy { get; set; }

        /// <summary>
        /// The filter options currently applied.
        /// </summary>
        public ProductFilterDTO Filter { get; set; }

        /// <summary>
        /// The keyword used for searching.
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// List of product response items.
        /// </summary>
        public List<ProductResponseItemDTO> Products { get; set; }
    }

    /// <summary>
    /// DTO for sorting options.
    /// </summary>
    public class ProductOrderByDTO
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
    public class ProductFilterDTO
    {
        /// <summary>
        /// Status filter.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Category ID filter.
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// Merchant ID filter.
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// Availability filter.
        /// </summary>
        public bool? IsAvailability { get; set; }
    }

    /// <summary>
    /// DTO for product response items.
    /// </summary>
    public class ProductResponseItemDTO
    {
        /// <summary>
        /// Product ID.
        /// </summary>
        public string _id { get; set; }

        /// <summary>
        /// List of category IDs.
        /// </summary>
        public List<string> CategoryId { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product images.
        /// </summary>
        public ProductImages Images { get; set; }

        /// <summary>
        /// Selling price.
        /// </summary>
        public double SellingPrice { get; set; }

        /// <summary>
        /// Discount percentage.
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// True price after discount.
        /// </summary>
        public double TruePrice { get; set; }

        /// <summary>
        /// Quantity available.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Product status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Product dimensions.
        /// </summary>
        public string Dimensions { get; set; }

        /// <summary>
        /// List of materials used.
        /// </summary>
        public List<string> Materials { get; set; }

        /// <summary>
        /// URL of the model texture.
        /// </summary>
        public string ModelTextureUrl { get; set; }

        /// <summary>
        /// Date when the product was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Date when the product was last updated.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Campaign ID.
        /// </summary>
        public string CampaignId { get; set; }

        /// <summary>
        /// Merchant ID.
        /// </summary>
        public string MerchantId { get; set; }
    }
}
