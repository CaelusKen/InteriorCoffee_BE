using AutoMapper;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Implements
{
    public class CampaignProductsService : BaseService<CampaignProductsService>, ICampaignProductsService
    {
        public CampaignProductsService(ILogger<CampaignProductsService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(logger, mapper, httpContextAccessor)
        {
        }
    }
}
