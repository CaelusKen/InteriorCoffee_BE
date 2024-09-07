using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        public Task<Role> GetRoleById(string id);
        public Task<Role> GetRoleByName(string roleName);
    }
}
