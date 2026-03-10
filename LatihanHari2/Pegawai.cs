using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanHari2
{
    public class Pegawai
    {
        public string? Id { get; set; }
        public string? Nama { get; set; }
        public decimal GajiPokok { get; set; } 
        public decimal TunjanganPersen { get; set; }

        public decimal HitungGajiBersih()
        {

            return GajiPokok * (1 + TunjanganPersen);
        }

        public void PrintSlipGaji()
        {
            Console.WriteLine($"SLIP GAJI - {Nama}");
            Console.WriteLine($"Gaji Pokok\t: {GajiPokok:N0}");
            Console.WriteLine($"Tunjangan\t: {GajiPokok * TunjanganPersen:N0}");
            Console.WriteLine($"Total\t\t: {HitungGajiBersih():N0}");
        }
    }
}