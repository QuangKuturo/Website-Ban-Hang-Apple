using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TECH.Areas.Admin.Models
{
    public class ProductModelView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Decription { get; set; }
        public string SubDecription { get; set; }
        public decimal? Price { get; set; }
        public decimal? ReducedPrice { get; set; }
        public int? Total { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? BrandsId { get; set; }
        public string BrandName { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; } 
        public List<string> UrlImage { get; set; }
        //public DateTime? ManufacturingDate { get; set; } // ngày sản xuất
        //public DateTime? ExpiryDate { get; set; } // hạn sử dụng
        public List<QuantityProductModelView> ProductQuantity { get; set; }
        public List<AppSizeModelView> AppSizeModel { get; set; }
        public List<ProductModelView> ProductSame { get; set; }
        public AppSizeModelView AppSizeModelCart { get; set; }
        public int SoLuong { get; set; }
    }
    public class ProductQuantity
    {
        public int ProductId { get; set; }
        public int Total { get; set; }
    }
}
