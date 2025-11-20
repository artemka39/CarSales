using CarSales.Contracts.Dtos;
using CarSales.Contracts.Requests;
using CarSales.Contracts.Responses;
using CarSales.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<OrdersSummaryResponse> GetOrdersSummaryAsync(OrdersSummaryRequest request);
        Task CreateOrderAsync(Order order);
        Task CreateMultipleOrdersAsync(IEnumerable<Order> orders);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<byte[]> ExportOrdersSummaryAsync(OrdersSummaryRequest request);
        Task DeleteAllOrders();
    }
}
