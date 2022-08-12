using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IOrderRepository : IRepository<Orders, int>
    {
       
    }

    public class OrderRepository : EFRepository<Orders, int>, IOrderRepository
    {
        public OrderRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
