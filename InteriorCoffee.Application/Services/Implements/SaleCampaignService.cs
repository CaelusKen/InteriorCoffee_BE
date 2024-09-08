using AutoMapper;
using InteriorCoffee.Application.DTOs.SaleCampaign;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.ErrorModel;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class SaleCampaignService : BaseService<SaleCampaignService>, ISaleCampaignService
    {
        private readonly ISaleCampaignRepository _saleCampaignRepository;

        public SaleCampaignService(ILogger<SaleCampaignService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, ISaleCampaignRepository saleCampaignRepository) 
            : base(logger, mapper, httpContextAccessor)
        {
            _saleCampaignRepository = saleCampaignRepository;
        }

        public async Task<List<SaleCampaign>> GetAllCampaigns()
        {
            return await _saleCampaignRepository.GetSaleCampaignListByCondition();
        }

        public async Task<SaleCampaign> GetCampaignById(string id)
        {
            return await _saleCampaignRepository.GetSaleCampaignByCondition(
                predicate: sc => sc._id.Equals(id));
        }

        public async Task CreateCampaign(CreateSaleCampaignDTO createdSaleCampaign)
        {
            SaleCampaign saleCampaign = _mapper.Map<SaleCampaign>(createdSaleCampaign);
            await _saleCampaignRepository.CreateSaleCampaign(saleCampaign);
        }

        public async Task UpdateCampaign(string id, UpdateSaleCampaignDTO updatedCampaign)
        {
            SaleCampaign campaign = await _saleCampaignRepository.GetSaleCampaignByCondition(
                predicate: sc => sc._id.Equals(id));

            if (campaign == null) throw new NotFoundException($"Campaign id {id} cannot be found");

            //Update campaign data
            campaign.Name = String.IsNullOrEmpty(updatedCampaign.Name) ? campaign.Name : updatedCampaign.Name;
            campaign.Description = String.IsNullOrEmpty(updatedCampaign.Description) ? campaign.Description : updatedCampaign.Description;
            campaign.Status = String.IsNullOrEmpty(updatedCampaign.Status) ? campaign.Status : updatedCampaign.Status;
            campaign.Value = updatedCampaign.Value;
            campaign.StartDate = updatedCampaign.StartDate;
            campaign.EndDate = updatedCampaign.EndDate;

            await _saleCampaignRepository.UpdateSaleCampaign(campaign);
        }

        public async Task DeleteCampagin(string id)
        {
            SaleCampaign campaign = await _saleCampaignRepository.GetSaleCampaignByCondition(
                predicate: sc => sc._id.Equals(id));

            if (campaign == null) throw new NotFoundException($"Campaign id {id} cannot be found");

            await _saleCampaignRepository.DeleteSaleCampaign(id);
        }
    }
}
