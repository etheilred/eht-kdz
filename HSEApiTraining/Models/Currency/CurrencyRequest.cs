using System;

namespace HSEApiTraining.Models.Currency
{
    public class CurrencyRequest
    {
        public string Symbol { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}