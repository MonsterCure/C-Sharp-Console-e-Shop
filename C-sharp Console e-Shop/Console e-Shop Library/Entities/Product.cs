using System.Collections.Generic;

namespace Console_e_Shop_Library.Entities
{
    public class Product : IItem
    {
        public static List<Product> Products { get; set; } = new List<Product>();  //C# 6.0 feature: auto-property initializer

        public string ProductName { get; set; }
        public double ProductPrice { get; set; }

        public Product() => Products.Add(this); //C# 7.0 feature: expression bodied (default) constructor

        public Product(string productName, double productPrice)
        {
            this.ProductName = productName;
            this.ProductPrice = productPrice;
        }
    }
}
