using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
using static TECH.General.General;

namespace TECH.Data.DatabaseEntity
{
    [Table("Orders")]
   public class Orders: DomainEntity<int>
    {
        //[Key]
        //public int Id { get; set; }
        public int? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customers Customers { get; set; }

        public DateTime? CreateDate { get; set; }
        //public OrdersStatus Status { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? TongTien { get; set; }
        public string TenNguoiNhan { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public int? TrangThai { get; set; }
        public bool IsDeleted { get; set; }
    }
}
