using ShopApp.Core.Utilities.Results;
using ShopApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
   public interface IOrderService
    {
        IDataResult<List<Order>> getOrders(string userId);
        IResult Add(Order order);

    }
}
