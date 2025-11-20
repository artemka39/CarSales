using CarSales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Application.Interfaces
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetCarsAsync();
        Task<Car> GetCarByIdAsync(int id);
        Task CreateCarAsync(Car car);
        Task CreateMultipleCarsAsync(IEnumerable<Car> cars);
        Task UpdateCarAsync(Car car);
        Task DeleteCarAsync(int id);
    }
}
