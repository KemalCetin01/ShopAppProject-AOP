using ShopApp.Core.DataAccess;
using ShopApp.Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entities.Concrete;

namespace ShopApp.DataAccess.Abstract
{
    public interface ICartDal : IEntityRepository<Cart>
    {
        Cart GetByUserId(string userId);
    }
}
