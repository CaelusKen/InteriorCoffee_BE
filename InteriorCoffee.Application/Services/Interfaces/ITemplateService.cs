using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Application.DTOs.Template;
using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<TemplateResponseDTO> GetTemplatesAsync(int? pageNo, int? pageSize, OrderBy orderBy, TemplateFilterDTO filter, string keyword);
        Task<GetTemplateDTO> GetTemplateById(string id);
        Task CreateTemplate(CreateTemplateDTO template);
        Task UpdateTemplate(string id, UpdateTemplateDTO updateTemplate);
        Task DeleteTemplate(string id);
    }
}
