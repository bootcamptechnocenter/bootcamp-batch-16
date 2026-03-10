using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soal1Hari2
{
    public class Pegawai
    {
        public string Id { get; set; }
        public string Nama { get; set; }
        public decimal Gaji { get; set; }
        public float TunjanganPersen { get; set; } = 0.1f;
        public decimal HitungGajiBersih()
        {
            return Gaji * (1 + (decimal)TunjanganPersen);
        }
        public void PrintSlipGaji()
        {
            decimal tunjangan = Gaji * (decimal)TunjanganPersen;
            decimal totalGaji = HitungGajiBersih();
            Console.WriteLine("===================================");
            Console.WriteLine($"SLIP GAJI - {Nama}");
            Console.WriteLine($"Gaji Pokok\t: {Gaji:C}");
            Console.WriteLine($"Tunjangan\t: {tunjangan:C}");
            Console.WriteLine($"Total Gaji\t: {totalGaji:C}");
            Console.WriteLine("===================================");
        }
    }
}