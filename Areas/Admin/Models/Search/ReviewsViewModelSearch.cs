using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models.Search
{
    public class ReviewsViewModelSearch : PageViewModel
    {
        public int? star { get; set; }
        public string? name { get; set; }
        
    }
}
