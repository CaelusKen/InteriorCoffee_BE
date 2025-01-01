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
        Task<SaleCampaign> GetCampaignById(string id);
        Task CreateCampaign(CreateSaleCampaignDTO createdSaleCampaign);
        Task UpdateCampaign(string id, UpdateSaleCampaignDTO updatedCampaign);
        Task DeleteCampagin(string id);

        //public Task AddProductsToCampaign(string campaignId, List<string> productIds);
        //public Task RemoveAllProductsInCampaign(string campaignId);
    }
}
