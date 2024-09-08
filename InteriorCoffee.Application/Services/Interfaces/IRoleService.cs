using InteriorCoffee.Application.DTOs.Role;
using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IRoleService
    {
        public Task<List<Role>> GetAllRoles();
        public Task<Role> GetRoleById(string id);
        public Task CreateRole(RoleDTO roleDTO);
        public Task UpdateRole(string id, RoleDTO roleDTO);
        public Task DeleteRole(string id);
    }
}
