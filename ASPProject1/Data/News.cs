using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPProject1.Data
{
    public class News
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }
        
        public DateTime Data { get; set; }

        public ICollection<NewsImages> NewsImages { get; set; }
    }
}
