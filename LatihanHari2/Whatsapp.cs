using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanHari2
{
    public class Whatsapp : Identity
    {
        public string WhatsappNumber { get; set; }
        public override string TypeOfIdentity()
        {
            return "Whatsapp";
        }
    }
}