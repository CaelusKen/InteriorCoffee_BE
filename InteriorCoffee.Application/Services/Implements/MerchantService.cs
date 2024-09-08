using InteriorCoffee.Application.Services.Interfaces;
using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InteriorCoffee.Application.Services.Implements
{
    public class MerchantService : IMerchantService
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly ILogger<MerchantService> _logger;

        public MerchantService(IMerchantRepository merchantRepository, ILogger<MerchantService> logger)
        {
            _merchantRepository = merchantRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Merchant>> GetAllMerchantsAsync()
        {
            try
            {
                return await _merchantRepository.GetMerchantList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all merchants.");
                throw;
            }
        }

        public async Task<Merchant> GetMerchantByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid merchant ID.");
                throw new ArgumentException("Merchant ID cannot be null or empty.");
            }

            try
            {
                return await _merchantRepository.GetMerchantById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while getting merchant with id {id}.");
                throw;
            }
        }

        public async Task CreateMerchantAsync(Merchant merchant)
        {
            if (merchant == null)
            {
                _logger.LogWarning("Invalid merchant data.");
                throw new ArgumentException("Merchant cannot be null.");
            }

            try
            {
                await _merchantRepository.CreateMerchant(merchant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a merchant.");
                throw;
            }
        }

        public async Task UpdateMerchantAsync(string id, Merchant merchant)
        {
            if (string.IsNullOrEmpty(id) || merchant == null)
            {
                _logger.LogWarning("Invalid merchant ID or data.");
                throw new ArgumentException("Merchant ID and data cannot be null or empty.");
            }

            try
            {
                await _merchantRepository.UpdateMerchant(merchant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating merchant with id {id}.");
                throw;
            }
        }

        public async Task DeleteMerchantAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogWarning("Invalid merchant ID.");
                throw new ArgumentException("Merchant ID cannot be null or empty.");
            }

            try
            {
                await _merchantRepository.DeleteMerchant(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting merchant with id {id}.");
                throw;
            }
        }
    }
}
