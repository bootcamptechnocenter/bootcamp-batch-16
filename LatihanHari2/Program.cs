// //if statement

// int nilai = 80;

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

// string grade = score switch
// {
//     >= 90 => "A",
//     >= 80 => "B",
//     >= 70 => "C",
//     >= 60 => "D",
//     _ => "F"
// };

// Console.WriteLine($"Nilai: {score}, Grade: {grade}");


// //looping

// //for loop
// for (int i = 0; i < 10; i++)
// {
//     Console.WriteLine($"Perulangan ke-{i}");
// }


// //foreach loop
// var car = new List<string> { "Toyota", "Honda", "Ford" };

// foreach (var item in car)
// {
//     Console.WriteLine($"Merek mobil: {item}");
// }

using LatihanHari2;

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

var rekening = new Rekening();
rekening.Setor(1_000_000);
Console.WriteLine($"Saldo setelah setor: {rekening.Saldo}");
rekening.Tarik(250_000);
Console.WriteLine($"Saldo setelah tarik: {rekening.Saldo}");
rekening.Tarik(800_000); 





