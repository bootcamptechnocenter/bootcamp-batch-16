using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace day_2
{
    public class WhatsApp : Identity
    {
        public string WhatsAppNumber { get; set; }
        public WhatsApp(string name, string address, string phoneNumber, string whatsappNumber) : base(name, address, phoneNumber)
        {
            WhatsAppNumber = whatsappNumber;
        }

        public override string TypeOfIdentity()
        {
            return "WhatsApp";
        }
    }
}