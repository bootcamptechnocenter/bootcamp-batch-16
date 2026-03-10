using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanHari2
{
    public class Email: Identity
    {
        public string? EmailAddress { get; set; }

        public override string TypeOfIdentity()
        {
            return "email";
        }
    }
}