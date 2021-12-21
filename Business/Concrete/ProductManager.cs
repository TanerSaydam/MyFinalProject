using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.DTOs;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;        

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;            
        }

        public IResult Add(Product product)
        {
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }        

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<IList<Product>> GetAll()
        {
            //if (DateTime.Now.Hour == 20)
            //{
            //    return new ErrorDataResult<IList<Product>>(Messages.MaintenanceTime);
            //}
            return new SuccessDataResult<IList<Product>>(_productDal.GetList().ToList());
        }

        public IDataResult<IList<Product>> GetListByCategory(int categoryId)
        {
            return new SuccessDataResult<IList<Product>>(_productDal.GetList(p => p.CategoryId == categoryId).ToList());
        }

        public IDataResult<IList<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<IList<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        public IResult TranscaptionalOperation(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }
    }
}
