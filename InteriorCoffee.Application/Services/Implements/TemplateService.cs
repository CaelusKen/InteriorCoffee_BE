using AutoMapper;
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

        public TemplateService(ILogger<TemplateService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, ITemplateRepository templateRepository) : base(logger, mapper, httpContextAccessor)
        {
            _templateRepository = templateRepository;
        }

        public async Task<List<Template>> GetAllTemplates()
        {
            return await _templateRepository.GetTemplateList();
        }

        public async Task<Template> GetTemplateById(string id)
        {
            var result = await _templateRepository.GetTemplate(
                predicate: t => t._id.Equals(id));

            if(result == null) throw new NotFoundException($"Template id {id} cannot be found");

            return result;
        }

        public async Task CreateTemplate(CreateTemplateDTO template)
        {
            Template newTemplate = _mapper.Map<Template>(template);
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

            await _templateRepository.UpdateTemplate(template);
        }

        public async Task DeleteTemplate(string id)
        {
            Template template = await _templateRepository.GetTemplate(
                predicate: t => t._id.Equals(id));

            if (template == null) throw new NotFoundException($"Template id {id} cannot be found");

            await _templateRepository.DeleteTemplate(id);
        }
    }
}
