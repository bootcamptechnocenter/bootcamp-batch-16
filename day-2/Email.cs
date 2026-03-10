using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace day_2
{
    public class Email : Identity
    {
        public string EmailAddress { get; set; }

        public Email(string name, string address, string phoneNumber, string emailAddress) : base(name, address, phoneNumber)
        {
            EmailAddress = emailAddress;
        }

        public override string TypeOfIdentity()
        {
            return "Email";
        }
    }
}