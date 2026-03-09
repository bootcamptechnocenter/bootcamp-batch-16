// string namaPerusahaan = "Berijalan";
// int tahunBerdiri = 1945;
// decimal omzet = 1_000_000_000m;
// bool aktif = true;
// string? catatan = null;

// Console.WriteLine($"Nama perusahaan: {namaPerusahaan}");
// Console.WriteLine($"Tahun berdiri: {tahunBerdiri}");
// Console.WriteLine($"Omzet: {omzet:C}");
// Console.WriteLine($"Aktif: {aktif}");
// Console.WriteLine($"Catatan: {catatan}");

// int a = 10;
// int b = 20;

// Console.WriteLine($"Penjumlahan: {a+b}");
// Console.WriteLine($"Pengurangan: {a-b}");
// Console.WriteLine($"Perkalian: {a*b}");
// Console.WriteLine($"Pembagian: {a/b}");
// Console.WriteLine($"Sisa bagi: {a%b}");

// Console.WriteLine($"a > b: {a > b}");
// Console.WriteLine($"a < b: {a < b}");
// Console.WriteLine($"a == b: {a == b}");
// Console.WriteLine($"a != b: {a != b}");
// Console.WriteLine($"a >= b: {a >= b}");
// Console.WriteLine($"a <= b: {a <= b}");

// string numberString = "123";
// int number = int.Parse(numberString);
// Console.WriteLine($"Number: {number}"); 

// Console.WriteLine($"Penjumlahan numberString + number: {numberString + number}");
// Console.WriteLine($"Penjumlahan number + number: {number + number}");


// string merek = "Honda";
// int tahun = 2024;
// decimal harga = 250_000_000m;
// bool aktif = true;
// DateTime dibuat = new DateTime(2026, 3, 3);

// int panjang = 5;
// int lebar = 3;
// int luasParkir = panjang * lebar;
// int suhu = 212;

// Console.WriteLine("=== Data Kendaraan ===");
// Console.WriteLine($"Merek\t: {merek}");
// Console.WriteLine($"Tahun\t: {tahun}");
// Console.WriteLine($"Harga\t: Rp {harga:N0}");
// Console.WriteLine($"Aktif\t: {aktif}");
// Console.WriteLine($"Dibuat\t: {dibuat.ToString("dd/MM/yyyy")}\n");
// Console.WriteLine($"Luas Parkir ({panjang}m x {lebar}m) = {luasParkir} m²");
// Console.WriteLine($"Suhu 100°C = {suhu}°F");



// Soal Latihan
int otr = 350_000_000;
double dp = 0.3*otr;
int tenor = 36;
double flat_rate = 0.5/100;

Console.WriteLine("=== Simulasi Kredit Mobil ===");
Console.WriteLine($"Pokok hutang\t: {otr - dp:C}");

Console.WriteLine($"Bunga flat perbulan\t: {(otr - dp) / tenor * flat_rate:C}");
Console.WriteLine($"Angsuran perbulan\t: {(otr - dp) / tenor + (otr - dp) / tenor * flat_rate:C}");