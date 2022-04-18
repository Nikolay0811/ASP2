using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Data
{
    public class NewsImages
    {
        public NewsImages()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }
        [Required]
        public string ImagePath { get; set; }

        //wrazka M:1
        [Required]
        //[ForeignKey("Product")]
        public int NewsId { get; set; }

        public News News{ get; set; }
            
    }
}
