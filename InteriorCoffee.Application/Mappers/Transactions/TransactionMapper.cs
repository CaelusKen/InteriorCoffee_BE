using AutoMapper;
using InteriorCoffee.Application.DTOs.Transaction;
using InteriorCoffee.Application.Enums.Transaction;
using InteriorCoffee.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Mappers.Transactions
{
    public class TransactionMapper : Profile
    {
        public TransactionMapper()
        {
            CreateMap<CreateTransactionDTO, Transaction>()
                .ForMember(des => des._id, src => src.MapFrom(src => ObjectId.GenerateNewId().ToString()))
                .ForMember(des => des.UpdatedDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.TransactionDate, src => src.MapFrom(src => DateTime.UtcNow))
                .ForMember(des => des.Status, src => src.MapFrom(src => TransactionStatusEnum.PENDING.ToString()));
        }
    }
}
