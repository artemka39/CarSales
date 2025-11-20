using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.Models
{
    public class CarModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarBrandId { get; set; }
        public virtual CarBrand CarBrand { get; set; }
        public decimal BasePrice { get; set; }
    }
}
