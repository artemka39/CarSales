using CarSales.Application.Interfaces;
using CarSales.Contracts;
using CarSales.Contracts.Dtos;
using CarSales.Contracts.Requests;
using CarSales.Contracts.Responses;
using CarSales.Domain.Constants;
using CarSales.Domain.Models;
using CarSales.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly DealerDbContext _dbContext;
        private readonly IExcelService _excelService;
        private readonly ILogger<OrderService> _logger;
        public OrderService(
            DealerDbContext dbContext, 
            IExcelService excelService,
            ILogger<OrderService> logger
            )
        {
            _dbContext = dbContext;
            _excelService = excelService;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            try
            {
                var orders = await _dbContext.Orders
                    .Include(o => o.Car)
                    .ThenInclude(c => c.CarModel)
                    .ThenInclude(m => m.CarBrand)
                    .Select(o => new OrderDto
                    {
                        Id = o.Id,
                        OrderDate = o.OrderDate,
                        Brand = o.Car.CarModel.CarBrand.Name,
                        Model = o.Car.CarModel.Name,
                        Color = ColorNameDictionary.GetColorName(o.Car.Color),
                        Complectation = o.Car.Complectation.ToString(),
                        Price = o.Car.Price
                    })
                    .ToListAsync();

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось получить список заказов.");
                return new List<OrderDto>();
            }
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            try
            {
                var rawOrder = await _dbContext.Orders
                    .Include(o => o.Car)
                    .ThenInclude(c => c.CarModel)
                    .ThenInclude(m => m.CarBrand)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (rawOrder == null)
                {
                    return null;
                }

                return new OrderDto
                {
                    Id = rawOrder.Id,
                    OrderDate = rawOrder.OrderDate,
                    Brand = rawOrder.Car.CarModel.CarBrand.Name,
                    Model = rawOrder.Car.CarModel.Name,
                    Color = ColorNameDictionary.GetColorName(rawOrder.Car.Color),
                    Complectation = rawOrder.Car.Complectation.ToString(),
                    Price = rawOrder.Car.Price
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Не удалось получить заказ с id={id}.");
                return null;
            }
        }

        public async Task<OrdersSummaryResponse> GetOrdersSummaryAsync(OrdersSummaryRequest request)
        {
            try
            {
                var query = _dbContext.Orders.AsQueryable();

                if (request.Year.HasValue)
                {
                    query = query.Where(o => o.OrderDate.Year == request.Year.Value);
                }

                if (request.ModelsIds.Length > 0)
                {
                    query = query.Where(o => request.ModelsIds.Contains(o.Car.CarModelId));
                }

                var summaryRaw = await query
                    .Select(o => new
                    {
                        o.OrderDate,
                        Brand = o.Car.CarModel.CarBrand.Name,
                        Model = o.Car.CarModel.Name,
                        o.Car.Price
                    })
                    .ToListAsync();

                var years = summaryRaw
                    .Select(o => o.OrderDate.Year)
                    .Distinct()
                    .OrderByDescending(y => y)
                    .ToList();

                var summaries = summaryRaw
                    .GroupBy(x => new { x.Brand, x.Model })
                    .Select(g => new OrdersSummaryDto
                    {
                        Model = $"{g.Key.Brand} {g.Key.Model}",
                        MonthlySales = g
                            .GroupBy(x => x.OrderDate.Month)
                            .OrderBy(x => x.Key)
                            .ToDictionary(
                                mg => mg.Key,
                                mg => mg.Sum(x => x.Price))
                    })
                    .OrderBy(r => r.Model)
                    .ToList();

                var response = new OrdersSummaryResponse
                {
                    Years = years,
                    OrdersSummaries = summaries
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось получить сводку по заказам.");
                return new OrdersSummaryResponse
                {
                    Years = new List<int>(),
                    OrdersSummaries = new List<OrdersSummaryDto>()
                };
            }
        }

        public async Task CreateOrderAsync(Order order)
        {
            try
            {
                await _dbContext.Orders.AddAsync(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось создать заказ.");
            }
        }

        public async Task CreateMultipleOrdersAsync(IEnumerable<Order> orders)
        {
            try
            {
                await _dbContext.Orders.AddRangeAsync(orders);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось создать несколько заказов.");
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            try
            {
                _dbContext.Orders.Update(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Не удалось обновить заказ с id={order.Id}.");
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            try
            {
                var order = await _dbContext.Orders.FindAsync(id);
                if (order != null)
                {
                    _dbContext.Orders.Remove(order);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Не удалось удалить заказ с id={id}.");
            }
        }

        public async Task<byte[]> ExportOrdersSummaryAsync(OrdersSummaryRequest request)
        {
            var ordersSummaries = await GetOrdersSummaryAsync(request);
            var columns = new Dictionary<string, List<string>>();
            foreach (var summary in ordersSummaries.OrdersSummaries)
            {
                if (!columns.ContainsKey("Модель"))
                {
                    columns["Модель"] = new List<string>();
                }
                columns["Модель"].Add(summary.Model);
                for (int month = 1; month <= 12; month++)
                {
                    var hasSales = summary.MonthlySales.TryGetValue(month, out var value);
                    var monthName = new DateTime(1, month, 1)
                        .ToString("MMMM", new CultureInfo("ru-RU"));
                    if (!columns.ContainsKey(monthName))
                    {
                        columns[monthName] = new List<string>();
                    }
                    columns[monthName].Add(value.ToString());
                }
            }
            var title = $"Отчет по продажам за {request.Year}";
            var worksheet = await Task.Run(() => _excelService.CreateWorksheet(title, columns));
            return worksheet;
        }

        public async Task DeleteAllOrders()
        {
            try
            {
                _dbContext.Orders.RemoveRange(_dbContext.Orders);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось удалить все заказы.");
            }
        }
    }
}
