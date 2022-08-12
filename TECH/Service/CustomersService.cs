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
    public interface ICustomersService
    {
        PagedResult<CustomersModelView> GetAllPaging(CustomersModelViewSearch categoryViewModelSearch);
        CustomersModelView GetById(int id);
        int Add(CustomersModelView view);
        bool Update(CustomersModelView view);
        bool Deleted(int id);
        void Save();
        CustomersModelView UserLogin(CustomersModelView userModelView);
    }
    public class CustomersService : ICustomersService
    {
        private readonly ICustomersRepository _customersRepository;
        private IUnitOfWork _unitOfWork;
        public CustomersService(ICustomersRepository customersRepository, IUnitOfWork unitOfWork)
        {
            _customersRepository = customersRepository;
            _unitOfWork = unitOfWork;
        }
       
        public CustomersModelView GetById(int id)
        {
            var data = _customersRepository.FindById(id);
            if (data != null && data.IsDeleted != true)
            {
                var model = new CustomersModelView()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Phone = data.Phone,
                    Email = data.Email,
                    TenDangNhap = data.TenDangNhap,
                    NgaySinh = data.NgaySinh,
                    Address = data.Address,
                    MatKhau = data.MatKhau,
                    GioiTinh = data.GioiTinh
                };
                return model;
            }
            return null;
        }
        public CustomersModelView UserLogin(CustomersModelView userModelView)
        {
            var data = _customersRepository.FindAll(u => u.IsDeleted != true).Where(u => u.TenDangNhap.Trim().ToLower() == userModelView.TenDangNhap.Trim().ToLower() ||
                                                            u.Email == userModelView.Email).FirstOrDefault();

            if (data != null && data.MatKhau != null && userModelView.MatKhau != null &&
                data.MatKhau.Trim() == userModelView.MatKhau.Trim())
            {
                var dataServer = new CustomersModelView()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Phone =data.Phone,
                    Email =data.Email,
                    GioiTinh =data.GioiTinh,
                    NgaySinh = data.NgaySinh,
                    Address =data.Address
                };                
                return dataServer;
            }
            return null;

        }
        public int Add(CustomersModelView view)
        {
            try
            {
                if (view != null)
                {
                    var _customers = new Customers
                    {
                        Name = view.Name,
                        Phone = view.Phone,
                        Email = view.Email,
                        TenDangNhap = view.TenDangNhap,
                        NgaySinh = view.NgaySinh,
                        Address = view.Address,
                        MatKhau = view.MatKhau,
                        GioiTinh = view.GioiTinh
                    };
                    _customersRepository.Add(_customers);
                    Save();
                    return _customers.Id;
                }
            }
            catch (Exception ex)
            {
                //return false;
            }

            return 0;

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(CustomersModelView view)
        {
            try
            {
                var dataServer = _customersRepository.FindById(view.Id);
                if (dataServer != null && dataServer.IsDeleted != true)
                {
                    dataServer.Id = view.Id;
                    dataServer.Name = view.Name;
                    dataServer.Phone = view.Phone;
                    dataServer.Email = view.Email;
                    dataServer.TenDangNhap = view.TenDangNhap;
                    dataServer.NgaySinh = view.NgaySinh;
                    dataServer.Address = view.Address;
                    dataServer.MatKhau = view.MatKhau;
                    dataServer.GioiTinh = view.GioiTinh;
                    _customersRepository.Update(dataServer);
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
                var dataServer = _customersRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.IsDeleted = true;
                    _customersRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<CustomersModelView> GetAllPaging(CustomersModelViewSearch customersModelViewSearch)
        {
            try
            {
                var query = _customersRepository.FindAll(c => c.IsDeleted != true);
               

                int totalRow = query.Count();
                query = query.Skip((customersModelViewSearch.PageIndex - 1) * customersModelViewSearch.PageSize).Take(customersModelViewSearch.PageSize);
                var data = query.Select(c => new CustomersModelView()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone,
                    Email = c.Email,
                    TenDangNhap = c.TenDangNhap,
                    NgaySinh = c.NgaySinh,
                    Address = c.Address,
                    MatKhau = c.MatKhau,
                    GioiTinh = c.GioiTinh,
            }).OrderByDescending(c => c.Id).ToList();
                var pagingData = new PagedResult<CustomersModelView>
                {
                    Results = data,
                    CurrentPage = customersModelViewSearch.PageIndex,
                    PageSize = customersModelViewSearch.PageSize,
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
