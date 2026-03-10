// string namaPerusahaan = "PT. ABC";
// int tahunBerdiri = 1990;
// decimal omset = 1_000_000_000m;
// bool aktif = true;
// string? catatan = null;
// double rasioKeuntungan = 0.25;


// Console.WriteLine($"Nama Perusahaan: {namaPerusahaan}");
// Console.WriteLine($"Tahun Berdiri: {tahunBerdiri}");
// Console.WriteLine($"Omset: {omset:C}");
// Console.WriteLine($"Aktif: {aktif}");
// Console.WriteLine($"Rasio Keuntungan: {rasioKeuntungan:P}");
// if (catatan != null)
// {
//     Console.WriteLine($"Catatan: {catatan}");
// }
// else
// {
//     Console.WriteLine("Catatan: Tidak ada catatan.");
// }


// int a = 10;
// int b = 20;
// Console.WriteLine($"Hasil penjumlahan: {a + b}");
// Console.WriteLine($"Hasil pengurangan: {a - b}");
// Console.WriteLine($"Hasil perkalian: {a * b}");
// Console.WriteLine($"Hasil pembagian: {(double)a / b}");
// Console.WriteLine($"Hasil modulus: {a % b}");

// Console.WriteLine(a > b);
// Console.WriteLine(a < b);
// Console.WriteLine(a == b);
// Console.WriteLine(a != b);

// string numberString = "100";
// int number = int.Parse(numberString);
// int number2 = 20;

// Console.WriteLine($"Hasil konversi string ke int: {number}");
// Console.WriteLine($"Hasil penjumlahan: {number + number2}");

// string merk = "Honda";
// string tahun = "2026";
// decimal harga = 250_000_000m;
// bool active = true;
// DateTime tanggalPembelian = new DateTime(2024, 6, 1);

// int panjangKendaraan = 5;
// int lebarKendaraan = 3;

// int suhu = 100;
// int suhuFahrenheit = (suhu * 9 / 5) + 32;

// Console.WriteLine("=== Data Kendaraan ===");
// Console.WriteLine($"Merk \t: {merk}");
// Console.WriteLine($"Tahun \t: {tahun}");
// Console.WriteLine($"Harga \t: {harga:C}");
// Console.WriteLine($"Active \t: {active}");
// Console.WriteLine($"Dibuat \t: {tanggalPembelian:dd MMMM yyyy}");

// Console.WriteLine($"\n\nLuas Parkir ({panjangKendaraan}m x {lebarKendaraan}m): {panjangKendaraan * lebarKendaraan} m\u00B2");

// Console.WriteLine($"Suhu: {suhu}\u00B0C = {suhuFahrenheit}\u00B0F");

// Console.Write("\nInput Angka: ");
// int userInput = int.TryParse(Console.ReadLine(), out int result) ? result : 0;

// Console.WriteLine($"Hasil konversi input ke int * 2: {userInput * 2}");

decimal hargaKendaraan = 350_000_000m;
decimal uangMuka = 0.3m;
int tenor = 36;
decimal bungaPerBulan = 0.005m;
decimal uangMukaRupiah = hargaKendaraan * uangMuka;
decimal sisaPinjaman = hargaKendaraan - uangMukaRupiah;
decimal bungaPerBulanRupiah = sisaPinjaman * bungaPerBulan;



Console.WriteLine("=== Simulasi Kredit Kendaraan ===");
Console.WriteLine($"Harga Kendaraan: {hargaKendaraan:C}");
Console.WriteLine($"Uang Muka: {uangMuka:P} = {uangMukaRupiah:C}");
Console.WriteLine($"Tenor: {tenor} bulan");
Console.WriteLine($"Bunga per Bulan: {bungaPerBulan:P} = {bungaPerBulanRupiah:C}");
Console.WriteLine($"Cicilan per Bulan: {(sisaPinjaman / tenor) + bungaPerBulanRupiah:C}");