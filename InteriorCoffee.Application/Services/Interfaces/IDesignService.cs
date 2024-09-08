using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IDesignService
    {
        Task<IEnumerable<Design>> GetAllDesignsAsync();
        Task<Design> GetDesignByIdAsync(string id);
        Task CreateDesignAsync(Design design);
        Task UpdateDesignAsync(string id, Design design);
        Task DeleteDesignAsync(string id);
    }
}
