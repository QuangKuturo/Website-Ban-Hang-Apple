using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IAppRoleRepository : IRepository<AppRoles, int>
    {
       
    }

    public class AppRoleRepository : EFRepository<AppRoles, int>, IAppRoleRepository
    {
        public AppRoleRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
