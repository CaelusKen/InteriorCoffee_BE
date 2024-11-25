using AutoMapper;
using Interior.Infrastructure.Repositories.Interfaces;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Design;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.DTOs.Template;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class TemplateService : BaseService<TemplateService>, ITemplateService
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly IFloorRepository _floorRepository;

        public TemplateService(ILogger<TemplateService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, ITemplateRepository templateRepository,
            IFloorRepository floorRepository) : base(logger, mapper, httpContextAccessor)
        {
            _templateRepository = templateRepository;
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
        private List<Template> ApplySorting(List<Template> templates, OrderBy orderBy)
        {
            if (orderBy != null)
            {
                if (SortableProperties.TryGetValue(orderBy.SortBy.ToLower(), out var propertyName))
                {
                    var propertyInfo = typeof(Template).GetProperty(propertyName);
                    if (propertyInfo != null)
                    {
                        templates = orderBy.Ascending
                            ? templates.OrderBy(t => propertyInfo.GetValue(t, null)).ToList()
                            : templates.OrderByDescending(t => propertyInfo.GetValue(t, null)).ToList();
                    }
                }
                else
                {
                    throw new ArgumentException($"Property '{orderBy.SortBy}' does not exist on type 'Template'.");
                }
            }
            return templates;
        }
        #endregion

        #region "Filtering"
        private List<Template> ApplyFilters(List<Template> templates, string status, string type, List<string> categories)
        {
            if (!string.IsNullOrEmpty(status))
            {
                templates = templates.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(type))
            {
                templates = templates.Where(t => t.Type.Equals(type, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (categories != null && categories.Any())
            {
                templates = templates.Where(t => t.Categories.Any(c => categories.Contains(c))).ToList();
            }

            return templates;
        }
        #endregion
        #endregion

        public async Task<TemplateResponseDTO> GetTemplatesAsync(int? pageNo, int? pageSize, OrderBy orderBy, TemplateFilterDTO filter, string keyword)
        {
            try
            {
                var (allTemplates, totalItems) = await _templateRepository.GetTemplatesAsync();

                // Apply filters
                allTemplates = ApplyFilters(allTemplates, filter.Status, filter.Type, filter.Categories);

                // Apply keyword search
                if (!string.IsNullOrEmpty(keyword))
                {
                    allTemplates = allTemplates.Where(t => t.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                                           t.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                               .ToList();
                }

                // Apply sorting logic only if orderBy is not null
                allTemplates = ApplySorting(allTemplates, orderBy);

                // Determine the page size dynamically if not provided
                var finalPageSize = pageSize ?? (PaginationConfig.UseDynamicPageSize ? allTemplates.Count : PaginationConfig.DefaultPageSize);

                // Calculate pagination details based on finalPageSize
                var totalPages = (int)Math.Ceiling((double)allTemplates.Count / finalPageSize);

                // Handle page boundaries
                var paginationPageNo = pageNo ?? 1;
                if (paginationPageNo > totalPages) paginationPageNo = totalPages;
                if (paginationPageNo < 1) paginationPageNo = 1;

                // Paginate the filtered templates
                var paginatedTemplates = allTemplates.Skip((paginationPageNo - 1) * finalPageSize)
                                                     .Take(finalPageSize)
                                                     .ToList();

                // Update the listAfter to reflect the current page size
                var listAfter = paginatedTemplates.Count;

                var templateResponseItems = _mapper.Map<List<TemplateResponseItemDTO>>(paginatedTemplates);

                #region "Mapping"
                return new TemplateResponseDTO
                {
                    PageNo = paginationPageNo,
                    PageSize = finalPageSize,
                    ListSize = totalItems,
                    CurrentPageSize = listAfter,
                    ListSizeAfter = listAfter,
                    TotalPage = totalPages,
                    OrderBy = new TemplateOrderByDTO
                    {
                        SortBy = orderBy?.SortBy,
                        IsAscending = orderBy?.Ascending ?? true
                    },
                    Filter = new TemplateFilterDTO
                    {
                        Status = filter.Status,
                        Type = filter.Type,
                        Categories = filter.Categories
                    },
                    Keyword = keyword,
                    Templates = templateResponseItems
                };
                #endregion
            }
            #region "Catch error"
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting templates.");
                return new TemplateResponseDTO
                {
                    PageNo = 1,
                    PageSize = 0,
                    ListSize = 0,
                    CurrentPageSize = 0,
                    ListSizeAfter = 0,
                    TotalPage = 0,
                    OrderBy = new TemplateOrderByDTO(),
                    Filter = new TemplateFilterDTO(),
                    Keyword = keyword,
                    Templates = new List<TemplateResponseItemDTO>()
                };
            }
            #endregion
        }

        public async Task<GetTemplateDTO> GetTemplateById(string id)
        {
            var template = await _templateRepository.GetTemplate(
                predicate: t => t._id.Equals(id));

            if (template == null) throw new NotFoundException($"Template id {id} cannot be found");

            //Get floor from floor repo
            var floors = await _floorRepository.GetFloorList(predicate: f => f.DesignTemplateId == id);

            GetTemplateDTO result = _mapper.Map<GetTemplateDTO>(template);
            result.Floors = floors;

            return result;
        }

        public async Task CreateTemplate(CreateTemplateDTO template)
        {
            Template newTemplate = _mapper.Map<Template>(template);

            //Add floors if initial template have floors
            if(template.Floors != null)
            {
                List<Floor> floors = _mapper.Map<List<Floor>>(template.Floors);
                //floors.ForEach(x => x.DesignTemplateId = newTemplate._id);
                await _floorRepository.AddRange(floors);
            }

            await _templateRepository.CreateTemplate(newTemplate);
        }

        public async Task UpdateTemplate(string id, UpdateTemplateDTO updateTemplate)
        {
            Template template = await _templateRepository.GetTemplate(
                predicate: t => t._id.Equals(id));

            if (template == null) throw new NotFoundException($"Template id {id} cannot be found");

            //Update template data
            template.Name = String.IsNullOrEmpty(updateTemplate.Name) ? template.Name : updateTemplate.Name;
            template.Description = String.IsNullOrEmpty(updateTemplate.Description) ? template.Description : updateTemplate.Description;
            template.Type = String.IsNullOrEmpty(updateTemplate.Type) ? template.Type : updateTemplate.Type;
            template.Status = String.IsNullOrEmpty(updateTemplate.Status) ? template.Status : updateTemplate.Status;
            template.StyleId = String.IsNullOrEmpty(updateTemplate.StyleId) ? template.StyleId : updateTemplate.StyleId;
            template.Image = String.IsNullOrEmpty(updateTemplate.Image) ? template.Image : updateTemplate.Image;
            template.Categories = updateTemplate.Categories == null ? template.Categories : updateTemplate.Categories;
            template.Products = updateTemplate.Products == null ? template.Products : updateTemplate.Products;

            await _templateRepository.UpdateTemplate(template);

            //Update Floors information
            if (updateTemplate.Floors != null)
            {
                await _floorRepository.DeleteAllFloorsInDesign(id);

                updateTemplate.Floors.ForEach(f => f.DesignTemplateId = id);
                await _floorRepository.AddRange(updateTemplate.Floors);
            }
        }

        public async Task DeleteTemplate(string id)
        {
            Template template = await _templateRepository.GetTemplate(
                predicate: t => t._id.Equals(id));

            if (template == null) throw new NotFoundException($"Template id {id} cannot be found");

            //Delete all floors of template
            await _floorRepository.DeleteAllFloorsInDesign(template._id);
            await _templateRepository.DeleteTemplate(id);
        }
    }
}
