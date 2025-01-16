using InteriorCoffee.Application.DTOs.Design;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IDesignService
    {
        Task<DesignResponseDTO> GetDesignsAsync(int? pageNo, int? pageSize, OrderBy orderBy, DesignFilterDTO filter, string keyword);
        Task<GetDesignDTO> GetDesignByIdAsync(string id);
        Task CreateDesignAsync(CreateDesignDTO design);
        Task UpdateDesignAsync(string id, UpdateDesignDTO design);
        Task DeleteDesignAsync(string id);
    }
}
