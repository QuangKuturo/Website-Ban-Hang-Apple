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
    public interface IAppUserService
    {
        PagedResult<UserModelView> GetAllPaging(UserModelViewSearch userModelViewSearch);
        UserModelView GetById(int id);
        int Add(UserModelView view);
        bool Update(UserModelView view);
        bool Deleted(int id);
        void Save();
        UserModelView UserLogin(UserModelView userModelView);
    }

    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IAppUserRolesRepository _appUserRolesRepository;
        private IUnitOfWork _unitOfWork;
        public AppUserService(IAppUserRepository appUserRepository,
            IAppUserRolesRepository appUserRolesRepository,
            IAppRoleRepository appRoleRepository,
            IUnitOfWork unitOfWork)
        {
            _appUserRepository = appUserRepository;
            _appUserRolesRepository = appUserRolesRepository;
            _appRoleRepository = appRoleRepository;
            _unitOfWork = unitOfWork;
        }
        public UserModelView GetById(int id)
        {
            var data = _appUserRepository.FindAll(p => p.Id == id).FirstOrDefault();
            var lstRoles = new List<RoleModelView>();
            if (data != null)
            {
                var lstUserRole =  _appUserRolesRepository.FindAll().Where(u => u.AppUserId == id).ToList();
                if (lstUserRole != null && lstUserRole.Count > 0)
                {
                    foreach (var itemRole in lstUserRole)
                    {
                        var role = _appRoleRepository.FindAll().Where(r => r.Id == itemRole.AppRoleId && r.IsDeleted != true).Select(r => new RoleModelView()
                        {
                            Id = r.Id,
                            Name = r.Name
                        }).FirstOrDefault();
                        if (role != null)
                        {
                            lstRoles.Add(role);
                        }
                    }

                }
                var model = new UserModelView()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Phone = data.Phone,
                    Email = data.Email,
                    UserName = data.UserName,
                    Birthday = data.Birthday,
                    Address = data.Address,
                    Avartar = data.Avartar,
                    PassWord = data.PassWord,
                    Roles = lstRoles,
                    RolesStr = string.Join(',', lstRoles.Select(r => r.Name).ToList()),
                    RolesArrary = lstRoles.Select(r => r.Id).ToList()
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
                    var appUser = new AppUser
                    {
                        Name = view.Name,
                        Phone = view.Phone,
                        Email  = view.Email,
                        UserName = view.UserName,
                        Birthday  = view.Birthday,
                        Address  = view.Address,
                        Avartar   = view.Avartar,  
                        PassWord = view.PassWord
                    };
                    _appUserRepository.Add(appUser);
                    
                    Save();

                    if (view.RolesArrary != null && view.RolesArrary.Count > 0)
                    {
                        foreach (var item in view.RolesArrary)
                        {
                            var _appUserRole = new AppUserRoles()
                            {
                                AppUserId = appUser.Id,
                                AppRoleId = Convert.ToInt32(item)
                            };
                            _appUserRolesRepository.Add(_appUserRole);
                            Save();
                        }

                    }

                    return appUser.Id;                    
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
                var dataServer = _appUserRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.Name = view.Name;
                    dataServer.Phone = view.Phone;
                    dataServer.Email = view.Email;
                    dataServer.UserName = view.UserName;
                    dataServer.Birthday = view.Birthday;
                    dataServer.Address = view.Address;
                    dataServer.Avartar = view.Avartar;
                    dataServer.PassWord = view.PassWord;
                    _appUserRepository.Update(dataServer);
                    var dataServerRoles = new List<AppUserRoles>();

                    dataServerRoles = _appUserRolesRepository.FindAll().Where(r => r.AppUserId == dataServer.Id).ToList();
                    if (dataServerRoles != null && dataServerRoles.Count > 0)
                    {
                        _appUserRolesRepository.RemoveMultiple(dataServerRoles);
                    }

                    if (view.RolesArrary != null && view.RolesArrary.Count > 0)
                    {
                        foreach (var item in view.RolesArrary)
                        {
                            var userRole = new AppUserRoles()
                            {
                                AppUserId = dataServer.Id,
                                AppRoleId = item
                            };
                            _appUserRolesRepository.Add(userRole);
                        }
                    }

                    
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
                    dataServer.IsDeleted = true;
                    _appUserRepository.Update(dataServer);
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
                var query = _appUserRepository.FindAll().Where(u=>!u.IsDeleted);

                if (!string.IsNullOrEmpty(userModelViewSearch.Name))
                {
                    query = query.Where(c => c.Name.ToLower() == userModelViewSearch.Name.ToLower());
                }

                int totalRow = query.Count();
                query = query.Skip((userModelViewSearch.PageIndex - 1) * userModelViewSearch.PageSize).Take(userModelViewSearch.PageSize);
                var data = query.Select(c => new UserModelView()
                {
                    Name = (!string.IsNullOrEmpty(c.Name) ? c.Name:""),
                    Id = c.Id,
                    Phone = !string.IsNullOrEmpty(c.Phone) ? c.Phone : "",
                    Email = !string.IsNullOrEmpty(c.Email) ? c.Email : "",
                    UserName = !string.IsNullOrEmpty(c.UserName) ? c.UserName : "",
                    Birthday = c.Birthday,
                    BirthdayStr = c.Birthday.HasValue ? c.Birthday.Value.ToString("dd/MM/yyyy"):"",
                    Address = !string.IsNullOrEmpty(c.Address) ? c.Address : "",                    
                    Avartar = c.Avartar,
                 }).ToList();

                if (data != null && data.Count > 0)
                {
                    var lstRoles = new List<RoleModelView>();
                    foreach (var item in data)
                    {
                        var lstUserRole = _appUserRolesRepository.FindAll().Where(u => u.AppUserId == item.Id).ToList();
                        if (lstUserRole != null && lstUserRole.Count > 0)
                        {
                            foreach (var itemRole in lstUserRole)
                            {
                                var role = _appRoleRepository.FindAll().Where(r => r.Id == itemRole.AppRoleId && r.IsDeleted != true).Select(r=>new RoleModelView() { 
                                    Id = r.Id,
                                    Name = r.Name                                
                                }).FirstOrDefault();
                                if (role != null)
                                {
                                    lstRoles.Add(role);
                                }
                            }
                            item.RolesStr = string.Join(',',lstRoles.Select(r=>r.Name).ToList());
                            lstRoles = new List<RoleModelView>();
                        }
                        else
                        {
                            item.RolesStr = "";
                        }

                    }
                }

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
            var data = _appUserRepository.FindAll(u=>u.IsDeleted != true).Where(u => u.UserName.Trim().ToLower() == userModelView.UserName.Trim().ToLower() ||
                                                          u.Email == userModelView.Email).FirstOrDefault();

            if (data != null && data.PassWord != null && userModelView.PassWord != null &&
                data.PassWord.Trim() == userModelView.PassWord.Trim())
            {
                var dataServer = new UserModelView()
                {
                    Id = data.Id,
                    Name = data.Name,
                    UserName = data.UserName,
                    Avartar = data.Avartar,
                };
                var lstRoles = new List<RoleModelView>();
                var lstUserRole = _appUserRolesRepository.FindAll().Where(u => u.AppUserId == data.Id).ToList();
                if (lstUserRole != null && lstUserRole.Count > 0)
                {
                    foreach (var itemRole in lstUserRole)
                    {
                        var role = _appRoleRepository.FindAll().Where(r => r.Id == itemRole.AppRoleId && r.IsDeleted != true).Select(r => new RoleModelView()
                        {
                            Id = r.Id,
                            Name = r.Name
                        }).FirstOrDefault();
                        if (role != null)
                        {
                            lstRoles.Add(role);
                        }
                    }
                    dataServer.Roles = lstRoles;
                }
                return dataServer;
            }
            return null;
            
        }

        public List<UserModelView> GetAll()
        {
            var data = _appUserRepository.FindAll().Select(c => new UserModelView()
            {
                Id = c.Id,
                Name = c.Name,
                //Code = c.Code
            }).ToList();

            return data;
        }

    }
}
