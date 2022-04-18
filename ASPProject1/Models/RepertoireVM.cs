using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Models
{
    public class RepertoireVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Video { get; set; }
        [Required(ErrorMessage = "Избери Видео от компютъра си...")]
        public DateTime Data { get; set; }
    }
}
