using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
namespace TECH.Data.DatabaseEntity
{
    [Table("ImagesProduct")]
   public class ImagesProduct: DomainEntity<int>
    {
        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int? AppImageId { get; set; }
        [ForeignKey("AppImageId")]
        public AppImages AppImages { get; set; }
    }
}
