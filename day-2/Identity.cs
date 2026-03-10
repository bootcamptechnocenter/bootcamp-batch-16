using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace day_2
{
    public class Identity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public Identity(string name, string address, string phoneNumber)
        {
            Name = name;
            Address = address;
            PhoneNumber = phoneNumber;
        }

        public virtual string TypeOfIdentity()
        {
            return "Identity";
        }

        public void DisplayIdentity()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Phone Number: {PhoneNumber}");
        }


    }
}