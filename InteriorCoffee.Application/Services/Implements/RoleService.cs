using AutoMapper;
using InteriorCoffee.Application.DTOs.Role;
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
    public class RoleService : BaseService<RoleService>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(ILogger<RoleService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IRoleRepository roleRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            return await _roleRepository.GetRoleListByCondition();
        }

        public async Task<Role> GetRoleById(string id)
        {
            return await _roleRepository.GetRoleByCondition(
                predicate: ro => ro._id.Equals(id));
        }

        public async Task CreateRole(RoleDTO roleDTO)
        {
            Role newRole = _mapper.Map<Role>(roleDTO);
            await _roleRepository.CreateRole(newRole);
        }

        public async Task UpdateRole(string id, RoleDTO roleDTO)
        {
            Role role = await _roleRepository.GetRoleByCondition(
                predicate: ro => ro._id.Equals(id));

            if (role == null) throw new NotFoundException($"Role id {id} cannot be found");

            //Update role data
            role.Name = String.IsNullOrEmpty(roleDTO.Name) ? role.Name : roleDTO.Name;
            role.Description = String.IsNullOrEmpty(roleDTO.Description) ? role.Description : roleDTO.Description;

            await _roleRepository.UpdateRole(role);
        }

        public async Task DeleteRole(string id)
        {
            Role role = await _roleRepository.GetRoleByCondition(
                predicate: ro => ro._id.Equals(id));

            if (role == null) throw new NotFoundException($"Role id {id} cannot be found");

            await _roleRepository.DeleteRole(id);
        }
    }
}
