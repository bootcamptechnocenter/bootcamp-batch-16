string namaPerusahaan = "PT. ABC";
int tahunBerdiri = 1990;
decimal omzet = 1_000_000_000m;
bool aktif = true;
string? catatan = null;

Console.WriteLine($"Nama Perusahaan: {namaPerusahaan}");
Console.WriteLine($"Tahun Berdiri: {tahunBerdiri}");
Console.WriteLine($"Omzet: {omzet:C}");
Console.WriteLine($"Aktif: {aktif}");
Console.WriteLine($"Catatan: {(catatan ?? "Tidak ada catatan")}");

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