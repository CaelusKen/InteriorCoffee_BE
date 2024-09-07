using InteriorCoffee.Domain.Models;
using InteriorCoffee.Infrastructure.Repositories.Base;
using InteriorCoffee.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Infrastructure.Repositories.Implements
{
    public class MerchantRepository : BaseRepository<MerchantRepository>, IMerchantRepository
    {
        private readonly IMongoCollection<Merchant> _merchants;

        public MerchantRepository(IOptions<MongoDBContext> setting, IMongoClient client) : base(setting, client)
        {
            _merchants = _database.GetCollection<Merchant>("Merchant");
        }

        public async Task<Merchant> GetMerchantByCode(string code)
        {
            return await _merchants.Find(m => m.MerchantCode.Equals(code)).FirstOrDefaultAsync();
        }
    }
}
