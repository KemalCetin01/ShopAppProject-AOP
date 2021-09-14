using ShopApp.Core.Entities;
using System.Collections.Generic;

namespace ShopApp.Entities.Concrete
{
   public class Cart : IEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
