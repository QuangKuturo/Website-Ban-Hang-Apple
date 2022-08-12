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
    public interface IColorsProductService
    {
       List<ColorsProductModelView> GetAll(int productId);
       bool AddImages(int productId, int[] colorsId);
    }
    public class ColorsProductService : IColorsProductService
    {
        private readonly IColorsProductRepository _colorsProductRepository;
        private IUnitOfWork _unitOfWork;
        public ColorsProductService(IColorsProductRepository colorsProductRepository, IUnitOfWork unitOfWork)
        {
            _colorsProductRepository = colorsProductRepository;
            _unitOfWork = unitOfWork;
        }
        public bool AddImages(int productId, int[] colorsId)
        {
            try
            {
                _colorsProductRepository.RemoveMultiple(_colorsProductRepository.FindAll(x => x.ProductId == productId).ToList());
                foreach (var color in colorsId)
                {
                    _colorsProductRepository.Add(new AppSizeProduct()
                    {
                        AppSizeId = color,
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
        public List<ColorsProductModelView> GetAll(int productId)
        {
            try
            {
                var model = _colorsProductRepository.FindAll(x => x.ProductId == productId).Select(p=> new ColorsProductModelView() { 
                    
                    ProductId = p.ProductId.Value,
                    ColorId = p.AppSizeId.Value
                }).ToList();
                
                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
