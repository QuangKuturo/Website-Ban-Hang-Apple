using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("AppUserRoles")]
    public class AppUserRoles : DomainEntity<int>
    {
        public int? AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public int? AppRoleId { get; set; }
        [ForeignKey("AppRoleId")]
        public AppRoles AppRoles { get; set; }
    }
}
