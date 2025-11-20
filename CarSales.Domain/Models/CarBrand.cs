using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.Models
{
    public class CarBrand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();
    }
}
