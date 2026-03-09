using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanHari2
{
    public class Pegawai
    {
        public string Id { get; set; }
        public string Nama { get; set; }
        public int GajiPokok { get; set; }
        public double Tunjangan { get; set; }

        public Pegawai(string id, string nama, int gajiPokok, double tunjangan)
        {
            Id = id;
            Nama = nama;
            GajiPokok = gajiPokok;
            Tunjangan = tunjangan;
        }

        public virtual decimal HitungGajiBersih()
        {
            return GajiPokok * (1 + (decimal)Tunjangan);
        }

        public void PrintSlipGaji()
        {
            Console.WriteLine("=".PadRight(29, '='));
            Console.WriteLine($"SLIP GAJI - {Nama}");
            Console.WriteLine($"Gaji Pokok\t: {GajiPokok, 10:N0}");
            Console.WriteLine($"Tunjangan\t: {GajiPokok*Tunjangan, 10:N0}");
            Console.WriteLine($"Total\t\t: {HitungGajiBersih(), 10:N0}");
            Console.WriteLine("=".PadRight(29, '='));

        }
    }
}