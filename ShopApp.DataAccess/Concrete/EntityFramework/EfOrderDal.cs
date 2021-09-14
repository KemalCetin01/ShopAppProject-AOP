using ShopApp.Core.DataAccess.EntityFramework;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EntityFramework.Contexts;
using ShopApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EntityFramework
{
   public class EfOrderDal : EfEntityRepositoryBase<Order, ShopAppContext>, IOrderDal
    {
    }
}
