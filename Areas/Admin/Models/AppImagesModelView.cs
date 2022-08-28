using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class AppImagesModelView
    {
        public int AppImageId { get; set; }
        public int? ProductId { get; set; }
        public string Url { get; set; }
        public string Alt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
