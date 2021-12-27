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
using FluentValidation;
using Business.ValidationRules.FluentValidation;
using Core.CrossCuttingConcerns.Validation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Performance;
using System.Threading;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly ICategoryService _categoryService;
        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [SecuredOperation("product.add,admin")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run
                (
                CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                CheckIfProductNameExists(product.ProductName),
                CheckIfCategoryLimitExceded()
                );

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        [CacheRemoveAspect("IProductService.Get")]
        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        [CacheAspect]
        [PerformanceAspect(5)]
        public IDataResult<IList<Product>> GetAll()
        {            
            return new SuccessDataResult<IList<Product>>(_productDal.GetList().ToList());
        }

        public IDataResult<IList<Product>> GetListByCategoryId(int categoryId)
        {
            return new SuccessDataResult<IList<Product>>(_productDal.GetList(p => p.CategoryId == categoryId).ToList());
        }

        public IDataResult<IList<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<IList<ProductDetailDto>>(_productDal.GetProductDetails());
        }

        [TransactionScopeAspect]
        public IResult TranscaptionalOperation(Product product)
        {
            _productDal.Update(product);
            throw new Exception("");
            product.ProductName = "Transcaption Deneme";
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetList(p => p.CategoryId == categoryId).Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.ProductCountOfCategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetList(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetList();
            if (result.Data.Count >= 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }

    }
}
