using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Data
{
    public class Messages
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateMess { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
