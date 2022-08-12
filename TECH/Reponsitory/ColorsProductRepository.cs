using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IColorsProductRepository : IRepository<AppSizeProduct, int>
    {
       
    }

    public class ColorsProductRepository : EFRepository<AppSizeProduct, int>, IColorsProductRepository
    {
        public ColorsProductRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
