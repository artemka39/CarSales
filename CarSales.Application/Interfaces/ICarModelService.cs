using CarSales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Application.Interfaces
{
    public interface ICarModelService
    {
        Task<IEnumerable<CarModel>> GetCarModelsAsync(int? brandId);
        Task<CarModel> GetCarModelByIdAsync(int id);
        Task CreateCarModelAsync(CarModel carModel);
        Task UpdateCarModelAsync(CarModel carModel);
        Task DeleteCarModelAsync(int id);
    }
}
