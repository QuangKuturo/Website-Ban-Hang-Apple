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
    public interface IOrderService
    {
        PagedResult<OrdersModelView> GetAllPaging(OrderViewModelSearch orderViewModelSearch);

        OrdersModelView GetById(int id);

        bool Add(OrdersModelView view);

        bool Update(OrdersModelView view);

        bool Deleted(int id);

        void Save();
    }
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private IUnitOfWork _unitOfWork;
        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }
        

        public OrdersModelView GetById(int id)
        {
            var data = _orderRepository.FindById(id);
            if (data != null && data.IsDeleted != true)
            {
                var model = new OrdersModelView()
                {
                    Id = data.Id,
                    AppUserId = data.AppUserId,
                    CustomerId = data.CustomerId,
                    CreateDate = data.CreateDate,
                    TongTien = data.TongTien,
                    TenNguoiNhan = data.TenNguoiNhan,
                    SDT = data.SDT,
                    DiaChi = data.DiaChi,
                    TrangThai = data.TrangThai,
                    IsDeleted = data.IsDeleted,
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
                    var _order = new Orders
                    {
                        AppUserId = view.AppUserId,
                        CustomerId = view.CustomerId,
                        CreateDate = view.CreateDate,
                        TongTien = view.TongTien,
                        TenNguoiNhan = view.TenNguoiNhan,
                        SDT = view.SDT,
                        DiaChi = view.DiaChi,
                        TrangThai = view.TrangThai,
                        IsDeleted = view.IsDeleted,
                    };
                    _orderRepository.Add(_order);
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

        public bool Update(OrdersModelView view)
        {
            try
            {
                var dataServer = _orderRepository.FindById(view.Id);
                if (dataServer != null && dataServer.IsDeleted != true)
                {
                    dataServer.Id = view.Id;
                    dataServer.AppUserId = view.AppUserId;
                    dataServer.CustomerId = view.CustomerId;
                    dataServer.CreateDate = view.CreateDate;
                    dataServer.TongTien = view.TongTien;
                    dataServer.TenNguoiNhan = view.TenNguoiNhan;
                    dataServer.SDT = view.SDT;
                    dataServer.DiaChi = view.DiaChi;
                    dataServer.TrangThai = view.TrangThai;
                    dataServer.IsDeleted = view.IsDeleted;
                    _orderRepository.Update(dataServer);
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
                var dataServer = _orderRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.IsDeleted = true;
                    _orderRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<OrdersModelView> GetAllPaging(OrderViewModelSearch orderViewModelSearch)
        {
            try
            {
                var query = _orderRepository.FindAll(c=>c.IsDeleted != true);
                if (orderViewModelSearch.Start.HasValue && !orderViewModelSearch.End.HasValue)
                {
                    query = query.Where(o => o.CreateDate >= orderViewModelSearch.Start);
                }
                else if (!orderViewModelSearch.Start.HasValue && orderViewModelSearch.End.HasValue)
                {
                    query = query.Where(o => o.CreateDate <= orderViewModelSearch.End);
                }
                else
                {
                    query = query.Where(o => o.CreateDate >= orderViewModelSearch.Start && o.CreateDate >= orderViewModelSearch.End);
                }
                
                int totalRow = query.Count();
                query = query.Skip((orderViewModelSearch.PageIndex - 1) * orderViewModelSearch.PageSize).Take(orderViewModelSearch.PageSize);
                var data = query.Select(c => new OrdersModelView()
                {
                    Id = c.Id,
                    AppUserId = c.AppUserId,
                    CustomerId = c.CustomerId,
                    CreateDate = c.CreateDate,
                    TongTien = c.TongTien,
                    TenNguoiNhan = c.TenNguoiNhan,
                    SDT = c.SDT,
                    DiaChi = c.DiaChi,
                    TrangThai = c.TrangThai,
                    IsDeleted = c.IsDeleted
            }).OrderByDescending(c=>c.Id).ToList();
                var pagingData = new PagedResult<OrdersModelView>
                {
                    Results = data,
                    CurrentPage = orderViewModelSearch.PageIndex,
                    PageSize = orderViewModelSearch.PageSize,
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
