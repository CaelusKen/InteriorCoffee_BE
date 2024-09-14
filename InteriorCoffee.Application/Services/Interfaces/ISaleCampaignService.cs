using InteriorCoffee.Application.DTOs.SaleCampaign;
using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface ISaleCampaignService
    {
        Task<(List<SaleCampaign>, int, int, int, int)> GetSaleCampaignsAsync(int? pageNo, int? pageSize);
        public Task<SaleCampaign> GetCampaignById(string id);
        public Task CreateCampaign(CreateSaleCampaignDTO createdSaleCampaign);
        public Task UpdateCampaign(string id, UpdateSaleCampaignDTO updatedCampaign);
        public Task DeleteCampagin(string id);
    }
}
