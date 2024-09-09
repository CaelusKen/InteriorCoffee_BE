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
            return await _designRepository.GetDesignList();
        }

        public async Task<Design> GetDesignByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Design ID cannot be null or empty.");
            return await _designRepository.GetDesignById(id);
        }

        public async Task CreateDesignAsync(Design design)
        {
            if (design == null) throw new ArgumentException("Design cannot be null.");
            await _designRepository.CreateDesign(design);
        }

        public async Task UpdateDesignAsync(string id, Design design)
        {
            if (string.IsNullOrEmpty(id) || design == null) throw new ArgumentException("Design ID and data cannot be null or empty.");
            await _designRepository.UpdateDesign(design);
        }

        public async Task DeleteDesignAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Design ID cannot be null or empty.");
            await _designRepository.DeleteDesign(id);
        }
    }
}
