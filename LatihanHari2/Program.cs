
// int nilai = 80;

// if (nilai > 100)
// {
//     Console.WriteLine("Nilai melebihi batas maksimal");
// }
// else
// {
//     Console.WriteLine("Nilai masih dalam batas normal");
// }

// Console.WriteLine("\nTernary Version");

// //ternary operator
// string hasil = nilai > 100 ? "Nilai melebihi batas maksimal" : "Nilai masih dalam batas normal";
// Console.WriteLine(hasil);


//switch case

// int score = 85;
// int grade = score switch
// {
//     >= 90 => 'A',
//     >= 80 => 'B',
//     >= 70 => 'C',
//     >= 60 => 'D',
//     _ => 'F'
// };

// Console.WriteLine($"Score: {score}, Grade: {(char)grade}");


// //looping

// //for loop
// for (int i = 2; i <= 10; i++)
// {
//     for (int pembagi = 2; pembagi <= i; pembagi++)
//     {
//         if (i % pembagi == 0)
//         {
//             if (pembagi == i)
//             {
//                 Console.WriteLine($"{i} adalah bilangan prima");
//             }
//             break;
//         }
//     }
// }


//foreach loop
// var car = new List<string> { "Honda", "Toyota", "Suzuki" };
// foreach (var item in car)
// {
//     Console.WriteLine($"Merek mobil: {item}");
// }


using LatihanHari2;

// Animal cat = new Animal("Kucing", "Ikan", "Miau", true);

// cat.Eat();
// cat.IsCarnivoreOrNot();

// var animals = new List<Animal>
// {
//     new Animal("Kucing", "Ikan", "Miau", true),
//     new Animal("Anjing", "Daging", "Guk", true),
//     new Animal("Singa", "Daging", "Roar", true)
// };

// foreach (var animal in animals)
// {
//     animal.Eat();
//     animal.IsCarnivoreOrNot();
//     Console.WriteLine();
// }


// var email = new EmailAddress
// {
//     Name = "John Doe",
//     Address = "123 Main St",
//     Phone = "555-1234",
//     Email = "johndoe@yopmail.com"
// };

// Console.WriteLine($"Name: {email.Name}");
// Console.WriteLine($"Address: {email.Address}");
// Console.WriteLine($"Phone: {email.Phone}");
// Console.WriteLine($"Email: {email.Email}");

// Console.WriteLine($"Type of Identity: {email.TypeOfIdentity()}");

// email.DisplayIdentity();

// var whatsapp = new WhatsApp
// {
//     Name = "Jane Doe",
//     Address = "456 Elm St",
//     Phone = "555-5678",
//     WhatsAppNumber = "08123456789"
// };

// var identityList = new List<Identity> { email, whatsapp };

// foreach (var identity in identityList)
// {
//     Console.WriteLine($"Type of Identity: {identity.TypeOfIdentity()}");
//     identity.DisplayIdentity();
//     Console.WriteLine();
// }


// LATIHAN MANDIRI
// Pegawai pegawai1 = new Pegawai("A4309", "Budi", 5_000_000, 0.1);
// Pegawai pegawai2 = new Pegawai("A4310", "Dwi", 6_000_000, 0.15);
// Pegawai pegawai3 = new Pegawai("A4311", "Sari", 7_000_000, 0.2);

// pegawai1.PrintSlipGaji();
// pegawai2.PrintSlipGaji();
// pegawai3.PrintSlipGaji();



var rekening = new Rekening();
rekening.Setor(1_000_000);
Console.WriteLine($"Saldo setelah setor: {rekening.Saldo:C}");
rekening.Tarik(200_000);
Console.WriteLine($"Saldo setelah tarik: {rekening.Saldo:C}");
rekening.Tarik(800_000);