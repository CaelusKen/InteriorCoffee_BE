using AutoMapper;
using InteriorCoffee.Application.DTOs.SaleCampaign;
using InteriorCoffee.Application.Enums.SaleCampaign;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Mappers.SaleCampaigns
{
    public class SaleCampaignMapper : Profile
    {
        public SaleCampaignMapper()
        {
            CreateMap<CreateSaleCampaignDTO, SaleCampaign>()
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                .ForMember(des => des.Status, src => src.MapFrom(src => CampaignStatusEnum.ACTIVE.ToString()));
        }
    }
}
