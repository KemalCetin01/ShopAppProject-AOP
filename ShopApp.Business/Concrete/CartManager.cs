using ShopApp.Business.Abstract;
using ShopApp.Business.Constants;
using ShopApp.Core.Utilities.Business;
using ShopApp.Core.Utilities.Results;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace ShopApp.Business.Concrete
{
    public class CartManager : ICartService
    {
        private ICartDal _cartDal;
        public CartManager(ICartDal cartDal)
        {
            _cartDal = cartDal;
        }
        public IResult AddToCart(string userId, int productId, int quantity)
        {
            IResult result = BusinessRules.Run(CheckIfCartByUserId(userId,productId,quantity));
            if (result != null)
            {
                return result;
            }
            return new SuccessResult(Messages.CartAdded);

        }
        public IResult CheckIfCartByUserId(string userId, int productId, int quantity)
        {
            var result1 = GetListCartByUserId(userId);
            var cart = _cartDal.GetByUserId(userId);

            if (cart!=null)
            {

                    var index = cart.CartItems.FindIndex(i => i.ProductId == productId);
                    if (index < 0)
                    {
                    cart.CartItems.Add(new CartItem()
                        {
                            ProductId = productId,
                            Quantity = quantity,
                            CartId = cart.Id
                        });
                    _cartDal.Add(cart);
                }
                else
                    {
                    cart.CartItems[index].Quantity += quantity;
                    _cartDal.Update(cart);
                }
                //sepette aynı üründen 1 tane varsa sayı arttırılır yoksa yeni ürün eklenir

                return new SuccessResult(Messages.CartAdded);
            }
            return new ErrorResult(Messages.CartFailed);
        }
        public IResult ClearCart(int cartId)
        {
            var entityResult = GetById(cartId);
            _cartDal.Delete(entityResult.Data);
            return new SuccessResult(Messages.CartDeleted);
        }

        public IResult DeleteFromCart(int cartId, int productId)
        {
            var entityResult = GetByIdAndProductId(cartId,productId);

            _cartDal.Delete(entityResult.Data);

            return new SuccessResult(Messages.CartDeleted);
        }

        public IDataResult<Cart> GetListCartByUserId(string userId)
        {
            return new SuccessDataResult<Cart>(_cartDal.Get(p => p.UserId == userId));
        }
        public IDataResult<Cart> GetCartByUserId(string userId)
        {
            return new SuccessDataResult<Cart>(_cartDal.GetByUserId(userId));
        }

        public IResult InitializeCart(string userId)
        {
            _cartDal.Add(new Cart() {UserId=userId });
            return new SuccessResult(Messages.CartAdded);
        }

        public IDataResult<Cart> GetById(int cartId)
        {
            return new SuccessDataResult<Cart>(_cartDal.Get(p => p.Id == cartId));
        } 
        public IDataResult<Cart> GetByIdAndProductId(int cartId,int productId)
        {
            return new SuccessDataResult<Cart>(_cartDal.Get(p => p.Id == cartId && p.CartItems.Any(a=>a.ProductId== productId)));
        }
    }
}
