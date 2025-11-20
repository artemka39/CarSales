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
    public class CarBrandService : ICarBrandService
    {
        private readonly DealerDbContext _dbContext;
        private readonly ILogger<CarBrandService> _logger;
        public CarBrandService(DealerDbContext dbContext, ILogger<CarBrandService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<CarBrand>> GetCarBrandsAsync()
        {
            try
            {
                var brands = await _dbContext.CarBrands.ToListAsync();
                return brands;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось загрузить бренды.");
                return Enumerable.Empty<CarBrand>();
            }
        }

        public async Task<CarBrand> GetCarBrandByIdAsync(int id)
        {
            try
            {
                var brand = await _dbContext.CarBrands.FindAsync(id);
                return brand;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Не удалось загрузить бренд с ID {id}.");
                return null;
            }
        }

        public async Task CreateCarBrandAsync(CarBrand carBrand)
        {
            try
            {
                var existingBrand = await _dbContext.CarBrands
                    .FirstOrDefaultAsync(b => b.Name.ToLower() == carBrand.Name.ToLower());
                if (existingBrand == null)
                {
                    await _dbContext.CarBrands.AddAsync(carBrand);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании бренда.");
            }
        }

        public async Task UpdateCarBrandAsync(CarBrand carBrand)
        {
            try
            {
                var existingBrand = await _dbContext.CarBrands.FindAsync(carBrand.Id);
                if (existingBrand != null)
                {
                    existingBrand.Name = carBrand.Name;
                    _dbContext.CarBrands.Update(existingBrand);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении бренда.");
            }
        }

        public async Task DeleteCarBrandAsync(int id)
        {
            try
            {
                var brand = await _dbContext.CarBrands.FindAsync(id);
                if (brand != null)
                {
                    _dbContext.CarBrands.Remove(brand);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении бренда.");
            }
        }
    }
}