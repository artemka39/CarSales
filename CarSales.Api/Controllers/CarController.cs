using CarSales.Application.Interfaces;
using CarSales.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarSales.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _carService.GetCarsAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar([FromBody] Car car)
        {
            await _carService.CreateCarAsync(car);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCar([FromBody] Car car)
        {
            await _carService.UpdateCarAsync(car);
            return Ok();
        }
    }
}
