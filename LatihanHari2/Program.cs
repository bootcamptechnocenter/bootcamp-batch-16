// 👌 IF ELSE

// int nilai = 80;
// string pesan = nilai > 100 ? "Nilaimu melebihi batas maksimal kawannn! 😮" : "Nilaimu masih normal cuyyy! 😎";
// Console.WriteLine(pesan);



// 👌 SWITCH CASE
// int score = 85;
// string grade = score switch {
//     >= 90 => "A",
//     >= 80 => "B",
//     >= 70 => "C",
//     >= 60 => "D",
//     _ => "F"
// };
// Console.WriteLine($"Nilai : {score}, Grade : {grade}");



// 👌 FOR LOOP
// for (int i = 0; i < 100; i++)
// {
//     int jumlahBagi = 0;

//     for (int j = 1; j <= i; j++)
//     {
//         if (i % j == 0 )
//         {
//             jumlahBagi++;
//         }
//     }

//     if (jumlahBagi == 2)
//     {
//         Console.WriteLine($"{i} adalah bilangan prima.");
//     }
// }




// 👌 FOREACH LOOP
// var car = new List<string> {"Toyota", "Honda", "Ford"};
// foreach (var item in car)
// {
//     Console.WriteLine(item);
// }



// 👌 OBJECT
// Animal cat = new Animal("Cat", "Fish", "EERERE", false);

// cat.Eat();
// cat.IsCarnivoreOrNot();


// var animals = new List<Animal>
// {
//     new Animal("Sandi", "Fish", "EERERE", true),
//     new Animal("Salim", "Fish", "EERERE", true),
//     new Animal("Rahma", "Fish", "EERERE", true),
// };

// foreach(var animal in animals) {
//     animal.Eat();
// }


// var email1 = new Email
// {
//     Name = "Agus",
//     Address = "Jalan sukarno hatta",
//     Phone = "0812313",
//     EmailAddress = "test@gmail.com",
// };

// var whatsapp1 = new WhatsApp
// {
//     Name = "Rahmat",
//     Address = "Jalan wahid hasim",
//     Phone = "0833244223",
//     WhatsAppNumber = "098123123123"
// };

// var identities = new List<Identity> {email1, whatsapp1};
// email1.DisplayIdentity();
// whatsapp1.DisplayIdentity();
// foreach (var identity in identities)
// {
//     Console.WriteLine($"Type of identity : {identity.TypeOfIdentity()}");
// }

// Console.WriteLine($"{email1.Name} mempunyai alamat email {email1.EmailAddress}");


// var pegawai = new Pegawai {
//     Id = "1",
//     Nama = "Anton",
//     GajiPokok = 6000000m,
//     TunjanganPersen = 2m
// };

// pegawai.PrintSlipGaji();

using LatihanHari2;

var rekening = new Rekening();
rekening.Setor(1_000_000);
Console.WriteLine($"Saldo setelah setor : {rekening.Saldo}");
rekening.Tarik(250_000);
Console.WriteLine($"Saldo setelah tarik : {rekening.Saldo}");
rekening.Tarik(800_000);