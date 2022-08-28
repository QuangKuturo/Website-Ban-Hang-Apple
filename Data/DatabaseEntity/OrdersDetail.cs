using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("OrdersDetail")]
   public class OrdersDetail: DomainEntity<int>
    {
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int? OrdersId { get; set; }
        [ForeignKey("OrdersId")]
        public Orders Orders { get; set; }
        public int? AppSizeId { get; set; }
        [ForeignKey("AppSizeId")]
        public AppSize AppSize { get; set; }
        public int SoLuong { get; set; }
        public bool IsDeleted { get; set; }
        [Column(TypeName = "decimal")]
        public decimal? ThanhTien { get; set; }
   }
}
