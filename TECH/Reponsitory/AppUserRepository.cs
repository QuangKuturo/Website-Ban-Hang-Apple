using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IAppUserRepository : IRepository<AppUser, int>
    {
       
    }

    public class AppUserRepository : EFRepository<AppUser, int>, IAppUserRepository
    {
        public AppUserRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
