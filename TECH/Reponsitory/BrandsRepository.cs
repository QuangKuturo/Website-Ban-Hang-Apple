using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IBrandsRepository : IRepository<Brands, int>
    {
       
    }

    public class BrandsRepository : EFRepository<Brands, int>, IBrandsRepository
    {
        public BrandsRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
