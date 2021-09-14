using ShopApp.Core.DataAccess.EntityFramework;
using ShopApp.Core.Utilities.Results;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EntityFramework.Contexts;
using ShopApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EntityFramework
{
    public class EfCartDal : EfEntityRepositoryBase<Cart, ShopAppContext>, ICartDal
    {
        public Cart GetByUserId(string userId)
        {
            using (var context = new ShopAppContext())
            {
                return context
                            .Carts
                            .Include(i => i.CartItems)
                            .ThenInclude(i => i.Product)
                            .FirstOrDefault(i => i.UserId == userId);
            }
        }


    }
}
