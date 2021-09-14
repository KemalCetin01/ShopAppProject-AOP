using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ShopApp.Core.DataAccess.EntityFramework;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EntityFramework.Contexts;
using ShopApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace ShopApp.DataAccess.Concrete.EntityFramework
{
    public class EfProuctDal : EfEntityRepositoryBase<Product, ShopAppContext>, IProductDal
    {
        public List<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            using (var context = new ShopAppContext())
            {
                var products = context.Products.AsQueryable();
                products = products.Include(i => i.ProductCategories)
                                   .ThenInclude(i => i.Category)
                                   .Where(i => i.ProductCategories.Any(a => a.Category.CategoryName.ToLower() == category.ToLower()));
                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();

            }
        }
    }
}
