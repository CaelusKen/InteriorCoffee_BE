using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IMerchantService
    {
        Task<IEnumerable<Merchant>> GetAllMerchantsAsync();
        Task<Merchant> GetMerchantByIdAsync(string id);
        Task CreateMerchantAsync(Merchant merchant);
        Task UpdateMerchantAsync(string id, Merchant merchant);
        Task DeleteMerchantAsync(string id);
    }
}
