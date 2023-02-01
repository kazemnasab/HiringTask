using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class CurrencyRate
    {
        public CurrencyRate(Currency currency, double rate)
        {
            Currency = currency;
            Rate = rate;
        }
        public Currency Currency { get; set; }
        public double Rate { get; set; }
    }
}
