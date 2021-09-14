using ShopApp.Core.Utilities.Results;
using ShopApp.Entities.Concrete;
using System.Collections.Generic;

namespace ShopApp.Business.Abstract
{
    public interface ICartService
    {
        IResult InitializeCart(string userId); //sepet Başlat
        IDataResult<Cart> GetCartByUserId(string userId);
        IResult AddToCart(string userId, int productId, int quantity);
        IResult DeleteFromCart(int userId, int productId);
        IResult ClearCart(int cartId);
        IDataResult<Cart> GetById(int productId);

    }
}
