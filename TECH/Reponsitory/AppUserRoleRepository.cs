using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IAppUserRolesRepository : IRepository<AppUserRoles, int>
    {
       
    }

    public class AppUserRolesRepository : EFRepository<AppUserRoles, int>, IAppUserRolesRepository
    {
        public AppUserRolesRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
