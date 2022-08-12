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
    public interface IBrandsService
    {
        PagedResult<BrandsModelView> GetAllPaging(ColorViewModelSearch colorViewModelSearch);
        List<BrandsModelView> GetAll();
        BrandsModelView GetById(int id);
        bool Add(BrandsModelView view);
        bool Update(BrandsModelView view);
        bool Deleted(int id);
        void Save();
    }
    public class BrandsService : IBrandsService
    {
        private readonly IBrandsRepository _brandsRepository;
        private IUnitOfWork _unitOfWork;
        public BrandsService(IBrandsRepository brandsRepository, IUnitOfWork unitOfWork)
        {
            _brandsRepository = brandsRepository;
            _unitOfWork = unitOfWork;
        }
        public BrandsModelView GetById(int id)
        {
            var data = _brandsRepository.FindAll(p => p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new BrandsModelView()
                {
                    Id = data.Id,
                    Name = data.Name,
                };
                return model;
            }
            return null;
        }

        public bool Add(BrandsModelView view)
        {
            try
            {
                if (view != null)
                {
                    var _brands = new Brands
                    {
                        Name = view.Name,
                    };
                    _brandsRepository.Add(_brands);
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
        public bool Update(BrandsModelView view)
        {
            try
            {
                var dataServer = _brandsRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.Name = view.Name;
                    _brandsRepository.Update(dataServer);
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
                var dataServer = _brandsRepository.FindById(id);
                if (dataServer != null)
                {
                    _brandsRepository.Remove(dataServer);
                    return true;

                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }
        public PagedResult<BrandsModelView> GetAllPaging(ColorViewModelSearch colorViewModelSearch)
        {
            try
            {
                var query = _brandsRepository.FindAll();
                if (!string.IsNullOrEmpty(colorViewModelSearch.Name))
                {
                    query = query.Where(c => c.Name.ToLower() == colorViewModelSearch.Name.ToLower());
                }


                int totalRow = query.Count();
                query = query.Skip((colorViewModelSearch.PageIndex - 1) * colorViewModelSearch.PageSize).Take(colorViewModelSearch.PageSize);
                var data = query.Select(c => new BrandsModelView()
                {
                    Name = c.Name,
                    Id = c.Id,
                    //ParentId = c.ParentId
                }).ToList();
                var pagingData = new PagedResult<BrandsModelView>
                {
                    Results = data,
                    CurrentPage = colorViewModelSearch.PageIndex,
                    PageSize = colorViewModelSearch.PageSize,
                    RowCount = totalRow,
                };
                return pagingData;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public List<BrandsModelView> GetAll()
        {
            var data = _brandsRepository.FindAll().Select(b => new BrandsModelView()
            {
                Id = b.Id,
                Name = b.Name
            }).ToList();
            return data;
        }
    }
}
