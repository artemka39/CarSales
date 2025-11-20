using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Application.Interfaces
{
    public interface IExcelService
    {
        byte[] CreateWorksheet(string title, Dictionary<string, List<string>> columns);
    }
}
