using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface ICustomersRepository : IRepository<Customers, int>
    {
       
    }

    public class CustomersRepository : EFRepository<Customers, int>, ICustomersRepository
    {
        public CustomersRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
