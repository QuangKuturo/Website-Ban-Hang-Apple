using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class ProductModelView
    {
        public int id { get; set; }
        public int? category_id { get; set; }
        public string? categorystr { get; set; }
        public string? name { get; set; }

        public string? slug { get; set; }

        public string? avatar { get; set; }

        public string? price { get; set; }

        public string? color { get; set; }

        public string? quantity { get; set; }

        public string? short_desc { get; set; }

        public string? description { get; set; }

        public string? specifications { get; set; }

        public string? endow { get; set; }

        public int? status { get; set; }

        public int? type { get; set; }
        public int? differentiate { get; set; }
    }
   
}
