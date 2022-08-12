using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IOrderDetailService
    {
        PagedResult<OrdersDetailModelView> GetAllPaging();

        OrdersDetailModelView GetById(int id);

        bool Add(OrdersDetailModelView view);

        bool Update(OrdersDetailModelView view);

        bool Deleted(int id);

        void Save();
    }
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private IUnitOfWork _unitOfWork;
        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            _orderDetailRepository = orderDetailRepository;
            _unitOfWork = unitOfWork;
        }
        

        public OrdersDetailModelView GetById(int id)
        {
            var data = _orderDetailRepository.FindById(id);
            if (data != null && data.IsDeleted != true)
            {
                var model = new OrdersDetailModelView()
                {
                    Id = data.Id,
                    ProductId = data.ProductId,
                    AppSizeId = data.AppSizeId,
                    OrdersId = data.OrdersId,
                    SoLuong = data.SoLuong,
                    IsDeleted = data.IsDeleted,
                    ThanhTien = data.ThanhTien,
                };
                return model;
            }
            return null;
        }

        public bool Add(OrdersDetailModelView view)
        {
            try
            {
                if (view != null)
                {
                    var _order = new OrdersDetail
                    {
                        ProductId = view.ProductId,
                        AppSizeId = view.AppSizeId,
                        OrdersId = view.OrdersId,
                        SoLuong = view.SoLuong,
                        IsDeleted = view.IsDeleted,
                        ThanhTien = view.ThanhTien,
                    };
                    _orderDetailRepository.Add(_order);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //return false;
            }

            return false;

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }

        public bool Update(OrdersDetailModelView view)
        {
            try
            {
                var dataServer = _orderDetailRepository.FindById(view.Id);
                if (dataServer != null && dataServer.IsDeleted != true)
                {
                    dataServer.Id = view.Id;
                    dataServer.ProductId = view.ProductId;
                    dataServer.AppSizeId = view.AppSizeId;
                    dataServer.OrdersId = view.OrdersId;
                    dataServer.SoLuong = view.SoLuong;
                    dataServer.IsDeleted = view.IsDeleted;
                    dataServer.ThanhTien = view.ThanhTien;
                    _orderDetailRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _orderDetailRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.IsDeleted = true;
                    _orderDetailRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<OrdersDetailModelView> GetAllPaging()
        {
            try
            {
                var query = _orderDetailRepository.FindAll(c=>c.IsDeleted != true);                                
                int totalRow = query.Count();
                //query = query.Skip((orderViewModelSearch.PageIndex - 1) * orderViewModelSearch.PageSize).Take(orderViewModelSearch.PageSize);
                var data = query.Select(c => new OrdersDetailModelView()
                {
                    Id = c.Id,
                    ProductId = c.ProductId,
                    AppSizeId = c.AppSizeId,
                    OrdersId = c.OrdersId,
                    SoLuong = c.SoLuong,
                    IsDeleted = c.IsDeleted,
                    ThanhTien = c.ThanhTien,
                }).OrderByDescending(c=>c.Id).ToList();
                var pagingData = new PagedResult<OrdersDetailModelView>
                {
                    Results = data,
                    //CurrentPage = orderViewModelSearch.PageIndex,
                    //PageSize = orderViewModelSearch.PageSize,
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
