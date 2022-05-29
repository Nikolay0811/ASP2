using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Models
{
    public class NewsImagesVM
    {
        public NewsImagesVM()
        {
            this.Id = Guid.NewGuid().ToString();
        }

            [Key]
            public string Id { get; set; }
            [Required]
            public int NewsId { get; set; }
            public List<SelectListItem> News { get; set; }
            [Required]
            public IFormFile ImagePath { get; set; }
    }
}


