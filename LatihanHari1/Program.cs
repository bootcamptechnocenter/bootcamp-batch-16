// string namaPerusahaan = "PT. ABC";
// int tahunBerdiri = 1999;
// decimal omzet = 1_000_000_000m;
// bool aktif = true;
// string? catatan = null;

// Console.WriteLine($"Nama PErusahaan: {namaPerusahaan}");
// Console.WriteLine($"Tahun Berdiri: {tahunBerdiri}");
// Console.WriteLine($"Omzet: {omzet:C}");
// Console.WriteLine($"Aktif: {aktif}");
// Console.WriteLine($"Catatan: {catatan}");

// int a = 10;
// int b = 20;

// Console.WriteLine($"Hasil penjumlahan: {a + b}");
// Console.WriteLine($"Hasil pengurangan: {a - b}");
// Console.WriteLine($"Hasil perkalian: {a * b}");
// Console.WriteLine($"Hasil pembagian: {a / b}");
// Console.WriteLine($"Hasil modulus: {a % b}");

// Console.WriteLine(a>b);
// Console.WriteLine(a<b);
// Console.WriteLine(a==b);
// Console.WriteLine(a!=b);

// string numberString = "100";
// int number = int.Parse(numberString);
// int number2 = 20;
// Console.WriteLine($"Hasil konversi string ke int: {number}");
// Console.WriteLine($"Hasil penjumlahan: {number + number2}");


// // SOAL 1
// string merk = "Honda";
// int tahun = 2024;
// decimal harga = 250_000_000m;
// bool aktif = true;
// string dibuat = DateTime.Now.ToString("dd/MM/yyyy");

// //SOAL 2
// Console.WriteLine($"=== Data Kendaraan ====");
// Console.WriteLine($"Merk\t: {merk}");
// Console.WriteLine($"Tahun\t: {tahun}");
// Console.WriteLine($"Harga\t: {harga}");
// Console.WriteLine($"Aktif\t: {aktif}");
// Console.WriteLine($"Dibuat\t: {dibuat}");
// Console.WriteLine("\n");
// //SOAL 3
// float panjang = 5;
// float lebar = 3;
// float luas = panjang * lebar;
// float celcius = 100;
// float farenheit = (celcius * (9/5)) + 32;
// Console.WriteLine($"Luas parkir ({panjang} m x {lebar} m) = {luas} m²");
// Console.WriteLine($"Suhu {celcius}°C = {farenheit}°F");
// Console.WriteLine("\n");

// //OPSIONAL
// int angka;
// Console.Write("Masukkan angka: ");
// if (int.TryParse(Console.ReadLine(), out angka)) {
//     Console.WriteLine($"Angka yang dimasukkan: {angka * 2}");
// }
// else {
//     Console.WriteLine("Input harus berupa integer!");
// }

//Harga kendaraan 350 juta, uang muka 30 persen dari harga, tenor 36 bulan, bunga 0.5 persen per bulan. Cari dp, pokok (harga - dp) bunga per bulan, cicilan perbulan. output: Harga, DP, pokok, bunga per bulan, cicilan per bulan
decimal hargaKendaraan = 350_000_000m;
decimal persenDP = 30m/100m;
int tenorBulan = 36;
decimal persentaseBunga = 0.5m/100m;
decimal dp = hargaKendaraan * persenDP;
decimal pokok = hargaKendaraan - dp;
decimal pokokPerBulan = pokok / tenorBulan;
decimal bungaPerBulan = pokok * persentaseBunga;
decimal cicilanPerBulan = pokokPerBulan + bungaPerBulan;

Console.WriteLine($"=== Data Cicilan ====");
Console.WriteLine($"Harga\t\t\t: {hargaKendaraan:C}");
Console.WriteLine($"Uang Muka\t\t: {dp:C}");
Console.WriteLine($"Pokok\t\t\t: {pokok:C}");
Console.WriteLine($"Bunga per Bulan\t\t: {bungaPerBulan:C}");
Console.WriteLine($"Cicilan per Bulan\t: {cicilanPerBulan:C}");