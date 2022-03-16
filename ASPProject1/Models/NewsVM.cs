using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Models
{
    public class NewsVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage="This field is required")]
        public string Name { get; set; }
        public string Text { get; set; }
        public string Fotos { get; set; }
        public DateTime Data { get; set; }
    }
}
