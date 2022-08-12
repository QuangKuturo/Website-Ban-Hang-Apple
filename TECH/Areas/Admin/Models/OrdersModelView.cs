using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class OrdersModelView
    {
        public int Id { get; set; }
        public int? AppUserId { get; set; }

        public int? CustomerId { get; set; }

        public DateTime? CreateDate { get; set; }

        public decimal? TongTien { get; set; }
        public string TenNguoiNhan { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public int? TrangThai { get; set; }
        public bool IsDeleted { get; set; }
    }
}
