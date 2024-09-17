using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Pagination;
using InteriorCoffee.Application.DTOs.Style;
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
    public class StyleService : BaseService<StyleService>, IStyleService
    {
        private readonly IStyleRepository _styleRepository;

        public StyleService(ILogger<StyleService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IStyleRepository styleRepository) : base(logger, mapper, httpContextAccessor)
        {
            _styleRepository = styleRepository;
        }

        public async Task<(List<Style>, int, int, int, int)> GetStylesAsync(int? pageNo, int? pageSize)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            try
            {
                var (allStyles, totalItems) = await _styleRepository.GetStylesAsync();
                var totalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize);

                // Handle page boundaries
                if (pagination.PageNo > totalPages) pagination.PageNo = totalPages;
                if (pagination.PageNo < 1) pagination.PageNo = 1;

                var styles = allStyles.Skip((pagination.PageNo - 1) * pagination.PageSize)
                                      .Take(pagination.PageSize)
                                      .ToList();

                return (styles, pagination.PageNo, pagination.PageSize, totalItems, totalPages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paginated styles.");
                return (new List<Style>(), pagination.PageNo, pagination.PageSize, 0, 0);
            }
        }



        public async Task<Style> GetStyleById(string id)
        {
            var result = await _styleRepository.GetStyle(
                predicate: s => s._id.Equals(id));

            if(result == null) throw new NotFoundException($"Style id {id} cannot be found");

            return result;
        }

        public async Task CreateStyle(StyleDTO styleDTO)
        {
            Style newStyle = _mapper.Map<Style>(styleDTO);
            await _styleRepository.CreateStyle(newStyle);
        }

        public async Task UpdateStyle(string id, StyleDTO styleDTO)
        {
            Style style = await _styleRepository.GetStyle(
                predicate: s => s._id.Equals(id));

            if (style == null) throw new NotFoundException($"Style id {id} cannot be found");

            //Update style data
            style.Name = String.IsNullOrEmpty(styleDTO.Name) ? style.Name : styleDTO.Name;
            style.Description = String.IsNullOrEmpty(styleDTO.Description) ? style.Description : styleDTO.Description;

            await _styleRepository.UpdateStyle(style);
        }

        public async Task DeleteStyle(string id)
        {
            Style style = await _styleRepository.GetStyle(
                predicate: s => s._id.Equals(id));

            if (style == null) throw new NotFoundException($"Style id {id} cannot be found");

            await _styleRepository.DeleteStyle(id);
        }
    }
}
