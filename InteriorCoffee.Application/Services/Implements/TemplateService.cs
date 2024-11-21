﻿using AutoMapper;
using Interior.Infrastructure.Repositories.Interfaces;
using InteriorCoffee.Application.Configurations;
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

        public async Task<(List<Template>, int, int, int, int)> GetTemplatesAsync(int? pageNo, int? pageSize)
        {
            try
            {
                var (allTemplates, totalItems) = await _templateRepository.GetTemplatesAsync();

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

                return (paginatedTemplates, paginationPageNo, finalPageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated templates.");
                return (new List<Template>(), 1, 0, 0, 0);
            }
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
                await _floorRepository.AddRange(template.Floors);
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
