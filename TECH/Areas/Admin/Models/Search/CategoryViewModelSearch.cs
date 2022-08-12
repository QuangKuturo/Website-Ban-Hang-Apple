using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models.Search
{
    public class CategoryViewModelSearch: PageViewModel
    {
        public int? CategoryId { get; set; }
        public int? CategoryParentId { get; set; }
    }
}
