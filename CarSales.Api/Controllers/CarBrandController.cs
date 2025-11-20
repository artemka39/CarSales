using Microsoft.AspNetCore.Mvc;
using CarSales.Application.Interfaces;
using System.Threading.Tasks;
using CarSales.Domain.Models;

namespace CarSales.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarBrandController : ControllerBase
    {
        private readonly ICarBrandService _carBrandService;
        public CarBrandController(ICarBrandService carBrandService)
        {
            _carBrandService = carBrandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCarBrands()
        {
            var carBrands = await _carBrandService.GetCarBrandsAsync();
            return Ok(carBrands);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarBrandById(int id)
        {
            var carBrand = await _carBrandService.GetCarBrandByIdAsync(id);
            if (carBrand == null)
            {
                return NotFound();
            }
            return Ok(carBrand);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCarBrand([FromBody] CarBrand carBrand)
        {
            await _carBrandService.CreateCarBrandAsync(carBrand);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCarBrand([FromBody] CarBrand carBrand)
        {
            await _carBrandService.UpdateCarBrandAsync(carBrand);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarBrand(int id)
        {
            await _carBrandService.DeleteCarBrandAsync(id);
            return Ok();
        }
    }
}
