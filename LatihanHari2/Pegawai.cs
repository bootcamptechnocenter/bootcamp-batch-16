using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace LatihanHari2
{
    public class Pegawai
    {
        public string Id {get; set;}
        public string Nama {get; set;}
        public decimal GajiPokok {get; set;}
        public decimal TunjanganPersen {get; set;}

        public decimal HitungGajiBersih() {
            decimal GajiBersih = GajiPokok * ((1 + TunjanganPersen)/100);

            return GajiBersih;
        }

        public void PrintSlipGaji() {
            decimal Tunjangan = GajiPokok * (TunjanganPersen / 100);
            decimal Total = GajiPokok + Tunjangan;

            Console.WriteLine("=======================");
            Console.WriteLine($"Slip Gaji - {Nama}");
            Console.WriteLine($"Gaji Pokok  : {GajiPokok:C}");
            Console.WriteLine($"Tunjangan   : {Tunjangan:C}");
            Console.WriteLine($"Total       : {Total:C}");
            Console.WriteLine("========================");
        }
    }
}