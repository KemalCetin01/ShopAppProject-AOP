using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Core.Entities;

namespace ShopApp.Entities.Concrete
{
    public class Category:IEntity
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int ProductId { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
