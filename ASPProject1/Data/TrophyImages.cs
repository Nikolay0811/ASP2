using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Data
{
    public class TrophyImages
    {
        public TrophyImages()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        [Key]
        public string Id { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public int TrophyId { get; set; }
        public Trophy Trophy { get; set; }
    }
}
