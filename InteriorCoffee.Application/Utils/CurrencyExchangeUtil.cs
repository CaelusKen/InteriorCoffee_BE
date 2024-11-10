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
            return amount * 25274.99;
        }

        public static double VNDtoUSD(double amount)
        {
            return amount / 25274.99;
        }

    }
}
