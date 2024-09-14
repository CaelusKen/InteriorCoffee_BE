using AutoMapper;
using InteriorCoffee.Application.Configurations;
using InteriorCoffee.Application.DTOs.Pagination;
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

        public async Task<(List<SaleCampaign>, int, int, int, int)> GetSaleCampaignsAsync(int? pageNo, int? pageSize)
        {
            var pagination = new Pagination
            {
                PageNo = pageNo ?? PaginationConfig.DefaultPageNo,
                PageSize = pageSize ?? PaginationConfig.DefaultPageSize
            };

            var (saleCampaigns, totalItems, currentPageSize, totalPages) = await _saleCampaignRepository.GetSaleCampaignsAsync(pagination.PageNo, pagination.PageSize);
            return (saleCampaigns, pagination.PageNo, currentPageSize, totalItems, totalPages);
        }


        public async Task<SaleCampaign> GetCampaignById(string id)
        {
            var result = await _saleCampaignRepository.GetSaleCampaign(
                predicate: sc => sc._id.Equals(id));

            if(result == null) throw new NotFoundException($"Sale campaign id {id} cannot be found");

            return result;
        }

        public async Task CreateCampaign(CreateSaleCampaignDTO createdSaleCampaign)
        {
            SaleCampaign saleCampaign = _mapper.Map<SaleCampaign>(createdSaleCampaign);
            await _saleCampaignRepository.CreateSaleCampaign(saleCampaign);
        }

        public async Task UpdateCampaign(string id, UpdateSaleCampaignDTO updatedCampaign)
        {
            SaleCampaign campaign = await _saleCampaignRepository.GetSaleCampaign(
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
            SaleCampaign campaign = await _saleCampaignRepository.GetSaleCampaign(
                predicate: sc => sc._id.Equals(id));

            if (campaign == null) throw new NotFoundException($"Campaign id {id} cannot be found");

            await _saleCampaignRepository.DeleteSaleCampaign(id);
        }
    }
}
