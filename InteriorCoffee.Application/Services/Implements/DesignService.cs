using AutoMapper;
using Interior.Infrastructure.Repositories.Interfaces;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Design;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class DesignService : BaseService<DesignService>, IDesignService
    {
        private readonly IDesignRepository _designRepository;
        private readonly IFloorRepository _floorRepository;

        public DesignService(ILogger<DesignService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IDesignRepository designRepository,
            IFloorRepository floorRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _designRepository = designRepository;
            _floorRepository = floorRepository;
        }

        #region Utility Function
        #region "Dictionary"
        private static readonly Dictionary<string, string> SortableProperties = new Dictionary<string, string>
        {
            { "name", "Name" },
            { "createddate", "CreatedDate" },
            { "updateddate", "UpdatedDate" },
            { "status", "Status" },
            { "type", "Type" }
        };
        #endregion

        #region "Sorting"
        private List<Design> ApplySorting(List<Design> designs, OrderBy orderBy)
        {
            if (orderBy != null)
            {
                if (SortableProperties.TryGetValue(orderBy.SortBy.ToLower(), out var propertyName))
                {
                    var propertyInfo = typeof(Design).GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        designs = orderBy.Ascending
                            ? designs.OrderBy(d => propertyInfo.GetValue(d, null)).ToList()
                            : designs.OrderByDescending(d => propertyInfo.GetValue(d, null)).ToList();
                    }
                }
                else
                {
                    throw new ArgumentException($"Property '{orderBy.SortBy}' does not exist on type 'Design'.");
                }
            }
            return designs;
        }
        #endregion

        #region "Filtering"
        private List<Design> ApplyFilters(List<Design> designs, string status, string type, List<string> categories)
        {
            if (!string.IsNullOrEmpty(status))
            {
                designs = designs.Where(d => d.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(type))
            {
                designs = designs.Where(d => d.Type.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (categories != null && categories.Any())
            {
                designs = designs.Where(d => d.Categories.Any(c => categories.Contains(c))).ToList();
            }

            return designs;
        }
        #endregion
        #endregion

        public async Task<DesignResponseDTO> GetDesignsAsync(int? pageNo, int? pageSize, OrderBy orderBy, DesignFilterDTO filter, string keyword)
        {
            try
            {
                var (allDesigns, totalItems) = await _designRepository.GetDesignsAsync();

                // Apply filters
                allDesigns = ApplyFilters(allDesigns, filter.Status, filter.Type, filter.Categories);

                // Apply keyword search
                if (!string.IsNullOrEmpty(keyword))
                {
                    allDesigns = allDesigns.Where(d => d.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                       d.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                           .ToList();
                }

                // Apply sorting logic only if orderBy is not null
                allDesigns = ApplySorting(allDesigns, orderBy);

                // Determine the page size dynamically if not provided
                var finalPageSize = pageSize ?? (PaginationConfig.UseDynamicPageSize ? allDesigns.Count : PaginationConfig.DefaultPageSize);

                // Calculate pagination details based on finalPageSize
                var totalPages = (int)Math.Ceiling((double)allDesigns.Count / finalPageSize);

                // Handle page boundaries
                var paginationPageNo = pageNo ?? 1;
                if (paginationPageNo > totalPages) paginationPageNo = totalPages;
                if (paginationPageNo < 1) paginationPageNo = 1;

                // Paginate the filtered designs
                var paginatedDesigns = allDesigns.Skip((paginationPageNo - 1) * finalPageSize)
                                                 .Take(finalPageSize)
                                                 .ToList();

                // Update the listAfter to reflect the current page size
                var listAfter = paginatedDesigns.Count;

                var designResponseItems = _mapper.Map<List<DesignResponseItemDTO>>(paginatedDesigns);

                #region "Mapping"
                return new DesignResponseDTO
                {
                    PageNo = paginationPageNo,
                    PageSize = finalPageSize,
                    ListSize = totalItems,
                    CurrentPageSize = listAfter,
                    ListSizeAfter = listAfter,
                    TotalPage = totalPages,
                    OrderBy = new DesignOrderByDTO
                    {
                        SortBy = orderBy?.SortBy,
                        IsAscending = orderBy?.Ascending ?? true
                    },
                    Filter = new DesignFilterDTO
                    {
                        Status = filter.Status,
                        Type = filter.Type,
                        Categories = filter.Categories
                    },
                    Keyword = keyword,
                    Designs = designResponseItems
                };
                #endregion
            }
            #region "Catch error"
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting designs.");
                return new DesignResponseDTO
                {
                    PageNo = 1,
                    PageSize = 0,
                    ListSize = 0,
                    CurrentPageSize = 0,
                    ListSizeAfter = 0,
                    TotalPage = 0,
                    OrderBy = new DesignOrderByDTO(),
                    Filter = new DesignFilterDTO(),
                    Keyword = keyword,
                    Designs = new List<DesignResponseItemDTO>()
                };
            }
            #endregion
        }

        public async Task<GetDesignDTO> GetDesignByIdAsync(string id)
        {
            var design = await _designRepository.GetDesignById(id);
            if (design == null)
            {
                throw new NotFoundException($"Design with id {id} not found.");
            }

            //Get floor from floor repo
            var floors = await _floorRepository.GetFloorList(predicate: f => f.DesignTemplateId == id);

            GetDesignDTO result = _mapper.Map<GetDesignDTO>(design);
            result.Floors = floors;

            return result;
        }

        public async Task CreateDesignAsync(CreateDesignDTO createDesignDTO)
        {
            var design = _mapper.Map<Design>(createDesignDTO);
            //Add floors if initial design have floors
            if(createDesignDTO.Floors != null)
            {
                List<Floor> floors = _mapper.Map<List<Floor>>(createDesignDTO.Floors);
                //floors.ForEach(x => x.DesignTemplateId = design._id);
                await _floorRepository.AddRange(floors);
            }

            await _designRepository.CreateDesign(design);
        }

        public async Task UpdateDesignAsync(string id, UpdateDesignDTO updateDesignDTO)
        {
            var existingDesign = await _designRepository.GetDesignById(id);
            if (existingDesign == null)
            {
                throw new NotFoundException($"Design with id {id} not found.");
            }

            //Update Design Information
            _mapper.Map(updateDesignDTO, existingDesign);
            await _designRepository.UpdateDesign(existingDesign);

            //Update Floors information
            foreach(Floor floor in updateDesignDTO.Floors)
            {
                await _floorRepository.UpdateFloor(floor);
            }
        }

        public async Task DeleteDesignAsync(string id)
        {
            var design = await _designRepository.GetDesignById(id);
            if (design == null)
            {
                throw new NotFoundException($"Design with id {id} not found.");
            }

            //Delete all floors of design
            await _floorRepository.DeleteAllFloorsInDesign(design._id);
            await _designRepository.DeleteDesign(id);
        }
    }
}
