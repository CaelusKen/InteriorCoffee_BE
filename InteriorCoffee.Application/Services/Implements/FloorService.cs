using AutoMapper;
using InteriorCoffee.Application.DTOs.Floor;
using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using Interior.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using InteriorCoffee.Application.Services.Base;
using InteriorCoffee.Domain.ErrorModel;
using Microsoft.AspNetCore.Http;
using InteriorCoffee.Domain.Models.Documents;
using System.Text.Json;
using InteriorCoffee.Application.Utils;

namespace InteriorCoffee.Application.Services.Implements
{
    public class FloorService : BaseService<FloorService>, IFloorService
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IMapper _mapper;

        public FloorService(ILogger<FloorService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor, IFloorRepository floorRepository)
            : base(logger, mapper, httpContextAccessor)
        {
            _floorRepository = floorRepository;
            _mapper = mapper;
        }

        public async Task<Floor> GetFloorByIdAsync(string id)
        {
            //try
            //{
                var floor = await _floorRepository.GetFloorById(id);
                if (floor == null)
                {
                    throw new NotFoundException($"Floor with id {id} not found.");
                }
                return floor;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error occurred while getting the floor by id.");
            //    throw;
            //}
        }

        public async Task<string> CreateFloorAsync(CreateFloorDTO createFloorDTO)
        {
            //try
            //{
                var floor = _mapper.Map<Floor>(createFloorDTO);
                await _floorRepository.CreateFloor(floor);
                return floor._id;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error occurred while creating the floor.");
            //    throw;
            //}
        }

        public async Task UpdateFloorAsync(string id, JsonElement updateFloor)
        {
            var existingFloor = await _floorRepository.GetFloorById(id);
            if (existingFloor == null)
            {
                throw new NotFoundException($"Floor with id {id} not found.");
            }

            // Log existing floor details
            _logger.LogInformation("Existing floor before update: {existingFloor}", existingFloor);

            var existingFloorJson = JsonSerializer.Serialize(existingFloor, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var existingFloorElement = JsonDocument.Parse(existingFloorJson).RootElement;

            var mergedFloor = JsonUtil.MergeJsonElements(existingFloorElement, updateFloor);

            var jsonString = JsonSerializer.Serialize(mergedFloor, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            _logger.LogInformation("Merged floor JSON: {jsonString}", jsonString);

            var updateFloorDto = JsonSerializer.Deserialize<UpdateFloorDTO>(jsonString, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            // Preserve the existing _id before mapping
            var existingId = existingFloor._id;

            // Map the updated fields to the existing floor, excluding _id
            _mapper.Map(updateFloorDto, existingFloor);

            // Ensure the _id is preserved
            existingFloor._id = existingId;

            // Log updated floor details
            _logger.LogInformation("Updated floor after mapping: {existingFloor}", existingFloor);

            await _floorRepository.UpdateFloor(existingFloor);

            // Log updated floor from repository
            var updatedFloor = await _floorRepository.GetFloorById(id);
            _logger.LogInformation("Floor after update from repository: {updatedFloor}", updatedFloor);
        }


        public async Task DeleteFloorAsync(string id)
        {
            //try
            //{
                var floor = await _floorRepository.GetFloorById(id);
                if (floor == null)
                {
                    throw new NotFoundException($"Floor with id {id} not found.");
                }
                await _floorRepository.DeleteFloor(id);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error occurred while deleting the floor.");
            //    throw;
            //}
        }
    }
}
