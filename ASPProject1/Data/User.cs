using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Data
{
    public class User : IdentityUser
    {


        public string Fullname { get; set; }

        public string Message { get; set; }
        public DateTime DateReg { get; set; }
        public DateTime DateMess { get; set; }

        public RoleType Role { get; set; }

        

    }
}
