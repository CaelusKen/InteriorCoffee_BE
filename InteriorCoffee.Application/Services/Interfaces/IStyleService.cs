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
        public Task<List<Style>> GetAllStyles();
        public Task<Style> GetStyleById(string id);
        public Task CreateStyle(StyleDTO styleDTO);
        public Task UpdateStyle(string id, StyleDTO styleDTO);
        public Task DeleteStyle(string id);
    }
}
