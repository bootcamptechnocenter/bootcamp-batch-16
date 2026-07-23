// using LatihanHari1.Car;

// string namaPerusahaan = "PT. ABC";
// int tahunBerdiri = 1990;
// decimal omzet = 1_000_000_000;
// bool aktif = true;
// string? catatan = null;



// Console.WriteLine($"Nama Perusahaan: {namaPerusahaan}");
// Console.WriteLine($"Tahun Berdiri: {tahunBerdiri}");
// Console.WriteLine($"Omzet: {omzet:C}");
// Console.WriteLine($"Aktif: {aktif}");


// int a = 10;
// int b = 20;

// Console.WriteLine($"Hasil penjumlahan: {a + b}");
// Console.WriteLine($"Hasil pengurangan: {a - b}");
// Console.WriteLine($"Hasil perkalian: {a * b}");
// Console.WriteLine($"Hasil pembagian: {a / b}");
// Console.WriteLine("Hasil modulus: " + (a % b));

// Console.WriteLine(a > b);
// Console.WriteLine(a < b);
// Console.WriteLine(a == b);
// Console.WriteLine(a != b);


// string numberString = "100";
// int number = int.Parse(numberString);
// int number2 = 20;

// Console.WriteLine($"Hasil konversi string ke int: {number}");
// Console.WriteLine($"Hasil penjumlahan: {number + number2}");

// Car car1 = new Car
// {
//     Brand = "Toyota",
//     Model = "Corolla"
// };

// Console.WriteLine($"Mobil: {car1.Brand} {car1.Model}");



// === INPUT 3 KENDARAAN ===
string[] kode = new string[3], nama = new string[3], model = new string[3];
int[] tahun = new int[3], stok = new int[3];
decimal[] harga = new decimal[3];

for (int i = 0; i < 3; i++)
{
    Console.WriteLine($"\n=== Kendaraan {i + 1} ===");
    Console.Write("Kode Brand  : ");
    kode[i] = (Console.ReadLine() ?? "").Trim().ToUpper();
    if (kode[i].Length != 3) { Console.WriteLine("❌ Kode harus 3 huruf!"); i--; continue; }

    Console.Write("Nama Brand  : ");
    nama[i] = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrEmpty(nama[i])) { Console.WriteLine("❌ Nama tidak boleh kosong!"); i--; continue; }

    Console.Write("Model       : ");
    model[i] = (Console.ReadLine() ?? "").Trim();
    if (string.IsNullOrEmpty(model[i])) { Console.WriteLine("❌ Model tidak boleh kosong!"); i--; continue; }

    Console.Write("Tahun       : ");
    if (!int.TryParse(Console.ReadLine(), out tahun[i]) || tahun[i] < 1900 || tahun[i] > 2026)
    { Console.WriteLine("❌ Tahun 1900-2026!"); i--; continue; }

    Console.Write("Harga OTR   : ");
    if (!decimal.TryParse(Console.ReadLine(), out harga[i]) || harga[i] <= 0)
    { Console.WriteLine("❌ Harga harus > 0!"); i--; continue; }

    Console.Write("Stok        : ");
    if (!int.TryParse(Console.ReadLine(), out stok[i]) || stok[i] < 0 || stok[i] > 999)
    { Console.WriteLine("❌ Stok 0-999!"); i--; continue; }
}

// === HITUNG ===
decimal totalHarga = 0; int totalStok = 0;
decimal hargaMax = decimal.MinValue, hargaMin = decimal.MaxValue;
int stokMax = int.MinValue, stokMin = int.MaxValue;
int idxHargaMax = 0, idxHargaMin = 0, idxStokMax = 0, idxStokMin = 0;

for (int i = 0; i < 3; i++)
{
    totalHarga += harga[i];
    totalStok += stok[i];

    if (harga[i] > hargaMax) { hargaMax = harga[i]; idxHargaMax = i; }
    if (harga[i] < hargaMin) { hargaMin = harga[i]; idxHargaMin = i; }
    if (stok[i] > stokMax)   { stokMax = stok[i];   idxStokMax = i; }
    if (stok[i] < stokMin)   { stokMin = stok[i];   idxStokMin = i; }
}

decimal rataHarga = totalHarga / 3;
decimal rataStok = totalStok / 3m;

// === OUTPUT ===
Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
Console.WriteLine("║              🚗 DEALER CONSOLE — LAPORAN 🚗                   ║");
Console.WriteLine("╠═══════════════════════════════════════════════════════════════╣");
Console.WriteLine("║ Kode  Nama         Model          Tahun  Harga OTR       Stok ║");
Console.WriteLine("╠═══════════════════════════════════════════════════════════════╣");
for (int i = 0; i < 3; i++)
    Console.WriteLine($"║ {kode[i],-4}  {nama[i],-12} {model[i],-14} {tahun[i],-5}  Rp {harga[i],12:N0}  {stok[i],4} ║");
Console.WriteLine("╠═══════════════════════════════════════════════════════════════╣");
Console.WriteLine($"║ TOTAL                               Rp {totalHarga,12:N0}  {totalStok,4} ║");
Console.WriteLine($"║ RATA-RATA                           Rp {rataHarga,12:N0}    {rataStok,4:F0} ║");
Console.WriteLine($"║ HARGA TERMAHAL     : {nama[idxHargaMax]} {model[idxHargaMax]} — Rp {hargaMax:N0}      ║");
Console.WriteLine($"║ HARGA TERMURAH     : {nama[idxHargaMin]} {model[idxHargaMin]} — Rp {hargaMin:N0}         ║");
Console.WriteLine($"║ STOK TERBANYAK     : {nama[idxStokMax]} {model[idxStokMax]} ({stokMax} unit)                ║");
Console.WriteLine($"║ STOK TERSEDIKIT    : {nama[idxStokMin]} {model[idxStokMin]} ({stokMin} unit)              ║");
Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");
Console.WriteLine($"\n📅 Laporan dibuat: {DateTime.Now:dd MMM yyyy HH:mm}");



