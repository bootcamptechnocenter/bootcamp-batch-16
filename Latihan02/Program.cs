using Latihan02;

var rekening = new Rekening();
rekening.Setor(1_000_000);
Console.WriteLine($"Saldo setelah setor: {rekening.Saldo}");
rekening.Tarik(250_000);
Console.WriteLine($"Saldo setelah tarik: {rekening.Saldo}");
rekening.Tarik(800_000); // Ini akan menyebabkan error karena saldo tidak cukup