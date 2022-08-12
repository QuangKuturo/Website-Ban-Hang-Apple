using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("AppSize")]
    public class AppSize: DomainEntity<int>
    {
        public string Description { get; set; } = null;
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
