using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class UserModelView
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
        public bool RememberMe { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime? Birthday { get; set; }
        public string BirthdayStr { get; set; }

        public string City { get; set; }
        public string Avartar { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsDeleted { get; set; }
        public List<RoleModelView> Roles { get; set; }
        public string RolesStr { get; set; }
        public List<int> RolesArrary { get; set; }
    }
}
