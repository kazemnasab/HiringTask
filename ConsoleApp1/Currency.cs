using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Currency
    {
        public Currency(string sku)
        {
            Sku = sku;
            Rates = new List<CurrencyRate>();
        }

        public double Computed { get; set; } = 0;
        public string Sku { get; set; }
        public List<CurrencyRate> Rates { get; set; }
    }
}
