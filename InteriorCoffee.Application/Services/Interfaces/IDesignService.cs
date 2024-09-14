using InteriorCoffee.Application.DTOs.Design;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IDesignService
    {
        Task<(List<Design>, int, int, int, int)> GetDesignsAsync(int? pageNo, int? pageSize);
        Task<Design> GetDesignByIdAsync(string id);
        Task CreateDesignAsync(CreateDesignDTO design);
        Task UpdateDesignAsync(string id, UpdateDesignDTO design);
        Task DeleteDesignAsync(string id);
    }
}
