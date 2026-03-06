using Day2;

int nilai = 80;

void printNilai(string nilai)
{
    Console.WriteLine($"Nilai kamu adalah {nilai}");
}

// IF ELSE

if (nilai >= 90)
{
    printNilai("A");
}
else if (nilai >= 80)
{
    printNilai("B");
}
else if (nilai >= 70)
{
    printNilai("C");
}
else if (nilai >= 60)
{
    printNilai("D");
}
else
{
    printNilai("E");
}

string result = nilai >= 90 ? "A" : nilai >= 80 ? "B" : nilai >= 70 ? "C" : nilai >= 60 ? "D" : "E";

printNilai(result);

// SWITCH CASE

switch (nilai)
{
    case >= 90:
        printNilai("A");
        break;
    case >= 80:
        printNilai("B");
        break;
    case >= 70:
        printNilai("C");
        break;
    case >= 60:
        printNilai("D");
        break;
    default:
        printNilai("E");
        break;
}

string resultSwitch = nilai switch
{
    >= 90 => "A",
    >= 80 => "B",
    >= 70 => "C",
    >= 60 => "D",
    _ => "E"
};

printNilai(resultSwitch);

// LOOP

for (int i = 0; i < 5; i++)
{
    Console.WriteLine($"Perulangan ke-{i}");
}

// LOOP PRIME NUMBER

for (int i = 2; i <= 10; i++)
{
    bool isPrime = true;

    for (int j = 2; j <= Math.Sqrt(i); j++)
    {
        if (i % j == 0)
        {
            isPrime = false;
            break;
        }
    }

    if (isPrime)
    {
        Console.WriteLine($"{i} adalah bilangan prima");
    }
}

// TANPA SQRT
for (int i = 2; i <= 10; i++)
{
    bool isPrime = true;

    for (int j = 2; j < i; j++)
    {
        if (i % j == 0)
        {
            isPrime = false;
            break;
        }
    }

    if (isPrime)
    {
        Console.WriteLine($"{i} adalah bilangan prima");
    }
}

// FOR EACH
var car = new List<string> { "Toyota", "Honda", "Suzuki" };

foreach (var item in car)
{
    Console.WriteLine(item);
}

// OBJECT

Animal cat = new Animal(
    "Cat",
    "Fish",
    "Meow",
    true
);

cat.Eat();
cat.IsCarnivoreOrNot();

var animals = new List<Animal>
{
    new Animal("Dog", "Meat", "Woof", true),
    new Animal("Cow", "Grass", "Moo", false),
    new Animal("Lion", "Meat", "Roar", true)
};

foreach (var animal in animals)
{
    animal.Eat();
    animal.IsCarnivoreOrNot();
}
