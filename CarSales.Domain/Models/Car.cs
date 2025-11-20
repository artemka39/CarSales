using CarSales.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.Models
{
    public class Car
    {
        public int Id { get; set; }
        public int CarModelId { get; set; }
        public virtual CarModel CarModel { get; set; }
        public ColorsEnum Color { get; set; }
        public ComplectationEnum Complectation { get; set; }
        public decimal Price { get; set; }
    }
}
