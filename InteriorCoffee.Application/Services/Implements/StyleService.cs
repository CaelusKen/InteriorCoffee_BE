using AutoMapper;
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

        public async Task<List<Style>> GetAllStyles()
        {
            return await _styleRepository.GetStyleList();
        }

        public async Task<Style> GetStyleById(string id)
        {
            return await _styleRepository.GetStyle(
                predicate: s => s._id.Equals(id));
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
