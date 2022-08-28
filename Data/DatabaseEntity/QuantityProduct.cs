using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Data.DatabaseEntity
{
    [Table("QuantityProduct")]
   public class QuantityProduct : DomainEntity<int>
    {
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int? AppSizeId { get; set; }
        [ForeignKey("AppSizeId")]
        public AppSize AppSizes { get; set; }
        public int? TotalImported { get; set; }
        public int? TotalSold { get; set; }
        public int? TotalStock { get; set; }
        public DateTime? DateImport { get; set; }
        public bool IsDeleted { get; set; }
    }
}
