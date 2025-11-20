using CarSales.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSales.Domain.Constants
{
    public static class ColorNameDictionary
    {
        private static readonly Dictionary<ColorsEnum, string> ColorNames = new()
        {
            { ColorsEnum.Red, "Красный" },
            { ColorsEnum.Blue, "Синий" },
            { ColorsEnum.Green, "Зеленый" },
            { ColorsEnum.Black, "Черный" },
            { ColorsEnum.White, "Белый" },
            { ColorsEnum.Silver, "Серебристый" },
            { ColorsEnum.Yellow, "Желтый" },
            { ColorsEnum.Orange, "Оранжевый" },
            { ColorsEnum.Purple, "Фиолетовый" },
            { ColorsEnum.Brown, "Коричневый" },
            { ColorsEnum.Gray, "Серый" }
        };

        public static string GetColorName(ColorsEnum colorKey) =>
            ColorNames.TryGetValue(colorKey, out var colorName) ? colorName : colorKey.ToString();
    }
}
