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
            var floor = await _floorRepository.GetFloorById(id);
            if (floor == null)
            {
                throw new NotFoundException($"Floor with id {id} not found.");
            }
            return floor;
        }

        public async Task<string> CreateFloorAsync(CreateFloorDTO createFloorDTO)
        {
            var floor = _mapper.Map<Floor>(createFloorDTO);
            await _floorRepository.CreateFloor(floor);
            return floor._id;
        }

        public async Task UpdateFloorAsync(UpdateFloorDTO updateFloorDTO)
        {
            var existingFloor = await _floorRepository.GetFloorById(updateFloorDTO._id);
            if (existingFloor == null)
            {
                throw new NotFoundException($"Floor with id {updateFloorDTO._id} not found.");
            }

            _mapper.Map(updateFloorDTO, existingFloor);
            await _floorRepository.UpdateFloor(existingFloor);
        }

        public async Task DeleteFloorAsync(string id)
        {
            var floor = await _floorRepository.GetFloorById(id);
            if (floor == null)
            {
                throw new NotFoundException($"Floor with id {id} not found.");
            }
            await _floorRepository.DeleteFloor(id);
        }
    }
}
