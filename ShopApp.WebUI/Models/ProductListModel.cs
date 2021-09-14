using ShopApp.Entities.Concrete;
using System;
using System.Collections.Generic;

namespace ShopApp.WebUI.Models
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int ItemPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string CurrentCategory { get; set; }
        public string CurrentBrand { get; set; }
        public int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemPerPage); // Ceiling --> yukarı yuvarla
        }
    }
    public class ProductListModel
    {

        public PageInfo PageInfo { get; set; }
        public List<Product> Products { get; set; }
    }
}
