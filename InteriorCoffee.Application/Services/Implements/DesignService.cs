using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InteriorCoffee.Application.Services.Implements
{
    public class DesignService : IDesignService
    {
        private readonly IDesignRepository _designRepository;
        private readonly ILogger<DesignService> _logger;

        public DesignService(IDesignRepository designRepository, ILogger<DesignService> logger)
        {
            _designRepository = designRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Design>> GetAllDesignsAsync()
        {
            try
            {
                return await _designRepository.GetDesignList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all designs.");
                throw;
            }
        }

        public async Task<Design> GetDesignByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid design ID.");
                throw new ArgumentException("Design ID cannot be null or empty.");
            }

            try
            {
                return await _designRepository.GetDesignById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting design with id {id}.");
                throw;
            }
        }

        public async Task CreateDesignAsync(Design design)
        {
            if (design == null)
            {
                _logger.LogWarning("Invalid design data.");
                throw new ArgumentException("Design cannot be null.");
            }

            try
            {
                await _designRepository.CreateDesign(design);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a design.");
                throw;
            }
        }

        public async Task UpdateDesignAsync(string id, Design design)
        {
            if (string.IsNullOrEmpty(id) || design == null)
            {
                _logger.LogWarning("Invalid design ID or data.");
                throw new ArgumentException("Design ID and data cannot be null or empty.");
            }

            try
            {
                await _designRepository.UpdateDesign(design);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating design with id {id}.");
                throw;
            }
        }

        public async Task DeleteDesignAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid design ID.");
                throw new ArgumentException("Design ID cannot be null or empty.");
            }

            try
            {
                await _designRepository.DeleteDesign(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting design with id {id}.");
                throw;
            }
        }
    }
}
