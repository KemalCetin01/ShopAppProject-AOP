using ShopApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public String ImageUrl { get; set; }
        public string Description { get; set; }
        public string Dimensions { get; set; } //Boyut
        public decimal? SequenceMeter { get; set; }//MetreKare
        public decimal? WarrantyPeriod { get; set; } //garanti süresi
        public String Modell { get; set; }
        public string Material { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public short UnitsInStock { get; set; }

        public int CategoryId { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}
