using InteriorCoffee.Application.DTOs.Floor;
using InteriorCoffee.Domain.Models;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IFloorService
    {
        Task<Floor> GetFloorByIdAsync(string id);
        Task<string> CreateFloorAsync(CreateFloorDTO createFloorDTO);
        Task UpdateFloorAsync(UpdateFloorDTO updateFloorDTO);
        Task DeleteFloorAsync(string id);
    }
}
