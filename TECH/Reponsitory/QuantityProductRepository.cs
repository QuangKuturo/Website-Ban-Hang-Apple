using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IQuantityProductRepository : IRepository<QuantityProduct, int>
    {
       
    }

    public class QuantityProductRepository : EFRepository<QuantityProduct, int>, IQuantityProductRepository
    {
        public QuantityProductRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
