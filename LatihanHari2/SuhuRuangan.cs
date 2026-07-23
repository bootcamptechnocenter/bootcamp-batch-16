using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanHari2.SuhuRuangan
{
    public class SuhuRuangan
    {
        private decimal _celcius {get; set;} = 0m;

    public SuhuRuangan(decimal celcius)
    {
        Celcius = celcius;  // pakai setter yang ada validasi
    }

    public decimal Celcius
    {
        get => _celcius;
        set
        {
            if (value < -273.15m)
                throw new ArgumentException("Suhu di bawah absolute zero (-273.15°C)!");
            _celcius = value;
        }
    }

    public decimal Fahrenheit => (_celcius * 9m / 5m) + 32m;
    public decimal Kelvin => _celcius + 273.15m;

    public string Deskripsi()
    {
        if (_celcius < 20) return "Dingin";
        if (_celcius <= 26) return "Normal";
        return "Panas";
    }
    }
}