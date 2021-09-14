using ShopApp.Business.Abstract;
using ShopApp.Business.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        public ShopController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult details(int id)
        {
            var resultProduct = _productService.GetById(id);
            var resultCategory = _categoryService.GetCategoriesByProductId(id);
            if (!resultProduct.Success ||! resultCategory.Success)
            {
                return BadRequest();
            }

            return View(new ProductDetailsModel()
            {
                Product = resultProduct.Data,
                Categories =resultCategory.Data});
        }

      //  [Authorize(Roles = "Product.List")] //Business tarafında yapıldı
        public IActionResult shopList(string categoryName,int page = 1)
        {
            //var accessToken = HttpContext.Session.GetString("JWToken");
            //HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var result = _productService.getProductsByCategory(categoryName, page, Consts.pageSize);
            var getCountProductByCategory = _productService.getCountByCategory(categoryName);
            if (result.Success)
            {
                //return Ok(result.Data);
                return View(new ProductListModel()
                {
                    Products = result.Data,
                    PageInfo = new PageInfo()
                    {
                        TotalItems= getCountProductByCategory.Data,
                        CurrentPage =page,
                        ItemPerPage=Consts.pageSize,
                        CurrentCategory=categoryName,
                        CurrentBrand=null
                    }
                });
            }

            return BadRequest(result.Message);
        }

    }
}
