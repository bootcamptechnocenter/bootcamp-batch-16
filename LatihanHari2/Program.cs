// =============== if statement ==============
// int nilai = 80;
// // if (nilai > 100)
// // {
// //     Console.WriteLine("Nilai melebihi batas maksimal");
// // }
// // else {
// //     Console.WriteLine("Nilai masih dalam batas normal")
// // }

// string state = nilai > 100 ? "Nilai melebihi batas maksimal" : "Nilai masih dalam batas normal";
// Console.WriteLine(state);

// // ================ switch statement ==============
// int score = 85;

// string grade = score switch
// {
//     >= 90 => "A",
//     >= 80 => "B",
//     >= 70 => "C",
//     >= 60 => "D",
//     _ => "E"
// };
// Console.WriteLine($"Nilai: {score}, Grade: {grade}");

// // ================ For loop ==============
// for (int i = 1; i < 10; i++)
// {
//     Console.WriteLine($"Perulangan ke-{i}");
// }

// // bilangan prima
// for (int i = 1; i < 10; i++)
// {
//     bool isPrime = true;
//     if (i == 1) isPrime = false;
//     for (int j = 2; j < i; j++)
//     {
//         if (i % j == 0)
//         {
//             isPrime = false;
//             break;
//         }
//     }
//     if (isPrime)
//     {
//         Console.WriteLine($"bilangan prima: {i}");
//     }
// }

// // ========= foreach loop ==============
// var car = new List<string> { "Honda", "Toyota", "Ford" };
// foreach (var item in car)
// {
//     Console.WriteLine($"Merek mobil: {item}");
// }


using LatihanHari2;

// Animal cat = new Animal
// {
//     Name = "Kitty",
//     Food = "Fish",
//     Sound = "Meow",
//     isCarnivore = true
// };

// Animal cat = new Animal("Cat", "Fish", "Meow", true);

// cat.Eat();
// cat.isCarnivoreOrNot();

// var animals = new List<Animal>
// {
//     new Animal("Dog", "Meat", "Woof", true),
//     new Animal("Cow", "Grass", "Moo", false),
//     new Animal("Lion", "Meat", "Roar", true)
// };

// foreach (var animal in animals)
// {
//     animal.Eat();
//     animal.isCarnivoreOrNot();
// }

// var email1 = new Email
// {
//     Name = "Erlan",
//     Address = "Jl. Merdeka No. 123",
//     Phone = "555-123-4567",
//     EmailAddress = "erlan@example.com"
// };

// var whatsapp1 = new Whatsapp {
//     Name = "Jane Smith",
//     Address = "Jl. Sudirman No. 456",
//     Phone = "555-555-5555",
//     WhatsappNumber = "081234567891"
// };

// email1.DisplayIdentity();
// whatsapp1.DisplayIdentity();

// var identities = new List<Identity> { email1, whatsapp1 };
// foreach (var identity in identities)
// {
//     Console.WriteLine($"Type of Identity: {identity.TypeOfIdentity()}");
// }

var rekening = new Rekening();
rekening.Setor(1_000_000);
Console.WriteLine($"Saldo setelah setor: {rekening.Saldo:C}");
rekening.Tarik(250_000);
Console.WriteLine($"Saldo setelah tarik: {rekening.Saldo:C}");
rekening.Tarik(800_000);