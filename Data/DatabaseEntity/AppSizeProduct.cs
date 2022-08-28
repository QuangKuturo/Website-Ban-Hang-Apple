using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("AppSizeProduct")]
    public class AppSizeProduct : DomainEntity<int>
    {
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int? AppSizeId { get; set; }
        [ForeignKey("AppSizeId")]
        public AppSize AppSizes { get; set; }
    }
}
