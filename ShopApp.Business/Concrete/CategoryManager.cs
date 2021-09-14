using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShopApp.Business.Abstract;
using ShopApp.Core.Utilities.Results;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities.Concrete;

namespace ShopApp.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public IDataResult<List<Category>> GetCategoriesByProductId(int productId)
        {

            return new SuccessDataResult<List<Category>>(_categoryDal.GetList(p=>p.ProductId==productId).ToList());
        }

        public IDataResult<List<Category>> GetList()
        {
            return new SuccessDataResult<List<Category>>(_categoryDal.GetList().ToList());
        }
    }
}
