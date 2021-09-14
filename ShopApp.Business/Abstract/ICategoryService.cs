using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Core.Utilities.Results;
using ShopApp.Entities.Concrete;

namespace ShopApp.Business.Abstract
{
    public interface ICategoryService
    {
        IDataResult<List<Category>> GetList();
        IDataResult<List<Category>> GetCategoriesByProductId(int productId);
    }
}
