using System.Text.Json;
using InteriorCoffee.Application.DTOs.Floor;
using InteriorCoffee.Domain.Models;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IFloorService
    {
        Task<Floor> GetFloorByIdAsync(string id);
        Task<string> CreateFloorAsync(CreateFloorDTO createFloorDTO);
        Task UpdateFloorAsync(string id, JsonElement updateFloor);
        Task DeleteFloorAsync(string id);
    }
}
