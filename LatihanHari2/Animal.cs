using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanHari2
{
    public class Animal
    {
        public string Name { get; set; }
        public string Food { get; set; }
        public string Sound { get; set; }
        public bool IsCarnivore { get; set; } = false;

        public Animal(string name, string food, string sound, bool isCarnivore)
        {
            Name = name;
            Food = food;
            Sound = sound;
            IsCarnivore = isCarnivore;
        }

        public void Eat()
        {
            Console.WriteLine($"{Name} is eating {Food} and makes a sound {Sound}");
        }

        public void IsCarnivoreOrNot()
        {
            if (IsCarnivore)
            {
                Console.WriteLine($"{Name} is a carnivore");
            }
            else
            {
                Console.WriteLine($"{Name} is not a carnivore");
            }
        }
    }
}