using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LatihanHari2
{
    public class EmailAddress: Identity
    {
        public string? Email { get; set; }

        public override string TypeOfIdentity()
        {
            return "Email";
        }
    }
}