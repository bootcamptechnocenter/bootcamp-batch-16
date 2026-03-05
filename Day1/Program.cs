string namaPerusahaan = "PT. ABC";
int tahunBerdiri = 1990;
decimal omzet = 1_000_000_000m;
// bool aktif = true;
// string? catatan = null;

Console.WriteLine($"Nama Perusahaan: {namaPerusahaan}");
Console.WriteLine($"Tahun Berdiri: {tahunBerdiri}");
Console.WriteLine($"Omzet: {omzet:C}");
// Console.WriteLine($"Aktif: {aktif}");
// Console.WriteLine($"Catatan: {(catatan ?? "Tidak ada catatan")}");

int a = 10;
int b = 20;

Console.WriteLine($"Hasil penjumlahan: {a + b}");
Console.WriteLine($"Hasil pengurangan: {a - b}");
Console.WriteLine($"Hasil perkalian: {a * b}");
Console.WriteLine($"Hasil pembagian: {b / a}");
Console.WriteLine($"Hasil modulus: {b % a}");

Console.WriteLine($"Apakah a lebih besar dari b? {a > b}");
Console.WriteLine($"Apakah a sama dengan b? {a == b}");
Console.WriteLine($"Apakah a tidak sama dengan b? {a != b}");
Console.WriteLine($"Apakah a lebih besar atau sama dengan b? {a >= b}");
Console.WriteLine($"Apakah a lebih kecil dari b? {a < b}");
Console.WriteLine($"Apakah a lebih kecil atau sama dengan b? {a <= b}");

string numberString = "123";
int number = int.Parse(numberString);
Console.WriteLine($"Hasil konversi string ke int: {number}");

string invalidNumberString = "abc";
Console.WriteLine($"Apakah '{invalidNumberString}' dapat dikonversi ke int? {int.TryParse(invalidNumberString, out int result)}");

try
{
    int invalidNumber = int.Parse(invalidNumberString);
}
catch (FormatException ex)
{
    Console.WriteLine($"Terjadi kesalahan format: {ex.Message}");
}
Console.WriteLine("");
Console.WriteLine("");
Console.WriteLine("");

Console.Write("== User Input ==");

Console.WriteLine("");

Console.Write("Masukkan panjang: ");
string? panjangInput = Console.ReadLine();
Console.Write("Masukkan lebar: ");
string? lebarInput = Console.ReadLine();
Console.Write("Masukkan suhu dalam Celsius: ");
string? suhu = Console.ReadLine();

Console.WriteLine("");
Console.WriteLine("");
Console.WriteLine("");

// Exercise
string merek = "Honda";
int tahun = 2024;
decimal harga = 250_000_000m;
bool aktif = true;
DateTime createdAt = DateTime.Parse("2026-03-03");

string info = $@"== Data Kendaraan ==
Merek   : {merek}
Tahun   : {tahun}
Harga   : {harga:C}
Aktif   : {aktif}
Dibuat  : {createdAt:dd/MM/yy}";

Console.WriteLine(info);

Console.WriteLine("");

if (!double.TryParse(panjangInput, out double panjang))
{
    Console.WriteLine("Input panjang tidak valid. Menggunakan 0.");
    panjang = 0;
}
if (!double.TryParse(lebarInput, out double lebar))
{
    Console.WriteLine("Input lebar tidak valid. Menggunakan 0.");
    lebar = 0;
}

double luasParkir = panjang * lebar;
Console.WriteLine($"Luas parkir ({panjang}m x {lebar}m): {luasParkir} m²");

if (double.TryParse(suhu, out double suhuCelsius))
{
    double suhuFahrenheit = (suhuCelsius * 9 / 5) + 32;
    Console.WriteLine($"Suhu {suhuCelsius}°C = {suhuFahrenheit}°F");
}
else
{
    Console.WriteLine("Input suhu tidak valid.");
}

// harga kendaraan = 350_000_000m;
// uang muka = 30% dari harga
// tenor = 36 bulan
// bunga per bulan = 0.5%
// cari harga dp, bunga perbulan, cicilan per bulan, pokok

decimal hargaKendaraan = 350_000_000m;
decimal uangMuka = hargaKendaraan * 0.3m;
decimal sisaPembayaran = hargaKendaraan - uangMuka;
decimal bungaPerBulan = sisaPembayaran * 0.005m;
decimal pokokPerBulan = sisaPembayaran / 36;
decimal cicilanPerBulan = pokokPerBulan + bungaPerBulan;

string hasil = $@"
== Simulasi Kredit Kendaraan ==

Harga Kendaraan  : {hargaKendaraan:N0}
Uang Muka        : {uangMuka:N0}
Sisa Pembayaran  : {sisaPembayaran:N0}
Bunga per Bulan  : {bungaPerBulan:N0}
Pokok per Bulan  : {pokokPerBulan:N0}
Cicilan per Bulan: {cicilanPerBulan:N0}

Total Pembayaran : {(cicilanPerBulan * 36) + uangMuka:N0}
";

Console.WriteLine(hasil);
