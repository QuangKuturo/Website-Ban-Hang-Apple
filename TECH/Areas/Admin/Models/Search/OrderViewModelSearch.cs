using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models.Search
{
    public class OrderViewModelSearch : PageViewModel
    {
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
