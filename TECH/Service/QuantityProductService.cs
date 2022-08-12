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
    public interface IQuantityProductService
    {
        //List<ImagesProductModelView> GetAll(int productId);
        bool Add(QuantityProductModelView view);
        bool Deleted(int id);
        List<QuantityProductModelView> GetQuantityProductForProductId(int productId);
        bool Update(QuantityProductModelView view);
        QuantityProductModelView GetById(int id);
        void Save();
    }
    public class QuantityProductService : IQuantityProductService
    {
        private readonly IQuantityProductRepository _quantityProductRepository;
        private IUnitOfWork _unitOfWork;
        public QuantityProductService(IQuantityProductRepository quantityProductRepository, IUnitOfWork unitOfWork)
        {
            _quantityProductRepository = quantityProductRepository;
            _unitOfWork = unitOfWork;
        }
        public bool Add(QuantityProductModelView view)
        {
            try
            {
                if (view != null)
                {
                    var _quantityProduct = new QuantityProduct
                    {
                        ProductId = view.ProductId,
                        AppSizeId = view.AppSizeId,
                        TotalImported = view.TotalImported,
                        DateImport = view.DateImport,
                    };
                    _quantityProductRepository.Add(_quantityProduct);
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
        public bool Update(QuantityProductModelView view)
        {
            try
            {
                var dataServer = _quantityProductRepository.FindById(view.Id);
                if (dataServer != null)
                {
                    dataServer.Id = view.Id;
                    dataServer.ProductId = view.ProductId;
                    dataServer.AppSizeId = view.AppSizeId;
                    dataServer.TotalImported = view.TotalImported;
                    dataServer.DateImport = view.DateImport;
                    dataServer.IsDeleted = false;
                    _quantityProductRepository.Update(dataServer);

                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public List<QuantityProductModelView> GetQuantityProductForProductId(int productId)
        {
            try
            {
                var lstQuantityProduct = _quantityProductRepository.FindAll(q=>q.IsDeleted !=true).Where(q=>q.ProductId == productId).Select(p=>new QuantityProductModelView() { 
                    Id = p.Id,
                    TotalImported = p.TotalImported,
                    TotalSold = p.TotalSold,
                    TotalStock = p.TotalStock,
                    IsDeleted = p.IsDeleted,
                    DateImport = p.DateImport,
                    ProductId = p.ProductId,
                    AppSizeId = p.AppSizeId
                }).OrderByDescending(p=>p.Id).ToList();

                if (lstQuantityProduct != null && lstQuantityProduct.Count > 0)
                {
                    foreach (var item in lstQuantityProduct)
                    {
                        item.DateImportStr = item.DateImport.HasValue ? item.DateImport.Value.ToString("dd/MM/yyyy") : "";
                    }
                }

                return lstQuantityProduct;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _quantityProductRepository.FindAll().Where(i=>i.Id == id).FirstOrDefault();
                if (dataServer != null)
                {
                    dataServer.IsDeleted = true;
                    _quantityProductRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
        public QuantityProductModelView GetById(int id)
        {
            if (id > 0)
            {
                var data = _quantityProductRepository.FindAll(p => p.Id == id).FirstOrDefault();
                if (data != null)
                {                    
                    var model = new QuantityProductModelView()
                    {
                        Id = data.Id,
                        AppSizeId = data.AppSizeId,
                        DateImport = data.DateImport,
                        ProductId = data.ProductId,                        
                        TotalImported = data.TotalImported,
                        TotalSold = data.TotalSold,
                        TotalStock = data.TotalStock
                    };
                    return model;
                }               
            }
            return null;
        }
    }
}
