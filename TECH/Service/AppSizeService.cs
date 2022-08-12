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
    public interface IAppSizeService
    {
        PagedResult<AppSizeModelView> GetAllPaging(AppSizeViewModelSearch appSizeViewModelSearch);
        List<AppSizeModelView> GetAll();
        AppSizeModelView GetById(int id);
        bool Add(AppSizeModelView view);
        bool Update(AppSizeModelView view);
        bool Deleted(int id);
        void Save();
    }
    public class AppSizeService : IAppSizeService
    {
        private readonly IAppSizeRepository _appSizeRepository;
        private IUnitOfWork _unitOfWork;
        public AppSizeService(IAppSizeRepository appSizeRepository, IUnitOfWork unitOfWork)
        {
            _appSizeRepository = appSizeRepository;
            _unitOfWork = unitOfWork;
        }
        public AppSizeModelView GetById(int id)
        {
            var data = _appSizeRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new AppSizeModelView()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description
                };
                return model;
            }
            return null;
        }

        public bool Add(AppSizeModelView view)
        {
            try
            {
                if (view != null)
                {
                    var _colors = new AppSize
                    {
                        Name = view.Name,
                        Description = view.Description
                    };
                    _appSizeRepository.Add(_colors);
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
        public bool Update(AppSizeModelView view)
        {
            try
            {
                var dataServer = _appSizeRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.Name = view.Name;
                    dataServer.Description = view.Description;
                    _appSizeRepository.Update(dataServer);
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
                var dataServer = _appSizeRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.IsDeleted = true;
                    _appSizeRepository.Update(dataServer);
                    return true;

                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<AppSizeModelView> GetAllPaging(AppSizeViewModelSearch appSizeViewModelSearch)
        {
            try
            {
                var query = _appSizeRepository.FindAll(s=>s.IsDeleted != true);
               
                if (!string.IsNullOrEmpty(appSizeViewModelSearch.Name))
                {
                    query = query.Where(c => c.Name.ToLower() == appSizeViewModelSearch.Name.ToLower());
                }
                int totalRow = query.Count();
                query = query.Skip((appSizeViewModelSearch.PageIndex - 1) * appSizeViewModelSearch.PageSize).Take(appSizeViewModelSearch.PageSize);
                var data = query.Select(c => new AppSizeModelView()
                {
                    Name = c.Name,
                    Description = c.Description,
                    Id = c.Id,
                }).OrderByDescending(c => c.Id).ToList();
                var pagingData = new PagedResult<AppSizeModelView>
                {
                    Results = data,
                    CurrentPage = appSizeViewModelSearch.PageIndex,
                    PageSize = appSizeViewModelSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public List<AppSizeModelView> GetAll()
        {
            var data = _appSizeRepository.FindAll(c => c.IsDeleted != true).Select(c => new AppSizeModelView()
            {
                Id = c.Id,
                Name = c.Name,
                //Code = c.Code
            }).ToList();

            return data;
        }
    }
}
