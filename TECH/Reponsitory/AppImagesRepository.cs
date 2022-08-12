using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IAppImagesRepository : IRepository<AppImages, int>
    {
       
    }

    public class AppImagesRepository : EFRepository<AppImages, int>, IAppImagesRepository
    {
        public AppImagesRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
