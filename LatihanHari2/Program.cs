// // IF STATEMENT
// int nilai = 85;
// Console.WriteLine(nilai > 100 ? "Nilai melebihi batas maksimal" : "Nilai masih dalam batas normal");
// Console.WriteLine();

// // SWITCH CASE
// int score = 85;

// string grade = score switch
// {
//     >= 90 => "A",
//     >= 80 => "B",
//     >= 70 => "C",
//     >= 60 => "D",
//     _ => "F"
// };

// Console.WriteLine($"NILAI: {score}, GRADE: {grade}");
// Console.WriteLine();

// // FOR LOOP
// for (int i = 2; i < 100; i++)
// {
//     int jumlah = 0;
//     for (int j = 1; j <= i; j++)
//     {
//         if (i % j == 0)
//         {
//             jumlah++;
//         }
//     }
//     if (jumlah == 2)
//     {
//         Console.WriteLine($"Bilangan Prima = {i}");
//     } 
// }
// Console.WriteLine();

// //FOREACH LOOP
// var cars = new List<string> { "Toyota", "Honda","Ford"};

// foreach (var car in cars) {
//     Console.WriteLine($"Merek Mobil: {car}");
// }


// using LatihanHari2;

// Animal cat = new Animal("Joni", "Fish", "miaw", true);

// cat.Eat();
// cat.IsCarnivoreOrNot();

// var animals = new List<Animal>
// {
//     new Animal("Dog", "Meat", "woof", true),
//     new Animal("Cow", "Grass", "moo", false),
//     new Animal("Lion", "Meat", "rawr", true)
// };

// foreach (var animal in animals)
// {
//     animal.Eat();
//     animal.IsCarnivoreOrNot();
// }


// using LatihanHari2;

// var email1 = new Email
// {
//     Name = "jojo",
//     Address = "jogja",
//     Phone = "0899923892",
//     EmailAddress = "jojo@gmail.com",
// };

// var whatsapp1 = new WhatsApp
// {
//     Name = "jojo2",
//     Address = "jogja2",
//     Phone = "0899923892",
//     WhatsAppNumber = "08123343542",
// };

// email1.DisplayIdentity();
// whatsapp1.DisplayIdentity();

// var identities = new List<Identity> {email1, whatsapp1};
// foreach (var identity in identities)
// {
//     Console.WriteLine($"Type of identity: {identity.TypeOfIdentity()}");
// }

// using LatihanHari2;
// var Pegawai1 = new Pegawai
// {
//     Id = "A12839",
//     Nama = "Budi",
//     GajiPokok = 5_000_000m,
//     TunjanganPersen = 0.1m
// };

// Pegawai1.PrintSlipGaji();

using LatihanHari2;

var rekening = new Rekening();
rekening.Setor(1_000_000);
Console.WriteLine($"Saldo Setelah setor: {rekening.Saldo}");
rekening.Tarik(250_000);
Console.WriteLine($"Saldo Setelah tarik: {rekening.Saldo}");
rekening.Tarik(800_000);

Console.ReadLine();
Console.Clear();