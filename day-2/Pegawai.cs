using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace day_2
{
    public class Pegawai
    {
        public string? Id { get; set; }
        public string? Nama { get; set; }
        public int? GajiPokok { get; set; }
        public double? TunjanganPersen { get; set; }

        public double? HitungGajiBersih()
        {
            return GajiPokok * (1 + TunjanganPersen);
        }

        public void PrintSlipGaji()
        {
            var culture = new CultureInfo("id-ID");
            var gajiPokok = GajiPokok ?? 0;
            var tunjangan = gajiPokok * (TunjanganPersen ?? 0);
            var gajiBersih = HitungGajiBersih() ?? 0;

            Console.WriteLine("=============================");
            Console.WriteLine($"SLIP GAJI - {Nama}");
            Console.WriteLine($"Gaji Pokok : {gajiPokok.ToString("C0", culture)}");
            Console.WriteLine($"Tunjangan  : {tunjangan.ToString("C0", culture)}");
            Console.WriteLine($"Gaji Bersih: {gajiBersih.ToString("C0", culture)}");
            Console.WriteLine("=============================");
        }
    }
}