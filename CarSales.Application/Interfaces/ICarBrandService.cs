using CarSales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Application.Interfaces
{
    public interface ICarBrandService
    {
        Task<IEnumerable<CarBrand>> GetCarBrandsAsync();
        Task<CarBrand> GetCarBrandByIdAsync(int id);
        Task CreateCarBrandAsync(CarBrand carBrand);
        Task UpdateCarBrandAsync(CarBrand carBrand);
        Task DeleteCarBrandAsync(int id);
    }
}
