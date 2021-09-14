using ShopApp.Business.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }
        public IActionResult Index()
        {
            var result = _productService.GetList();

            //ProductModel productModel = _mapper.Map<ProductModel>(result.Data);
            if (result.Success)
            {
                // return Ok(result.Data);
                return View(new ProductListModel()
                {
                    Products = result.Data
                });
            }
            return BadRequest(result.Message);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult NotFound(int code)
        {
            ViewBag.Code = code;
            return View();
        }
    }
}
