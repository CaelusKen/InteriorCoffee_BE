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
        public Task<List<SaleCampaign>> GetAllCampaigns();
        public Task<SaleCampaign> GetCampaignById(string id);
        public Task CreateCampaign(CreateSaleCampaignDTO createdSaleCampaign);
        public Task UpdateCampaign(string id, UpdateSaleCampaignDTO updatedCampaign);
        public Task DeleteCampagin(string id);
    }
}
