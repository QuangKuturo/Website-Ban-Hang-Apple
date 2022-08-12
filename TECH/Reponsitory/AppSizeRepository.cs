using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IAppSizeRepository : IRepository<AppSize, int>
    {
       
    }

    public class AppSizeRepository : EFRepository<AppSize, int>, IAppSizeRepository
    {
        public AppSizeRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
