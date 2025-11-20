using CarSales.Application.Interfaces;
using CarSales.Domain.Models;
using CarSales.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Application.Services
{
    public class CarService : ICarService
    {
        private readonly DealerDbContext _dbContext;
        private readonly ILogger<CarService> _logger;
        public CarService(DealerDbContext dbContext, ILogger<CarService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Car>> GetCarsAsync()
        {
            try
            {
                var cars = await _dbContext.Cars
                    .Include(c => c.CarModel)
                    .ThenInclude(cm => cm.CarBrand)
                    .ToListAsync();
                return cars;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось загрузить автомобили.");
                return Enumerable.Empty<Car>();
            }
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            try
            {
                var car = await _dbContext.Cars
                    .Include(c => c.CarModel)
                    .ThenInclude(cm => cm.CarBrand)
                    .FirstOrDefaultAsync(c => c.Id == id);
                return car;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Не удалось загрузить автомобиль с ID {id}.");
                return null;
            }
        }

        public async Task CreateCarAsync(Car car)
        {
            try
            {
                await _dbContext.Cars.AddAsync(car);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось создать автомобиль.");
            }
        }

        public async Task CreateMultipleCarsAsync(IEnumerable<Car> cars)
        {
            try
            {
                await _dbContext.Cars.AddRangeAsync(cars);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось создать несколько автомобилей.");
            }
        }

        public async Task UpdateCarAsync(Car car)
        {
            try
            {
                _dbContext.Cars.Update(car);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось обновить автомобиль.");
            }
        }

        public async Task DeleteCarAsync(int id)
        {
            try
            {
                var car = await _dbContext.Cars.FindAsync(id);
                _dbContext.Cars.Remove(car);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Не удалось удалить автомобиль с ID {id}.");
            }
        }
    }
}
