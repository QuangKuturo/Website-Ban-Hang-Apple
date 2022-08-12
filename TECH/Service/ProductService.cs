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
    public interface IProductService
    {
        PagedResult<ProductModelView> GetAllPaging(ProductViewModelSearch productViewModelSearch);
        ProductModelView GetById(int id);
        int Add(ProductModelView view);
        bool Update(ProductModelView view);
        bool Deleted(int id);
        List<ProductModelView> GetProductCategory(int categoryId);
        void Save();
    }
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private IUnitOfWork _unitOfWork;
        public ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }              
        public ProductModelView GetById(int id)
        {
            var data = _productRepository.FindAll(p=>p.Id == id).FirstOrDefault();
            if (data != null)
            {
                var model = new ProductModelView()
                {
                    Name = data.Name,
                    Id = data.Id,
                    Decription = data.Decription,
                    SubDecription = data.SubDecription,
                    Price = data.Price.Value,
                    ReducedPrice = data.ReducedPrice,
                    Total = data.Total,
                    CategoryId = data.CategoryId,
                    BrandsId = data.BrandsId,
                    //ManufacturingDate = data.ManufacturingDate,
                    //ExpiryDate = data.ExpiryDate
                };
                return model;
            }
            return null;
        }

        public int Add(ProductModelView view)
        {            
            try
            {
                if (view != null)
                {
                    var _product = new Product
                    {
                        Name = view.Name,
                        Decription = view.Decription,
                        SubDecription = view.SubDecription,
                        CategoryId = view.CategoryId,
                        Total = view.Total,
                        Price = view.Price,
                        ReducedPrice = view.ReducedPrice,
                        BrandsId = view.BrandsId,        
                        //ManufacturingDate = view.ManufacturingDate,
                        //ExpiryDate = view.ExpiryDate
                    };
                    _productRepository.Add(_product);
                    Save();

                    return _product.Id;
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
        public bool Update(ProductModelView view)
        {
            try
            {
                var dataServer = _productRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.Name = view.Name;
                    dataServer.Decription = view.Decription;
                    dataServer.SubDecription = view.SubDecription;
                    dataServer.CategoryId = view.CategoryId;
                    dataServer.Total = view.Total;
                    dataServer.Price = view.Price;
                    dataServer.ReducedPrice = view.ReducedPrice;
                    dataServer.BrandsId = view.BrandsId;
                    //dataServer.ManufacturingDate = view.ManufacturingDate;
                    //dataServer.ExpiryDate = view.ExpiryDate;
                    _productRepository.Update(dataServer);
                    //Save();
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
                var dataServer = _productRepository.FindById(id);
                if (dataServer != null)
                {
                    //var listCategoryChild = _productRepository.FindAll().Where(c => c.Id == dataServer.Id).ToList();
                    //foreach (var itemChild in listCategoryChild)
                    //{
                    //    _productRepository.Remove(itemChild);
                    //}

                    //_productRepository.Remove(dataServer);
                    dataServer.IsDeleted = true;
                    _productRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public List<ProductModelView> GetProductCategory(int categoryId)
        {
            var data = _productRepository.FindAll(p => p.IsDeleted != true).Where(p=>p.CategoryId == categoryId).Select(p => new ProductModelView()
            {
                Name = p.Name,
                Id = p.Id,
                Decription = p.Decription,
                SubDecription = p.SubDecription,
                Price = p.Price.Value,
                ReducedPrice = p.ReducedPrice,
                Total = p.Total,
                CategoryId = p.CategoryId,
                BrandsId = p.BrandsId,
            }).OrderByDescending(p => p.Id).ToList();
            return data;
        }
        public PagedResult<ProductModelView> GetAllPaging(ProductViewModelSearch ProductViewModelSearch)
        {
            try
            {
                var query = _productRepository.FindAll(p=>p.IsDeleted != true);
           
                if (!string.IsNullOrEmpty(ProductViewModelSearch.ProductName))
                {
                    query = query.Where(p => p.Name == ProductViewModelSearch.ProductName);
                }
                if (ProductViewModelSearch.CategoryId.HasValue && ProductViewModelSearch.CategoryId.Value > 0)
                {
                    query = query.Where(p => p.CategoryId == ProductViewModelSearch.CategoryId);
                }
                if (ProductViewModelSearch.ProductId > 0)
                {
                    query = query.Where(p => p.Id == ProductViewModelSearch.ProductId);
                }
                if (ProductViewModelSearch.ProductId > 0)
                {
                    query = query.Where(p => p.Id == ProductViewModelSearch.ProductId);
                }

                if (ProductViewModelSearch.Status.HasValue)
                {
                    query = query.Where(p =>p.Status == ProductViewModelSearch.Status);
                }

                int totalRow = query.Count();
                query = query.Skip((ProductViewModelSearch.PageIndex - 1) * ProductViewModelSearch.PageSize).Take(ProductViewModelSearch.PageSize);
                var data = query.Select(p => new ProductModelView()
                {
                    Name = p.Name,
                    Id = p.Id,
                    Decription= p.Decription,
                    SubDecription = p.SubDecription,
                    Price = p.Price.Value,
                    ReducedPrice = p.ReducedPrice,
                    Total = p.Total,
                    CategoryId = p.CategoryId,
                    BrandsId = p.BrandsId,
                    //ManufacturingDate = p.ManufacturingDate,
                    //ExpiryDate = p.ExpiryDate
                    //ParentId = c.ParentId
                }).OrderByDescending(p=>p.Id).ToList();
                var pagingData = new PagedResult<ProductModelView>
                {
                    Results = data,
                    CurrentPage = ProductViewModelSearch.PageIndex,
                    PageSize = ProductViewModelSearch.PageSize,
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
