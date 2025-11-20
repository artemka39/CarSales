using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Contracts.Dtos
{
    public class OrdersSummaryDto
    {
        public string Model { get; set; }
        public Dictionary<int, decimal> MonthlySales { get; set; } = new();
    }
}
