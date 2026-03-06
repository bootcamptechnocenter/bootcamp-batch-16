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