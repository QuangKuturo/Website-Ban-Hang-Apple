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
    public interface IImagesProductService
    {
        //List<ImagesProductModelView> GetAll(int productId);
        bool AddImages(int productId, List<ImagesProductModelView> appImagesModelView);
        bool Deleted(int id);
        List<ImagesProductModelView> GetProductForProductId(int productId);
    }
    public class ImagesProductService : IImagesProductService
    {
        private readonly IImagesProductRepository _imagesProductRepository;
        private IUnitOfWork _unitOfWork;
        public ImagesProductService(IImagesProductRepository imagesProductRepository, IUnitOfWork unitOfWork)
        {
            _imagesProductRepository = imagesProductRepository;
            _unitOfWork = unitOfWork;
        }
        public bool AddImages(int productId, List<ImagesProductModelView> appImagesModelView)
        {
            try
            {                
                foreach (var image in appImagesModelView)
                {
                    _imagesProductRepository.Add(new ImagesProduct()
                    {
                        AppImageId = image.AppImageId,
                        ProductId = productId,
                    });
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<ImagesProductModelView> GetProductForProductId(int productId)
        {
            try
            {
                var lstImages = _imagesProductRepository.FindAll().Where(i => i.ProductId == productId).Select(p=>new ImagesProductModelView() { 
                    ProductId = p.ProductId,
                    AppImageId = p.AppImageId
                }).ToList();

                if (lstImages != null)
                {
                    return lstImages;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _imagesProductRepository.FindAll().Where(i=>i.AppImageId == id).FirstOrDefault();
                if (dataServer != null)
                {
                    _imagesProductRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }
        //public List<ImagesProductModelView> GetAll(int productId)
        //{
        //    try
        //    {
        //        var model = _imagesProductRepository.FindAll(x => x.ProductId == productId).Select(ip=> new ImagesProductModelView() { 
        //            Url = ip.Url,
        //            Alt = ip.Alt,
        //            ProductId = ip.ProductId
        //        }).ToList();

        //        return model;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
    }
}
