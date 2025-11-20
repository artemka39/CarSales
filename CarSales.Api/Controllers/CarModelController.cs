using CarSales.Application.Interfaces;
using CarSales.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarSales.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarModelController : ControllerBase
    {
        private readonly ICarModelService _carModelService;
        public CarModelController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarModels([FromQuery] int? brandId)
        {
            var carModels = await _carModelService.GetCarModelsAsync(brandId);
            return Ok(carModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarModelById(int id)
        {
            var carModel = await _carModelService.GetCarModelByIdAsync(id);
            if (carModel == null)
            {
                return NotFound();
            }
            return Ok(carModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCarModel([FromBody] CarModel carModel)
        {
            await _carModelService.CreateCarModelAsync(carModel);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCarModel([FromBody] CarModel carModel)
        {
            await _carModelService.UpdateCarModelAsync(carModel);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarModel(int id)
        {
            await _carModelService.DeleteCarModelAsync(id);
            return Ok();
        }
    }
}
