using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    interface ICurrencyConverter
    {
        bool Lock { set; get; }
        Hashtable Currencies { set; get; }
        /// <summary>
        /// Clears any prior configuration.
        /// </summary>
        void ClearConfiguration();
        /// <summary>
        /// Updates the configuration. Rates are inserted or replaced internally.
        /// </summary>
        void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates);
        /// <summary>
        /// Converts the specified amount to the desired currency.
        /// </summary>
        double Convert(string fromCurrency, string toCurrency, double amount);
    }
    internal class CurrencyConverter: ICurrencyConverter
    {

        public bool Lock { get; set; } = false;
        public Hashtable Currencies { get; set; } = new Hashtable();

        /// <summary>
        /// Clears any prior configuration.
        /// </summary>
        public void ClearConfiguration() {
            this.Currencies.Clear();
            this.Lock = false;
        }

        /// <summary>
        /// Updates the configuration. Rates are inserted or replaced internally.
        /// </summary>
        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates) {
            ClearConfiguration();
            Currency fromCurrency, toCurrency;
            foreach (var conversionRate in conversionRates)
            {
                if (Currencies[conversionRate.Item1] == null)
                    Currencies.Add(conversionRate.Item1, new Currency(conversionRate.Item1)); //adding a key/value using the Add() method
                if (Currencies[conversionRate.Item2] == null)
                    Currencies.Add(conversionRate.Item2, new Currency(conversionRate.Item2)); //adding a key/value using the Add() method
                fromCurrency = Currencies[conversionRate.Item1] as Currency;
                toCurrency = Currencies[conversionRate.Item2] as Currency;
                fromCurrency.Rates.Add(new CurrencyRate(toCurrency, conversionRate.Item3));
                toCurrency.Rates.Add(new CurrencyRate(fromCurrency, 1 / conversionRate.Item3));
            }
        }

        /// <summary>
        /// Converts the specified amount to the desired currency.
        /// </summary>
        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            while (this.Lock) ;
            this.Lock = true;
            foreach (DictionaryEntry currency in Currencies)
                (currency.Value as Currency).Computed = 0;
            var from = Currencies[fromCurrency] as Currency;
            var to = Currencies[toCurrency] as Currency;
            if (from == null && to == null)
            {
                this.Lock = false;
                return double.NaN;
            }
            if (from == to)
            {
                this.Lock = false;
                return 1;
            }
            Queue<Currency> queue = new Queue<Currency>();
            from.Computed = 1;
            queue.Enqueue(from);
            Currency current;
            while (true)
            {
                if (queue.Count == 0)
                {
                    this.Lock = false;
                    return double.NaN;
                }
                current = queue.Dequeue();
                foreach (var rate in current.Rates.Where(m => m.Currency.Computed == 0))
                {
                    rate.Currency.Computed = current.Computed * rate.Rate;
                    if (rate.Currency.Sku == to.Sku)
                    {
                        if (current.Sku != from.Sku)
                        {
                            from.Rates.Add(new CurrencyRate(current, current.Computed));
                            to.Rates.Add(new CurrencyRate(from, 1 / current.Computed));
                        }
                        this.Lock = false;
                        return rate.Currency.Computed * amount;
                    }
                    queue.Enqueue(rate.Currency);
                }
            }
            this.Lock = false;
            return double.NaN;
        }
    }
}
