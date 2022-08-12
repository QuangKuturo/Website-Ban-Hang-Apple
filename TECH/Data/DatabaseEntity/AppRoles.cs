using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("AppRoles")]
    public class AppRoles: DomainEntity<int>
    {
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
