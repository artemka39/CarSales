using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Contracts.Requests
{
    public class OrdersSummaryRequest
    {
        public int? Year { get; set; }
        public int[] ModelsIds { get; set; } = [];
    }
}
