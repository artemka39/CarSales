using CarSales.Application.Interfaces;
using CarSales.Domain.Models;
using CarSales.Domain.Enums;
using CarSales.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarSales.Domain.Constants;

namespace CarSales.Application.Services
{
    public class TestDataService : ITestDataService
    {
        private readonly ICarBrandService _carBrandService;
        private readonly ICarModelService _carModelService;
        private readonly ICarService _carService;
        private readonly IOrderService _orderService;

        public TestDataService(
            ICarBrandService carBrandService,
            ICarModelService carModelService,
            ICarService carService,
            IOrderService orderService)
        {
            _carBrandService = carBrandService;
            _carModelService = carModelService;
            _carService = carService;
            _orderService = orderService;
        }

        public async Task GenerateTestCarModelsAsync()
        {
            var brandsToCreate = new List<CarBrand>
            {
                new CarBrand { Name = "Toyota" },
                new CarBrand { Name = "Ford" },
                new CarBrand { Name = "BMW" },
                new CarBrand { Name = "Mercedes-Benz" },
                new CarBrand { Name = "Audi" },
                new CarBrand { Name = "Volkswagen" },
                new CarBrand { Name = "Hyundai" },
            };
            foreach (var brand in brandsToCreate)
            {
                await _carBrandService.CreateCarBrandAsync(brand);
            }
            var brands = (await _carBrandService.GetCarBrandsAsync()).ToDictionary(k => k.Name, v => v.Id);
            var models = new List<CarModel>
            {
                new CarModel { Name = "Corolla", CarBrandId = brands["Toyota"], BasePrice = 2200000 },
                new CarModel { Name = "Camry", CarBrandId = brands["Toyota"], BasePrice = 2800000 },
                new CarModel { Name = "RAV4", CarBrandId = brands["Toyota"], BasePrice = 3000000 },

                new CarModel { Name = "Focus", CarBrandId = brands["Ford"], BasePrice = 1800000 },
                new CarModel { Name = "Mondeo", CarBrandId = brands["Ford"], BasePrice = 2500000 },
                new CarModel { Name = "Mustang", CarBrandId = brands["Ford"], BasePrice = 4500000 },

                new CarModel { Name = "3 Series", CarBrandId = brands["BMW"], BasePrice = 2500000 },
                new CarModel { Name = "5 Series", CarBrandId = brands["BMW"], BasePrice = 3100000 },
                new CarModel { Name = "X5", CarBrandId = brands["BMW"], BasePrice = 3500000 },

                new CarModel { Name = "C-Class", CarBrandId = brands["Mercedes-Benz"], BasePrice = 2700000 },
                new CarModel { Name = "E-Class", CarBrandId = brands["Mercedes-Benz"], BasePrice = 3300000},
                new CarModel { Name = "GLC", CarBrandId = brands["Mercedes-Benz"], BasePrice = 3600000},

                new CarModel { Name = "A4", CarBrandId = brands["Audi"] , BasePrice = 2400000},
                new CarModel { Name = "A6", CarBrandId = brands["Audi"] , BasePrice = 3000000},
                new CarModel { Name = "Q5", CarBrandId = brands["Audi"] , BasePrice = 3300000},

                new CarModel { Name = "Golf", CarBrandId = brands["Volkswagen"] , BasePrice = 1900000},
                new CarModel { Name = "Passat", CarBrandId = brands["Volkswagen"] , BasePrice = 2500000},
                new CarModel { Name = "Tiguan", CarBrandId = brands["Volkswagen"] , BasePrice = 2800000},

                new CarModel { Name = "Elantra", CarBrandId = brands["Hyundai"] , BasePrice = 1700000},
                new CarModel { Name = "Sonata", CarBrandId = brands["Hyundai"] , BasePrice = 2300000},
                new CarModel { Name = "Tucson", CarBrandId = brands["Hyundai"] , BasePrice = 2700000},
            };
            foreach (var model in models)
            {
                await _carModelService.CreateCarModelAsync(model);
            }
        }

        public async Task GenerateTestOrdersAsync(int yearsCount, int ordersCount)
        {
            var random = new Random();
            var models = (await _carModelService.GetCarModelsAsync(null)).ToList();
            if (models.Count == 0)
            {
                await GenerateTestCarModelsAsync();
                models = (await _carModelService.GetCarModelsAsync(null)).ToList();
            }
            var cars = new List<Car>();
            var orders = new List<Order>();
            for (int i = 0; i < ordersCount; i++)
            {
                var model = models[random.Next(models.Count)];
                cars.Add(new Car
                {
                    CarModelId = model.Id,
                    Color = (ColorsEnum)random.Next(1, Enum.GetValues(typeof(ColorsEnum)).Length),
                    Complectation = (ComplectationEnum)random.Next(1, Enum.GetValues(typeof(ComplectationEnum)).Length),
                    Price = ComplectationMultiplierDictionary.GetMultiplier((ComplectationEnum)random.Next(1, Enum.GetValues(typeof(ComplectationEnum)).Length)) * model.BasePrice
                });
            }
            await _carService.CreateMultipleCarsAsync(cars);
            var startDate = DateTime.Now.AddYears(-yearsCount);
            var totalDays = (DateTime.Now - startDate).Days;

            foreach (var car in cars)
            {
                var orderDate = startDate.AddDays(random.Next(totalDays));
                orders.Add(new Order
                {
                    Car = car,
                    OrderDate = orderDate
                });
            }
            await _orderService.CreateMultipleOrdersAsync(orders);
        }

        public async Task DeleteAllOrders()
        {
            await _orderService.DeleteAllOrders();
        }
    }
}