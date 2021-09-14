using ShopApp.Core.DataAccess;
using ShopApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.DataAccess.Abstract
{
    public interface IProductDal:IEntityRepository<Product>
    {
        List<Product> GetProductsByCategory(string category, int page, int pageSize);

    }
}
