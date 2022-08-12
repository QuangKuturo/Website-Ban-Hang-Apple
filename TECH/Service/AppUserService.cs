
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
    public interface IAppUserService
    {
        PagedResult<UserModelView> GetAllPaging(UserModelViewSearch userModelViewSearch);
        UserModelView GetByid(int id);
        int Add(UserModelView view);
        bool Update(UserModelView view);
        bool Deleted(int id);
        void Save();
        UserModelView UserLogin(UserModelView userModelView);
        bool IsMailExist(string mail);
        bool IsPhoneExist(string phone);
        bool UpdateStatus(int id, int status);
    }

    public class AppUserService : IAppUserService
    {
        private readonly IUsersRepository _appUserRepository;
        private IUnitOfWork _unitOfWork;
        public AppUserService(IUsersRepository appUserRepository,
            IUnitOfWork unitOfWork)
        {
            _appUserRepository = appUserRepository;
            _unitOfWork = unitOfWork;
        }
        public bool IsMailExist(string mail)
        {
            var data = _appUserRepository.FindAll().Any(p=>p.email == mail);
            return data;
        }
        public bool IsPhoneExist(string phone)
        {
            var data = _appUserRepository.FindAll().Any(p => p.phone_number == phone);
            return data;
        }
        public UserModelView GetByid(int id)
        {
            var data = _appUserRepository.FindAll(p => p.id == id).FirstOrDefault();
            if (data != null)
            {                
                var model = new UserModelView()
                {
                    id = data.id,
                    full_name = data.full_name,
                    avatar = data.avatar,
                    phone_number = data.phone_number,
                    email = data.email,
                    address = data.address,
                    role = data.role,
                    password = data.password,
                    status = data.status,
                    register_date = data.register_date
                };
                return model;
            }
            return null;
        }
        public int Add(UserModelView view)
        {
            try
            {
                if (view != null)
                {
                    var appUser = new Users
                    {
                        full_name = view.full_name,
                        avatar = view.avatar,
                        phone_number  = view.phone_number,
                        email = view.email,
                        address  = view.address,
                        role  = view.role,
                        password   = view.password,  
                        status = 0,
                        register_date = DateTime.Now
                    };
                    _appUserRepository.Add(appUser);
                    
                    Save();

                    return appUser.id;                    
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;

        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public bool Update(UserModelView view)
        {
            try
            {
                var dataServer = _appUserRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.address = view.address;
                    dataServer.role = view.role;
                    _appUserRepository.Update(dataServer);                                        
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
                var dataServer = _appUserRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.status = status;
                    _appUserRepository.Update(dataServer);
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
                var dataServer = _appUserRepository.FindById(id);
                if (dataServer != null)
                {
                    _appUserRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<UserModelView> GetAllPaging(UserModelViewSearch userModelViewSearch)
        {
            try
            {
                var query = _appUserRepository.FindAll();

                if (userModelViewSearch.role.HasValue)
                {
                    query = query.Where(c => c.role == userModelViewSearch.role.Value);
                }
                
                if (!string.IsNullOrEmpty(userModelViewSearch.name))
                {
                    query = query.Where(c => c.full_name == userModelViewSearch.name || c.email == userModelViewSearch.name || c.phone_number == userModelViewSearch.name);
                }

                int totalRow = query.Count();
                query = query.Skip((userModelViewSearch.PageIndex - 1) * userModelViewSearch.PageSize).Take(userModelViewSearch.PageSize);
                var data = query.Select(c => new UserModelView()
                {
                    full_name = (!string.IsNullOrEmpty(c.full_name) ? c.full_name : ""),
                    id = c.id,
                    phone_number = !string.IsNullOrEmpty(c.phone_number) ? c.phone_number : "",
                    email = !string.IsNullOrEmpty(c.email) ? c.email : "",
                    address = !string.IsNullOrEmpty(c.address) ? c.address : "",
                    role = c.role,
                    password = c.password,
                    status = c.status,
                    register_date = c.register_date,
                 }).ToList();
              
                var pagingData = new PagedResult<UserModelView>
                {
                    Results = data,
                    CurrentPage = userModelViewSearch.PageIndex,
                    PageSize = userModelViewSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public UserModelView UserLogin(UserModelView userModelView)
        {
            var data = _appUserRepository.FindAll().Where(u => u.phone_number.Trim().ToLower() == userModelView.phone_number.Trim().ToLower() ||
                                                          u.email == userModelView.email).FirstOrDefault();

            if (data != null && data.password != null && userModelView.password != null &&
                data.password.Trim() == userModelView.password.Trim())
            {
                var dataServer = new UserModelView()
                {
                    id = data.id,
                    full_name = data.full_name,
                };               
                return dataServer;
            }
            return null;
            
        }

        public List<UserModelView> GetAll()
        {
            var data = _appUserRepository.FindAll().Select(c => new UserModelView()
            {
                id = c.id,
                full_name = c.full_name,
                //Code = c.Code
            }).ToList();

            return data;
        }

    }
}
