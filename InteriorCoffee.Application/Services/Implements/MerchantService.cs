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
            return await _merchantRepository.GetMerchantList();
        }

        public async Task<Merchant> GetMerchantByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Merchant ID cannot be null or empty.");
            return await _merchantRepository.GetMerchantById(id);
        }

        public async Task CreateMerchantAsync(Merchant merchant)
        {
            if (merchant == null) throw new ArgumentException("Merchant cannot be null.");
            await _merchantRepository.CreateMerchant(merchant);
        }

        public async Task UpdateMerchantAsync(string id, Merchant merchant)
        {
            if (string.IsNullOrEmpty(id) || merchant == null) throw new ArgumentException("Merchant ID and data cannot be null or empty.");
            await _merchantRepository.UpdateMerchant(merchant);
        }

        public async Task DeleteMerchantAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Merchant ID cannot be null or empty.");
            await _merchantRepository.DeleteMerchant(id);
        }
    }
}
