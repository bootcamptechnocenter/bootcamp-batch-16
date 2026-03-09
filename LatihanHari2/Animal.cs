
namespace LatihanHari2
{
    public class Animal
    {
        public string Name { get; set; } = "Kucing";
        public string Food { get; set; } = "Iwak";
        public string Sound { get; set; } = "Meong";
        public bool IsCarnivore { get; set; } = false;

        //constructor
        public Animal(string name, string food, string sound, bool isCarnivore)
        {
            Name = name;
            Food = food;
            Sound = sound;
            IsCarnivore = isCarnivore;
        }

        public void Eat()
        {
            Console.WriteLine($"{Name} makan {Food}, dan mengeluarkan suara {Sound}");
        }

        public void IsCarnivoreOrNot()
        {
            if (IsCarnivore)
            {
                Console.WriteLine($"{Name} adalah hewan karnivora");
            }
            else
            {
                Console.WriteLine($"{Name} bukan hewan karnivora");
            }
        }
    }
}