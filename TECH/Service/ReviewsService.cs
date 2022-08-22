
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TECH.Areas.Admin.Models;
using TECH.Areas.Admin.Models.Search;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Utilities;

namespace TECH.Service
{
    public interface IReviewsService
    {
        PagedResult<ReviewsModelView> GetAllPaging(ReviewsViewModelSearch ReviewsModelViewSearch);
        ReviewsModelView GetByid(int id);
        bool Add(ReviewsModelView view);
        bool Update(ReviewsModelView view);
        bool Deleted(int id);
        void Save();
        bool UpdateStatus(int id, int status);
    }

    public class ReviewsService : IReviewsService
    {
        private readonly IReviewsRepository _reviewsRepository;
        private IUnitOfWork _unitOfWork;
        public ReviewsService(IReviewsRepository postsRepository,
            IUnitOfWork unitOfWork)
        {
            _reviewsRepository = postsRepository;
            _unitOfWork = unitOfWork;
        }
        public ReviewsModelView GetByid(int id)
        {
            var data = _reviewsRepository.FindAll(p => p.id == id).FirstOrDefault();
            if (data != null)
            {                
                var model = new ReviewsModelView()
                {
                    id = data.id,
                    product_id = data.product_id,
                    order_id = data.order_id,
                    comment = data.comment,
                    star = data.star,
                    status = data.star,
                    created_at = data.created_at

                };
                return model;
            }
            return null;
        }
        public bool Add(ReviewsModelView view)
        {
            try
            {
                if (view != null)
                {
                    var products = new Reviews
                    {
                        product_id = view.product_id,
                        order_id = view.order_id,
                        comment = view.comment,
                        star = view.star,
                        status = view.star,
                        created_at = DateTime.Now,                        
                    };
                    _reviewsRepository.Add(products);

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
        public bool Update(ReviewsModelView view)
        {
            try
            {
                var dataServer = _reviewsRepository.FindById(view.id);
                if (dataServer != null)
                {
                    dataServer.id = view.id;
                    dataServer.title = view.title;
                    dataServer.avatar = view.avatar;
                    dataServer.slug = Regex.Replace(view.title.ToLower(), @"\s+", "-");
                    //dataServer.author = view.author;
                    dataServer.short_content = view.short_content;
                    dataServer.content = view.content;
                    dataServer.updated_at = DateTime.Now;     
                    _reviewsRepository.Update(dataServer);                                        
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

      public  bool UpdateStatus(int id, int status)
        {
            try
            {
                var dataServer = _reviewsRepository.FindById(id);
                if (dataServer != null)
                {
                    dataServer.status = status;
                    _reviewsRepository.Update(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        public bool Deleted(int id)
        {
            try
            {
                var dataServer = _reviewsRepository.FindById(id);
                if (dataServer != null)
                {
                    _reviewsRepository.Remove(dataServer);
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return false;
        }
        public PagedResult<ReviewsModelView> GetAllPaging(ReviewsViewModelSearch ReviewsModelViewSearch)
        {
            try
            {
                var query = _reviewsRepository.FindAll();              
                
                if (!string.IsNullOrEmpty(ReviewsModelViewSearch.name))
                {
                    query = query.Where(c => c.title == ReviewsModelViewSearch.name);
                }

                int totalRow = query.Count();
                query = query.Skip((ReviewsModelViewSearch.PageIndex - 1) * ReviewsModelViewSearch.PageSize).Take(ReviewsModelViewSearch.PageSize);
                var data = query.Select(p => new ReviewsModelView()
                {
                    id = p.id,
                    title = p.title,
                    avatar = p.avatar,
                    slug = p.slug,
                    author = p.author,
                    status = p.status,
                    short_content = p.short_content,
                    content = p.content,
                    created_at = p.created_at,
                    create_atstr = p.created_at.HasValue ? p.created_at.Value.ToString("hh:mm") + " - " + p.created_at.Value.ToString("dd/MM/yyyy") : "",
                    updated_at = p.updated_at
                }).ToList();
              
                var pagingData = new PagedResult<ReviewsModelView>
                {
                    Results = data,
                    CurrentPage = ReviewsModelViewSearch.PageIndex,
                    PageSize = ReviewsModelViewSearch.PageSize,
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
