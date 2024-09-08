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
        public Task<List<Template>> GetAllTemplates();
        public Task<Template> GetTemplateById(string id);
        public Task CreateTemplate(CreateTemplateDTO template);
        public Task UpdateTemplate(string id, UpdateTemplateDTO updateTemplate);
        public Task DeleteTemplate(string id);
    }
}
