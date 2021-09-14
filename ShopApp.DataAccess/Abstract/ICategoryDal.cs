using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Core.DataAccess;
using ShopApp.Entities.Concrete;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICategoryDal:IEntityRepository<Category>
    {
    }
}
