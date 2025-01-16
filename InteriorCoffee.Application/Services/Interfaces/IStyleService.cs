using InteriorCoffee.Application.DTOs.Style;
using InteriorCoffee.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Services.Interfaces
{
    public interface IStyleService
    {
        Task<(List<Style>, int, int, int, int)> GetStylesAsync(int? pageNo, int? pageSize);
        Task<Style> GetStyleById(string id);
        Task CreateStyle(StyleDTO styleDTO);
        Task UpdateStyle(string id, StyleDTO styleDTO);
        Task DeleteStyle(string id);
    }
}
