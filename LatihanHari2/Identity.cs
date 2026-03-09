using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanHari2
{
    public class Identity
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }


        public virtual string TypeOfIdentity()
        {
            return "Identity";
        }

        public void DisplayIdentity()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Phone: {Phone}");
        }
    }
}