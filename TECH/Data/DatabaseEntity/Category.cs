using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECH.SharedKernel;

namespace TECH.Data.DatabaseEntity
{
    [Table("Category")]
    public class Category: DomainEntity<int>
    {
        //[Key]
        //public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = null;
        public bool IsDeleted { get; set; }
    }
}
