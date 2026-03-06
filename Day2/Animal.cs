using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Day2
{
    public class Animal
    {
        public required string Name { get; set; }
        public string Food { get; set; } = "Unknown";
        public string Sound { get; set; } = "Unknown";
        public bool IsCarnivore { get; set; } = false;

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