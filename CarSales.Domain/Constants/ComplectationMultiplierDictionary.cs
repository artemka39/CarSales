using CarSales.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.Constants
{
    public static class ComplectationMultiplierDictionary
    {
        private static readonly Dictionary<ComplectationEnum, decimal> ComplectationMultipliers = new()
        {
            { ComplectationEnum.Base, 1.0m },
            { ComplectationEnum.Casual, 1.2m },
            { ComplectationEnum.Performance, 1.5m },
            { ComplectationEnum.Luxury, 2.0m }
        };

        public static decimal GetMultiplier(ComplectationEnum complectation) => 
            ComplectationMultipliers.TryGetValue(complectation, out var multiplier) ? multiplier : 1.0m;
    }
}
