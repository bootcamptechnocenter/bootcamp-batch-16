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

