using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Application.Interfaces
{
    public interface ITestDataService
    {
        Task GenerateTestCarModelsAsync();
        Task GenerateTestOrdersAsync(int yearsCount, int ordersCount);
        Task DeleteAllOrders();
    }
}
