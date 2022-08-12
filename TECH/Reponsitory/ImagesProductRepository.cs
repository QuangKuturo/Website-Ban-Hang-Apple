using Microsoft.EntityFrameworkCore;
using System;
using TECH.Data.DatabaseEntity;

namespace TECH.Reponsitory
{
    public interface IImagesProductRepository : IRepository<ImagesProduct, int>
    {
       
    }

    public class ImagesProductRepository : EFRepository<ImagesProduct, int>, IImagesProductRepository
    {
        public ImagesProductRepository(DataBaseEntityContext context) : base(context)
        {
        }
    }
}
