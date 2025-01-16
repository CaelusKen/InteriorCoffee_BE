using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorCoffee.Application.Utils
{
    public static class CurrencyExchangeUtil
    {
        //Exchange rate: 1 USD ~~ 25274,99 VND

        public static double USDtoVND(double amount)
        {
            return RoundUp(amount * 25274.99, 0);
        }

        public static double VNDtoUSD(double amount)
        {
            return RoundUp(amount / 25274.99, 2);
        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
    }
}
