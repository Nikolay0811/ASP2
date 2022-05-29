using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Models
{
    public class NewsDetailsVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Text { get; set; }
        public string Fotos { get; set; }
        [Required(ErrorMessage = "Избери снимка от компютъра си...")]
        public DateTime Data { get; set; }     
        public List<string> ImagesPaths { get; internal set; }
    }
}
