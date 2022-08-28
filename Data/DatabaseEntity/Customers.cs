using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("Customers")]
   public class Customers: DomainEntity<int>
    {        
        public string Name { get; set; }
        [Column(TypeName ="varchar(12)")]
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string MatKhau { get; set; }
        public string TenDangNhap { get; set; }
        public string GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool IsDeleted { get; set; }
    }
}
