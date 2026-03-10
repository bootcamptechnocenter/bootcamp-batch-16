// string namaPerusahaan = "PT. ABC";
// int tahunBerdiri = 1990;
// decimal omzet = 1_000_000_000m;
// bool aktif = true;
// string? catatan = null;

// Console.WriteLine($"Nama Perusahaan: {namaPerusahaan}");
// Console.WriteLine($"Tahun Berdiri: {tahunBerdiri}");
// Console.WriteLine($"Omzet: {omzet:N0}");
// Console.WriteLine($"Aktif: {aktif}");
// Console.WriteLine($"Catatan: {catatan}");

// int a = 10;
// int b = 20;

// Console.WriteLine($"Hasil penjumlahan: {a + b}");
// Console.WriteLine($"Hasil pengurangan: {a - b}");
// Console.WriteLine($"Hasil perkalian: {a * b}");
// Console.WriteLine($"Hasil pembagian: {(double)a / b}");
// Console.WriteLine($"Hasil modulus: {a % b}");

// Console.WriteLine(a > b);
// Console.WriteLine($"Apakah a lebih kecil dari b? {a < b}");
// Console.WriteLine($"Apakah a sama dengan b? {a == b}");
// Console.WriteLine($"Apakah a tidak sama dengan b? {a != b}");

// string numberString = "100";
// int number = int.Parse(numberString);
// int number2 = 20;

// Console.WriteLine($"Hasil konversi string ke int: {number}");
// Console.WriteLine($"Hasil penjumlahan dengan konversi: {number + number2}");

// ------------------------------------------ //
// string merekKenderaan = "Honda";
// int tahunKenderaan = 2024;
// decimal hargaKenderaan = 250_000_000m;
// bool tersedia = true;
// DateTime dibuatPada = DateTime.Now;

// int HitungLuasParkir(int panjang, int lebar)
// {
//     return panjang * lebar;
// }

// int KonversiCelsiusKeFahrenheit(int celsius)
// {
//     return (celsius * 9 / 5) + 32;
// }

// Console.WriteLine("=== Data Kenderaan ===");
// Console.WriteLine($"Merek  : {merekKenderaan}");
// Console.WriteLine($"Tahun  : {tahunKenderaan}");
// Console.WriteLine($"Harga  : {hargaKenderaan:C}");
// Console.WriteLine($"Aktif  : {tersedia}");
// Console.WriteLine($"Dibuat : {dibuatPada:dd/MM/yyyy}");

// Console.WriteLine($"Luas parkir (5m x 3m) = {HitungLuasParkir(5, 3)} m²");
// Console.WriteLine($"Suhu 100°C = {KonversiCelsiusKeFahrenheit(100)} °F");

// Console.Write("kasih angka: ");
// string input = Console.ReadLine();
// int angka = int.Parse(input);
// Console.WriteLine($"{angka} x 2 = {angka * 2}");

// hargaKenderaan 350jt
// uangmuka 30% dari harga
// tenor 36 bulan
// bunga per bulan 0.5%

// cari harga pokok, harga dp, cicilan per bulan, bunga per bulan

decimal hargaKenderaan2 = 350_000_000m;
decimal uangMuka = hargaKenderaan2 * 0.30m;
decimal hargaPokok = hargaKenderaan2 - uangMuka;
int tenor = 36;
decimal bungaPerBulan = hargaPokok * 0.005m;
decimal cicilanPerBulan = (hargaPokok / tenor) + bungaPerBulan;
Console.WriteLine($"harga pokok: {hargaPokok:C}");
Console.WriteLine($"uang muka: {uangMuka:C}");
Console.WriteLine($"bunga per bulan: {bungaPerBulan:C}");
Console.WriteLine($"cicilan per bulan: {cicilanPerBulan:C}");


