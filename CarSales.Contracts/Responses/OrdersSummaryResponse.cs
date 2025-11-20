using CarSales.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Contracts.Responses
{
    public class OrdersSummaryResponse
    {
        public List<int> Years { get; set; } = new();
        public List<OrdersSummaryDto> OrdersSummaries { get; set; } = new();
    }
}
