using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanHari2
{
    public class WhatsApp: Identity
    {
        public string WhatsAppNumber { get; set; }

        public override string TypeOfIdentity()
        {
            return "WhatsApp";
        }
    }
}