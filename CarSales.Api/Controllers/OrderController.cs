using CarSales.Application.Interfaces;
using CarSales.Contracts.Dtos;
using CarSales.Contracts.Requests;
using CarSales.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarSales.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost("summary")]
        public async Task<IActionResult> GetOrdersSummary([FromBody] OrdersSummaryRequest request)
        {
            var summary = await _orderService.GetOrdersSummaryAsync(request);
            return Ok(summary);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            await _orderService.CreateOrderAsync(order);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] Order order)
        {
            await _orderService.UpdateOrderAsync(order);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok();
        }

        [HttpPost("export-summary")]
        public async Task<IActionResult> ExportOrdersSummary([FromBody] OrdersSummaryRequest request)
        {
            var fileBytes = await _orderService.ExportOrdersSummaryAsync(request);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrdersSummary.xlsx");
        }
    }
}
