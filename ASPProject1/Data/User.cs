using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Data
{
    public class User : IdentityUser
    {


        public string FullName { get; set; }
      
        
        public DateTime DateReg { get; set; }
        public virtual ICollection<Messages> Messages2 { get; set; }





    }
}
