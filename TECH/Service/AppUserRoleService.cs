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
    // thêm
    public interface IAppUserRoleService
    {
       //List<AppUserRoles> GetImagesByProductId(int productId);

       void AddAppUserRole(int userId, int[] rodeId);
       
       void Save();
    }

    public class AppUserRoleService : IAppUserRoleService
    {
        private readonly IAppUserRolesRepository _appUserRolesRepository;        
        private IUnitOfWork _unitOfWork;

        public AppUserRoleService(IAppUserRolesRepository appUserRolesRepository,
            IUnitOfWork unitOfWork)
        {
            _appUserRolesRepository = appUserRolesRepository;
            _unitOfWork = unitOfWork;
        }
       
        public void AddAppUserRole(int userId, int[] rodeId)
        {
            try
            {                
                foreach (var item in rodeId)
                {
                    var appUserRoles = new AppUserRoles()
                    {
                        AppUserId = userId,
                        AppRoleId = item
                    };
                    _appUserRolesRepository.Add(appUserRoles);                                     
                }
                Save();
            }
            catch (Exception)
            {
                
            }
        }        

        public void Save()
        {
            _unitOfWork.Commit();
        }       

    }
}
