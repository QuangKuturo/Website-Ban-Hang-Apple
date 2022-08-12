using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class OrdersDetailModelView 
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? AppSizeId { get; set; }
        public int? OrdersId { get; set; }
        public int SoLuong { get; set; }
        public bool IsDeleted { get; set; }
        public decimal? ThanhTien { get; set; }
    }
}
