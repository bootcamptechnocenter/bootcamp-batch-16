using day_2;

Console.WriteLine("\n=== Percabangan If-Else ===\n");
int nilai = 85;
if (nilai >= 90)
{
    Console.WriteLine("Nilai A");
}
else if (nilai >= 80)
{
    Console.WriteLine("Nilai B");
}
else if (nilai >= 70)
{
    Console.WriteLine("Nilai C");
}
else if (nilai >= 60)
{
    Console.WriteLine("Nilai D");
}
else
{
    Console.WriteLine("Nilai E");
}

string result = nilai > 100 ? "Nilai melebihi batas maksimal" : "Nilai masih dalam batas normal";
Console.WriteLine(result);

int score = 90;

string grade = score switch
{
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    >= 60 => "D",
    _ => "E"
};
Console.WriteLine($"Nilai: {score}, Grade: {grade}");

Console.WriteLine("\n=== Perulangan For ===\n");

for (int i = 0; i < 20; i++)
{
    bool isPrime = true;
    for (int j = 2; j <= i / 2; j++)
    {
        if (i % j == 0)
        {
            isPrime = false;
            break;
        }
    }
    if (isPrime && i > 1)
    {
        Console.WriteLine($"{i} adalah bilangan prima");
    }
}

Console.WriteLine("\n=== Perulangan Foreach ===\n");

List<string> car = new List<string> { "BMW", "Mercedes", "Audi", "Toyota", "Honda" };

foreach (var item in car)
{
    Console.WriteLine($"Mobil: {item}");
}

Console.WriteLine("\n=== Perulangan While ===\n");
int count = 0;
while (count < 5)
{
    Console.WriteLine($"Count: {count}");
    count++;
}

Console.WriteLine("\n=== Class ===\n");

Animal cat = new Animal("Kucing", "Ikan", "Meow", true);

Console.WriteLine($"Nama: {cat.Name}");
Console.WriteLine($"Makanan: {cat.Food}");
Console.WriteLine($"Suara: {cat.Sound}");
cat.Eat();
cat.IsCarnivoreOrNot();

List<Animal> animals = new List<Animal>
{
    new Animal("Anjing", "Daging", "Guk-guk", true),
    new Animal("Kelinci", "Wortel", "Cuit-cuit", false),
    new Animal("Sapi", "Rumput", "Moo", false)
};

foreach (var animal in animals)
{
    Console.WriteLine($"\nNama: {animal.Name}");
    Console.WriteLine($"Makanan: {animal.Food}");
    Console.WriteLine($"Suara: {animal.Sound}");
    animal.Eat();
    animal.IsCarnivoreOrNot();
    Console.WriteLine("====================================");
}

Console.WriteLine("\n=== Inheritance ===\n");
Email email = new Email("John Doe", "123 Main St", "555-1234", "john.doe@example.com");
email.DisplayIdentity();
Console.WriteLine($"Type of Identity: {email.TypeOfIdentity()}");

Console.WriteLine("\n=== Latihan 1 ===\n");
Pegawai pegawai = new Pegawai
{
    Id = "P001",
    Nama = "Minji",
    GajiPokok = 10_000_000,
    TunjanganPersen = 0.2
};
Pegawai pegawai2 = new Pegawai
{
    Id = "P002",
    Nama = "Ian",
    GajiPokok = 6_000_000,
    TunjanganPersen = 0.25
};
Pegawai pegawai3 = new Pegawai
{
    Id = "P003",
    Nama = "Asa",
    GajiPokok = 8_000_000,
    TunjanganPersen = 0.25
};

pegawai.PrintSlipGaji();
pegawai2.PrintSlipGaji();
pegawai3.PrintSlipGaji();

Console.WriteLine("\n=== Encapsulation ===\n");

var rekening = new Rekening();
rekening.Setor(1_000_000);
Console.WriteLine($"Saldo setelah setor: {rekening.Saldo}");
rekening.Tarik(200_000);
Console.WriteLine($"Saldo setelah tarik: {rekening.Saldo}");
rekening.Tarik(900_000);