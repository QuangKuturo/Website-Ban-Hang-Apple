
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IOrdersService
    {
        PagedResult<OrdersModelView> GetAllPaging(OrdersViewModelSearch OrdersModelViewSearch);
        OrdersModelView GetByid(int id);
        bool Add(OrdersModelView view);
        bool Update(OrdersModelView view);
        bool Deleted(int id);
        void Save();
        bool UpdateStatus(int id, int status);
    }

    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private IUnitOfWork _unitOfWork;
        public OrdersService(IOrdersRepository ordersRepository,
            IUnitOfWork unitOfWork)
        {
            _ordersRepository = ordersRepository;
            _unitOfWork = unitOfWork;
        }
        public OrdersModelView GetByid(int id)
        {
            var data = _ordersRepository.FindAll(p => p.id == id).FirstOrDefault();
            if (data != null)
            {                
                var model = new OrdersModelView()
                {
                    id = data.id,
                    user_id = data.user_id,
                    note = data.note,
                    review = data.review,
                    payment = data.payment,
                    status = data.status,
                    total = data.total,
                    fee_ship = data.fee_ship,
                    created_at = data.created_at,
                };
                return model;
            }
            return null;
        }
        public bool Add(OrdersModelView view)
        {
            try
            {
                if (view != null)
                {
                    var products = new Orders
                    {
                        user_id = view.user_id,
                        note = view.note,
                        review = view.review,
                        payment = view.payment,
                        status = view.status,
                        total = view.total,
                        fee_ship = view.fee_ship,
                        created_at = DateTime.Now,
                    };
                    _ordersRepository.Add(products);

                    return true;                    
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(OrdersModelView view)
        {
            try
            {
                var dataServer = _ordersRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.note = view.note;
                    dataServer.review = view.review;
                    dataServer.payment = view.payment;
                    dataServer.status = view.status;
                    dataServer.total = view.total;
                    dataServer.fee_ship = view.fee_ship;
                    dataServer.created_at = DateTime.Now;
                    _ordersRepository.Update(dataServer);                                        
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

      public  bool UpdateStatus(int id, int status)
        {
            try
            {
                var dataServer = _ordersRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.status = status;
                    _ordersRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _ordersRepository.FindById(id);
                if (dataServer != null)
                {
                    _ordersRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<OrdersModelView> GetAllPaging(OrdersViewModelSearch OrdersModelViewSearch)
        {
            try
            {
                var query = _ordersRepository.FindAll();

                if (OrdersModelViewSearch.payment.HasValue)
                {
                    query = query.Where(c => c.payment == OrdersModelViewSearch.payment.Value);
                }
                
                if (!string.IsNullOrEmpty(OrdersModelViewSearch.name))
                {
                    query = query.Where(c => c.code == OrdersModelViewSearch.name);
                }

                int totalRow = query.Count();
                query = query.Skip((OrdersModelViewSearch.PageIndex - 1) * OrdersModelViewSearch.PageSize).Take(OrdersModelViewSearch.PageSize);
                var data = query.Select(p => new OrdersModelView()
                {
                    id = p.id,
                    user_id = p.user_id,
                    note = p.note,
                    review = p.review,
                    payment = p.payment,
                    status = p.status,
                    total = p.total,
                    totalstr = p.total.HasValue && p.total.Value > 0? p.total.Value.ToString("#,###"):"",
                    fee_ship = p.fee_ship,
                    created_atstr = p.created_at.HasValue ? p.created_at.Value.ToString("hh:mm") + " - " + p.created_at.Value.ToString("dd/MM/yyyy") : "",
                }).ToList();
              
                var pagingData = new PagedResult<OrdersModelView>
                {
                    Results = data,
                    CurrentPage = OrdersModelViewSearch.PageIndex,
                    PageSize = OrdersModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }       
    }
}
