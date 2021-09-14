using ShopApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Entities.Concrete
{
   public class ProductCategory : IEntity
    {  //iki tabloyu birleştiren tablo  contition(cankşın)

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
