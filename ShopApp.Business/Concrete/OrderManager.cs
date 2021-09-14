using ShopApp.Business.Abstract;
using ShopApp.Business.Constants;
using ShopApp.Core.Utilities.Business;
using ShopApp.Core.Utilities.Results;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private IOrderDal _orderDal;
        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }
        public IResult Add(Order order)
        {
            _orderDal.Add(order);
            return new SuccessResult(Messages.OrderAdded);
        }

        public IDataResult<List<Order>> getOrders(string userId)
        {
            // return new SuccessDataResult<List<Order>>(_orderDal.GetList(p=>p.UserId==null)).ToList());
            return null;

        }
    }
}
