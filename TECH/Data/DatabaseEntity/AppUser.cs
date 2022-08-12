using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;
using static TECH.General.General;

namespace TECH.Data.DatabaseEntity
{
    [Table("AppUser")]
   public class AppUser: DomainEntity<int>
    {
        //[Key]
        //public int Id { get; set; }        
        public string Name { get; set; }
        [Column(TypeName ="varchar(12)")]        
        public string Phone { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Address { get; set; }
        public string Avartar { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string PassWord { get; set; }
        public bool IsDeleted { get; set; }
   }
}
