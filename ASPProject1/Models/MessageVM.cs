using ASPProject1.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Models
{
    public class MessageVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Message { get; set; }
        public DateTime DateMess { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
