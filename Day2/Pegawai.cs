namespace Day2
{
    public class Pegawai(
        int id,
        string nama,
        decimal gajiPokok,
        decimal tunjanganPersen
    )
    {
        public int Id { get; set; } = id;
        public string Nama { get; set; } = nama;
        public decimal GajiPokok { get; set; } = gajiPokok;
        public decimal TunjanganPersen { get; set; } = tunjanganPersen;

        public decimal HitungGajiBersih()
        {
            return GajiPokok + (GajiPokok * TunjanganPersen);
        }

        public void PrintSlipGaji()
        {
            string result = $@"
==============================
SLIP GAJI - {Nama}
Gaji Pokok   : {GajiPokok,10:N0}
Tunjangan    : {GajiPokok * TunjanganPersen,10:N0}
Total        : {HitungGajiBersih(),10:N0}
==============================";

            Console.WriteLine(result);
        }
    }
}