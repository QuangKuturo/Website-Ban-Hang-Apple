using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IProductRepository : IRepository<Product, int>
    {
       
    }

    public class ProductRepository : EFRepository<Product, int>, IProductRepository
    {
        public ProductRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
