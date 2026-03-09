// string namaPerusahaan = "PT. ABC";
// int tahunBerdiri = 1990;
// decimal omzet = 1_000_000_000m;
// bool  aktif = true;
// string? catatan = "Ga usah pake sambel";

// Console.WriteLine($"Nama Perusahaan: {namaPerusahaan}");
// Console.WriteLine($"Tahun Berdiri: {tahunBerdiri}");
// Console.WriteLine($"Omzet: {omzet:C}");
// Console.WriteLine($"Aktif: {aktif}");
// Console.WriteLine($"Catatan: {catatan}");

// int a = 10;
// int b = 20;

// Console.WriteLine($"Hasil Penjumlahan: {a + b}");
// Console.WriteLine($"Hasil Pengurangan: {a - b}");
// Console.WriteLine($"Hasil Perkalian: {a * b}");
// Console.WriteLine($"Hasil Pembagian: {a / b}");
// Console.WriteLine($"Hasile Modulus: {a % b}");

// Console.WriteLine($"Apakah a lebih besar dari b? {a > b}");
// Console.WriteLine($"Apakah a lebih kecil dari b? {a < b}");
// Console.WriteLine($"Apakah a sama dengan b? {a == b}");
// Console.WriteLine($"Apakah a tidak sama dengan b? {a != b}");

// string numberString = "123";
// int number = int.Parse(numberString);
// int number2 = 201;
// Console.WriteLine($"Hasil konversi string ke int: {number}");
// Console.WriteLine($"Hasil penjumlahan : {number + number2}");

// using System;
// using System.Globalization;

// public class Program
// {
//     public static void Main()
//     {
//         Console.Write("Masukkan panjang : ");
//         string inputPanjang = Console.ReadLine() ?? "0";
//         Console.Write("Masukkan lebar : ");
//         string inputLebar = Console.ReadLine() ?? "0";
//         Console.Write("Masukan suhu dalam Celsius : ");
//         string inputSuhu = Console.ReadLine() ?? "0";

//         string brand = "Honda";
//         int year = 2024;
//         decimal price = 250000000m;
//         string formatedPrice = price.ToString("C", CultureInfo.GetCultureInfo("id-ID"));
//         bool isActive = true;
//         string date = DateTime.Now.ToString("dd/MM/yyyy");

//         Console.WriteLine("==== Data Kendaraan ====");
//         Console.WriteLine($"Merek    : {brand}");
//         Console.WriteLine($"Tahun    : {year}");
//         Console.WriteLine($"Harga    : {formatedPrice}");
//         Console.WriteLine($"Aktif    : {isActive}");
//         Console.WriteLine($"Tanggal  : {date}");
//         Console.WriteLine();
//         Console.WriteLine($"Luas parkir ({inputPanjang}m x {inputLebar}m) = {int.Parse(inputPanjang) * int.Parse(inputLebar)}m²");
//         Console.WriteLine($"Suhu {inputSuhu}°C = {(int.Parse(inputSuhu) * 9 / 5) + 32}°F");
//     }
// }


decimal hargaKendaraan = 350000000m;
decimal uangMuka = 30m; //persen
int tenor = 36; //bulan
decimal bunga = 0.5m; //persen

decimal dp = uangMuka / 100 * hargaKendaraan;
decimal pokok = hargaKendaraan - dp;
decimal bungaPerBulan = (pokok * bunga / 100) / tenor;
decimal cicilanPerBulan = pokok / tenor + bungaPerBulan;

Console.WriteLine($"Harga Kendaraan    : {hargaKendaraan:C}");
Console.WriteLine($"Uang Muka          : {uangMuka}%");
Console.WriteLine($"Tenor              : {tenor} bulan");
Console.WriteLine($"Bunga              : {bunga}%");
Console.WriteLine($"Down Payment       : {dp:C}");
Console.WriteLine($"Pokok Pinjaman     : {pokok:C}");
Console.WriteLine($"Bunga per Bulan    : {bungaPerBulan:C}");
Console.WriteLine($"Cicilan per Bulan  : {cicilanPerBulan:C}");



