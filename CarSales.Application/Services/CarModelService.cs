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
    public class CarModelService : ICarModelService
    {
        private readonly DealerDbContext _dbContext;
        private readonly ILogger<CarModelService> _logger;
        public CarModelService(DealerDbContext dbContext, ILogger<CarModelService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<CarModel>> GetCarModelsAsync(int? brandId)
        {
            try
            {
                var models = await _dbContext.CarModels.Where(m => !brandId.HasValue || m.CarBrandId == brandId.Value).ToListAsync();
                return models;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось загрузить модели.");
                return Enumerable.Empty<CarModel>();
            }
        }

        public async Task<CarModel> GetCarModelByIdAsync(int id)
        {
            try
            {
                var model = await _dbContext.CarModels.FindAsync(id);
                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Не удалось загрузить модель с ID {id}.");
                return null;
            }
        }

        public async Task CreateCarModelAsync(CarModel carModel)
        {
            try
            {
                var existingModel = await _dbContext.CarModels
                    .FirstOrDefaultAsync(m => m.Name.ToLower() == carModel.Name.ToLower());
                if (existingModel == null)
                {
                    await _dbContext.CarModels.AddAsync(carModel);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании модели.");
            }
        }

        public async Task UpdateCarModelAsync(CarModel carModel)
        {
            try
            {
                var existingModel = await _dbContext.CarModels.FindAsync(carModel.Id);
                if (existingModel != null)
                {
                    existingModel.Name = carModel.Name;
                    existingModel.CarBrandId = carModel.CarBrandId;
                    _dbContext.CarModels.Update(existingModel);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении модели.");
            }
        }

        public async Task DeleteCarModelAsync(int id)
        {
            try
            {
                var existingModel = await _dbContext.CarModels.FindAsync(id);
                if (existingModel != null)
                {
                    var hasAssociatedCars = await _dbContext.Cars.AnyAsync(c => c.CarModelId == id);
                    if (!hasAssociatedCars)
                    {
                        _dbContext.CarModels.Remove(existingModel);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new InvalidOperationException("Cannot delete car model with associated cars.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении модели.");
            }
        }
    }
}