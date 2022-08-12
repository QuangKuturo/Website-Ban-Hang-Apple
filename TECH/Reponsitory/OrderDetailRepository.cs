using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IOrderDetailRepository : IRepository<OrdersDetail, int>
    {
       
    }

    public class OrderDetailRepository : EFRepository<OrdersDetail, int>, IOrderDetailRepository
    {
        public OrderDetailRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
