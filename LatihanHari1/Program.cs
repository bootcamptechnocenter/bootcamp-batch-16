using LatihanHari1.Car;

string namaPerusahaan = "PT. ABC";
int tahunBerdiri = 1990;
decimal omzet = 1_000_000_000;
bool aktif = true;
string? catatan = null;



Console.WriteLine($"Nama Perusahaan: {namaPerusahaan}");
Console.WriteLine($"Tahun Berdiri: {tahunBerdiri}");
Console.WriteLine($"Omzet: {omzet:C}");
Console.WriteLine($"Aktif: {aktif}");


int a = 10;
int b = 20;

Console.WriteLine($"Hasil penjumlahan: {a + b}");
Console.WriteLine($"Hasil pengurangan: {a - b}");
Console.WriteLine($"Hasil perkalian: {a * b}");
Console.WriteLine($"Hasil pembagian: {a / b}");
Console.WriteLine("Hasil modulus: " + (a % b));

Console.WriteLine(a > b);
Console.WriteLine(a < b);
Console.WriteLine(a == b);
Console.WriteLine(a != b);


string numberString = "100";
int number = int.Parse(numberString);
int number2 = 20;

Console.WriteLine($"Hasil konversi string ke int: {number}");
Console.WriteLine($"Hasil penjumlahan: {number + number2}");

Car car1 = new Car
{
    Brand = "Toyota",
    Model = "Corolla"
};

Console.WriteLine($"Mobil: {car1.Brand} {car1.Model}");







