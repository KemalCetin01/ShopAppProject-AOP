using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Core.DataAccess.EntityFramework;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EntityFramework.Contexts;
using ShopApp.Entities.Concrete;

namespace ShopApp.DataAccess.Concrete.EntityFramework
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category, ShopAppContext>, ICategoryDal
    {
    }
}
