using InteriorCoffee.Application.DTOs.Merchant;
using InteriorCoffee.Application.DTOs.OrderBy;
using InteriorCoffee.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IMerchantService
    {
        Task<MerchantResponseDTO> GetMerchantsAsync(int? pageNo, int? pageSize, OrderBy orderBy, MerchantFilterDTO filter, string keyword);
        Task<Merchant> GetMerchantByIdAsync(string id);
        Task CreateMerchantAsync(CreateMerchantDTO merchant);
        Task UpdateMerchantAsync(string id, UpdateMerchantDTO merchant);
        Task DeleteMerchantAsync(string id);

        Task VerifyMerchantAsync(string id);
        Task<List<Merchant>> GetUnverifiedMerchants();
    }
}
