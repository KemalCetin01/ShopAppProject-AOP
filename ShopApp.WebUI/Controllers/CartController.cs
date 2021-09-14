using ShopApp.Business.Abstract;
using ShopApp.Entities.Concrete;
using IyzipayCore;
using IyzipayCore.Model;
using IyzipayCore.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ShopApp.WebUI.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;
        private IOrderService _orderService;
        private IAuthService _autService;
        protected string _userId;
        public CartController(ICartService cartService,IOrderService orderService, IAuthService autService)
        {
            _cartService = cartService;
            _orderService = orderService;
            _autService = autService;

           
        }

        public IActionResult Index()
        {
            _userId = _autService.getUserId(HttpContext.Session.GetString("JWToken")).Message;
            var cart = _cartService.GetCartByUserId(_userId);
            if (cart.Data == null)
            {
                return NotFound();
            }
            return View(new CartModel()
            {
                CartId = cart.Data.Id,
                CartItems = cart.Data.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.Product.Id,
                    Name = i.Product.ProductName,
                    Price = (decimal)i.Product.UnitPrice,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            _userId = _autService.getUserId(HttpContext.Session.GetString("JWToken")).Message;

            _cartService.AddToCart(_userId, productId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteFromCart(int productId)
        {
            _cartService.DeleteFromCart(Convert.ToInt32(_userId), productId);
            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            var cart = _cartService.GetCartByUserId(_userId);

            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                CartId = cart.Data.Id,
                CartItems = cart.Data.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.Product.Id,
                    Name = i.Product.ProductName,
                    Price = (decimal)i.Product.UnitPrice,
                    ImageUrl = i.Product.ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            };

            return View(orderModel);
        }
        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                //Ödeme
                //Sipariş
                var cart = _cartService.GetCartByUserId(_userId);

                model.CartModel = new CartModel()
                {
                    CartId = cart.Data.Id,
                    CartItems = cart.Data.CartItems.Select(i => new CartItemModel()
                    {
                        CartItemId = i.Id,
                        ProductId = i.Product.Id,
                        Name = i.Product.ProductName,
                        Price = (decimal)i.Product.UnitPrice,
                        ImageUrl = i.Product.ImageUrl,
                        Quantity = i.Quantity
                    }).ToList()
                };

                var payment = PaymentProcess(model);

                if (payment.Status == "success")
                {
                    SaveOrder(model, payment, _userId);
                    ClearCart(cart.Data.Id);
                    return View("Success");
                }
            }

            return View(model);
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();

            order.OrderNumber = new Random().Next(111111, 999999).ToString();
            order.OrderState = EnumOrderState.Completed; //sipariş durumu
            order.PaymentTypes = EnumPaymentTypes.CreditCart; //ödeme tipi
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.Email = model.Email;
            order.Phone = model.Phone;
            order.Address = model.Address;
            order.UserId = userId;

            foreach (var item in model.CartModel.CartItems)
            {
                var orderitem = new OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.ProductId
                };
                order.OrderItems.Add(orderitem);
            }
            _orderService.Add(order);
        }

        private void ClearCart(int cartId)
        {
            _cartService.ClearCart(cartId);
        }

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-8Ut5931cW2vX1kNom04tcwL0JbP5RP4L"; //your api key
            options.SecretKey = "sandbox-nhXj4NubCEByFl3VQJi7go63nn4ZuQPt"; //your secret key
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = Guid.NewGuid().ToString();
            request.Price = model.CartModel.TotalPrice().ToString().Split(",")[0]; ;
            request.PaidPrice = model.CartModel.TotalPrice().ToString().Split(",")[0]; ;
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = model.CartModel.CartId.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.Cvv;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            //paymentCard.CardHolderName = "John Doe";
            //paymentCard.CardNumber = "5528790000000008";
            //paymentCard.ExpireMonth = "12";
            //paymentCard.ExpireYear = "2030";
            //paymentCard.Cvc = "123";

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "Nuri";
            buyer.Surname = "Çetin";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.CartModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Phone";
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItem.Price = (item.Price * item.Quantity).ToString().Split(",")[0];

                basketItems.Add(basketItem);
            }

            request.BasketItems = basketItems;

            return Payment.Create(request, options);

        }

        public IActionResult GetOrders()
        {
            var orders = _orderService.getOrders(_userId);
            var orderListModel = new List<OrderListModel>();
            OrderListModel orderModel;

            foreach (var order in orders.Data)
            {
                orderModel = new OrderListModel();
                orderModel.OrderId = order.Id;
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.OrderDate = order.OrderDate;
                orderModel.OrderNote = order.OrderNote;
                orderModel.Phone = order.Phone;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;
                orderModel.Email = order.Email;
                orderModel.Address = order.Address;
                orderModel.City = order.City;

                orderModel.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                {
                    OrderItemId = i.Id,
                    Name = i.Product.ProductName,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.Product.ImageUrl
                }).ToList();

                orderListModel.Add(orderModel);

            }

            return View(orderListModel);
        }
    }
}
