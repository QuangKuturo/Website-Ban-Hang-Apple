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
    public interface IAppImagesService
    {
       List<AppImagesModelView> GetImagesByProductId(int productId);

       List<ImagesProductModelView> AddImages(int productId, string[] images);

       AppImages GetAppImagesById(int imageId);
       bool Deleted(int id);
       void Save();
    }

    public class AppImagesService : IAppImagesService
    {
        private readonly IAppImagesRepository _appImagesRepository;
        private readonly IImagesProductRepository _imagesProductRepository;
        private IUnitOfWork _unitOfWork;

        public AppImagesService(IAppImagesRepository appImagesRepository,
            IImagesProductRepository imagesProductRepository,
            IUnitOfWork unitOfWork)
        {
            _appImagesRepository = appImagesRepository;
            _imagesProductRepository = imagesProductRepository;
            _unitOfWork = unitOfWork;
        }
       
        public List<ImagesProductModelView> AddImages(int productId, string[] images)
        {
            try
            {
                var lstImageProduct = new List<ImagesProductModelView>();
                foreach (var image in images)
                {
                    var appImages = new AppImages()
                    {
                        Url = image,
                    };
                    _appImagesRepository.Add(appImages);
                    Save();
                    lstImageProduct.Add(new ImagesProductModelView() { 
                        ProductId = productId,
                        AppImageId = appImages.Id
                    });
                }

                return lstImageProduct;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _appImagesRepository.FindById(id);
                if (dataServer != null)
                {
                    _appImagesRepository.Remove(dataServer);
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

        public AppImages GetAppImagesById(int imageId)
        {
            var AppImage = _appImagesRepository.FindById(imageId);
            if (AppImage != null && !AppImage.IsDeleted)           
                return AppImage;                
            
            return null;
        }

        public List<AppImagesModelView> GetImagesByProductId(int productId)
        {
            try
            {
                var lstImagesModel = new List<AppImagesModelView>();
                var model = _imagesProductRepository.FindAll(x => x.ProductId == productId).Select(ip=> new ImagesProductModelView() { 
                    AppImageId = ip.AppImageId,
                    ProductId = ip.ProductId
                }).ToList();

                if (model != null && model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        if (item.AppImageId.HasValue && item.AppImageId.Value > 0)
                        {
                            var AppImages = GetAppImagesById(item.AppImageId.Value);
                            if (AppImages != null)
                            {
                                lstImagesModel.Add(new AppImagesModelView()
                                {
                                    AppImageId = AppImages.Id,
                                    ProductId = productId,
                                    Url = AppImages.Url
                                });
                            }
                        }
                    }
                }
                return lstImagesModel;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
