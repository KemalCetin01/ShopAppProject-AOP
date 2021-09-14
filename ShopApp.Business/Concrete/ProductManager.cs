using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ShopApp.Business.Abstract;
using ShopApp.Business.BusinessAspects.Autofac;
using ShopApp.Business.Constants;
using ShopApp.Business.ValidationRules.FluentValidation;
using ShopApp.Core.Aspects.Autofac.Caching;
using ShopApp.Core.Aspects.Autofac.Exception;
using ShopApp.Core.Aspects.Autofac.Logging;
using ShopApp.Core.Aspects.Autofac.Performance;
using ShopApp.Core.Aspects.Autofac.Transaction;
using ShopApp.Core.Aspects.Autofac.Validation;
using ShopApp.Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using ShopApp.Core.CrossCuttingConcerns.Validation;
using ShopApp.Core.Extensions;
using ShopApp.Core.Utilities.Business;
using ShopApp.Core.Utilities.Results;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EntityFramework;
using ShopApp.Entities.Concrete;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ShopApp.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;
        private ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.Id == productId));
        }

        [PerformanceAspect(1)]
        public IDataResult<List<Product>> GetList()
        {
            Thread.Sleep(2000);
            return new SuccessDataResult<List<Product>>(_productDal.GetList().ToList());
        }

        [SecuredOperation("Product.List,Admin")]
        [LogAspect(typeof(FileLogger))]

        [CacheAspect(duration: 10)]
        public IDataResult<List<Product>> GetListByCategory(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.CategoryId == categoryId).ToList());
        }

        [SecuredOperation("Product.List,Admin")]
        public IDataResult<List<Product>> getProductsByCategory(string categoryName,int page,int pageSize)
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
                return new SuccessDataResult<List<Product>>(_productDal.GetList(p => p.ProductCategories.Any(a => a.Category.CategoryName.ToLower() == categoryName.ToLower())).Skip((page - 1) * pageSize).Take(pageSize).ToList());
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetList().Skip((page - 1) * pageSize).Take(pageSize).ToList());

        }

        public IDataResult<int> getCountByCategory(string categoryName)
        {
            if (!string.IsNullOrEmpty(categoryName))
            {
            return new SuccessDataResult<int>(_productDal.GetList(p => p.ProductCategories.Any(a => a.Category.CategoryName.ToLower() == categoryName.ToLower())).Count());
              
            } 
            return new SuccessDataResult<int>(_productDal.GetList().Count());
        }

        [ValidationAspect(typeof(ProductValidator), Priority = 1)]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(CheckIfProductNameExists(product.ProductName), CheckIfCategoryIsEnabled());

            if (result != null)
            {
                return result;
            }
            GenerateProductCode(product);
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }
        private void GenerateProductCode(Product product)
        {
            Random random = new Random();
            product.ProductCode = product.ProductName.Substring(0, 2) + random.Next(999) + product.Modell.Substring(0, product.Modell.Length / 2);

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

        private IResult CheckIfCategoryIsEnabled()
        {
            var result = _categoryService.GetList();
            if (result.Data.Count < 7)
            {
                return new ErrorResult(Messages.CategoryIsEnabled);
            }

            return new SuccessResult();
        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult(Messages.ProductDeleted);
        }

        public IResult Update(Product product)
        {

            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }

        [TransactionScopeAspect]
        public IResult TransactionalOperation(Product product)
        {
            _productDal.Update(product);
            //_productDal.Add(product);
            return new SuccessResult(Messages.ProductUpdated);
        }
    }
}
