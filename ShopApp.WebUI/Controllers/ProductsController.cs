using AutoMapper;
using ShopApp.Business.Abstract;
using ShopApp.Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        private IProductService _productService;
        private IMapper _mapper;
        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        [HttpGet("Products/getall")]
        public IActionResult getList()
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

        [HttpGet("getlistbycategory")]
        public IActionResult GetListByCategory(int categoryId)
        {
            var result = _productService.GetListByCategory(categoryId);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
        [HttpGet("getbyid")]
        public IActionResult GetById(int productId)
        {
            var result = _productService.GetById(productId);
            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }

        public IActionResult CreateProduct()
        {
            return View(new ProductModel());
        }

        [HttpPost("Products/add")]
        public IActionResult CreateProduct(ProductModel productModel)
        {
            Product product = _mapper.Map<Product>(productModel);
            var result = _productService.Add(product);
            ViewBag.Message = result.Message;
            if (result.Success)
            {
                return getList();
                //   return View("getList");
            }

            return View(productModel);
        }
        public IActionResult Update(int id)
        {

            var result = _productService.GetById(id);
            if (result.Success)
            {
                return View(new ProductListModel()
                {
                   // Products = result.Data
                });
            }

            return BadRequest(result.Message);
        }
        [HttpPost("update")]
        public IActionResult Update(ProductModel productModel)
        {
            Product product = _mapper.Map<Product>(productModel);
            var result = _productService.Update(product);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("delete")]
        public IActionResult Delete(ProductModel productModel)
        {
            Product product = _mapper.Map<Product>(productModel);
            var result = _productService.Delete(product);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }

        [HttpPost("transaction")]
        public IActionResult TransactionTest(ProductModel productModel)
        {
            Product product = _mapper.Map<Product>(productModel);
            var result = _productService.TransactionalOperation(product);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
