using CarSales.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarSales.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestDataController : ControllerBase
    {
        private readonly ITestDataService _testDataGeneratorService;
        public TestDataController(ITestDataService testDataGeneratorService)
        {
            _testDataGeneratorService = testDataGeneratorService;
        }

        [HttpPost]
        [Route("generate-test-car-models")]
        public async Task<IActionResult> GenerateTestCarModels()
        {
            await _testDataGeneratorService.GenerateTestCarModelsAsync();
            return Ok();
        }

        [HttpPost]
        [Route("generate-test-orders")]
        public async Task<IActionResult> GenerateTestOrders([FromQuery] int yearsCount, [FromQuery] int ordersCount)
        {
            await _testDataGeneratorService.GenerateTestOrdersAsync(yearsCount, ordersCount);
            return Ok();
        }

        [HttpDelete]
        [Route("delete-all-orders")]
        public async Task<IActionResult> DeleteAllOrders()
        {
            await _testDataGeneratorService.DeleteAllOrders();
            return Ok();
        }
    }
}
