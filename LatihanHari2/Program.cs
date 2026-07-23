// //if statement
using LatihanHari2.SuhuRuangan;


var suhu = new SuhuRuangan(0m);
Console.WriteLine($"Suhu: {suhu.Celcius}°C, {suhu.Fahrenheit}°F, {suhu.Kelvin}K, Deskripsi: Suhu ruangan saat ini {suhu.Deskripsi()}");


// int nilai = int.Parse(Console.ReadLine());

// if (nilai > 100)
// {
//     Console.WriteLine("Nilai melebihi batas maksimal");
// }
// else
// {
//     Console.WriteLine("Nilai masih dalam batas normal");
// }

// //switch statement
// int score = 85;

// string grade = int.TryParse(Console.ReadLine(), out int nilai) ? nilai switch
// {
//     >= 90 => "A",
//     >= 80 => "B",
//     >= 70 => "C",
//     >= 60 => "D",
//     _ => "E",
// }: "Input tidak valid";

// Console.WriteLine($"Nilai: {nilai}, Grade: {grade}");


// // //looping

// // //for loop
// // for (int i = 0; i < 10; i++)
// // {
// //     Console.WriteLine($"Perulangan ke-{i}");
// // }


// // //foreach loop
// // var car = new List<string> { "Toyota", "Honda", "Ford" };

// foreach (var item in car)
// {
//     Console.WriteLine($"Merek mobil: {item}");
// }

// using LatihanHari2;

// Animal cat = new Animal("Cat", "Fish", "Meow", false);

// cat.Eat();
// cat.IsCarnivoreOrNot();


// var animals = new List<Animal>
// {
//     new Animal("Dog", "Meat", "Woof", true),
//     new Animal("Cow", "Grass", "Moo", false),
//     new Animal("Lion", "Meat", "Roar", true)
// };

// foreach (var animal in animals)
// {
//     animal.Eat();
// }

// var email1 = new Email
// {
//     Name = "John Doe",
//     Address = "123 Main St",
//     Phone = "555-1234",
//     EmailAddress = "johndoe@mail.com"
// };

// var whatsapp1 = new WhatsApp
// {
//     Name = "Jane Smith",
//     Address = "456 Elm St",
//     Phone = "555-5678",
//     WhatsAppNumber = "08123456789"
// };

// email1.DisplayIdentity();
// whatsapp1.DisplayIdentity();

// var identities = new List<Identity> { email1, whatsapp1 };
// foreach (var identity in identities)
// {
//     Console.WriteLine($"Type of Identity: {identity.TypeOfIdentity()}");
// }

// var rekening = new Rekening();
// rekening.Setor(1_000_000);
// Console.WriteLine($"Saldo setelah setor: {rekening.Saldo}");
// rekening.Tarik(250_000);
// Console.WriteLine($"Saldo setelah tarik: {rekening.Saldo}");
// rekening.Tarik(800_000); 




