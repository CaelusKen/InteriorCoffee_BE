using AutoMapper;
using InteriorCoffee.Application.DTOs.Role;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Mappers.Roles
{
    public class RoleMapper : Profile
    {
        public RoleMapper()
        {
            CreateMap<RoleDTO, Role>()
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()));
        }
    }
}
