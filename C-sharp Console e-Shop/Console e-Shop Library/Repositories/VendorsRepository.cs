using System;
using System.Collections.Generic;
using System.Linq;

using static System.Console;  //C# 6.0 feature: using static

using Console_e_Shop_Library.Entities;
using Console_e_Shop_Library.Extensions;

namespace Console_e_Shop_Library.Repositories
{
    public static class VendorsRepository
    {
        #region Vendors and Products
        public static readonly Dictionary<string, List<Product>> VendorsAndProducts = new Dictionary<string, List<Product>>() //C# 6.0 feature: dictionary initializer
        {
            [new Vendor("Stationery store").VendorName] = new List<Product>
            {
                new Product("book", 4.57),
                new Product("pen", 0.97),
                new Product("pencil", 1.13),
                new Product("A4 paper", 0.02),
                new Product("envelope", 0.15),
                new Product("bookmark", 5.25)
            },
            [new Vendor("Bakery").VendorName] = new List<Product>
            {
                new Product("bread", 0.49),
                new Product("pretzel", 0.35),
                new Product("bagel", 0.45),
                new Product("donut", 0.42),
                new Product("scone", 0.39)
            },
            [new Vendor("Supermarket").VendorName] = new List<Product>
            {
                new Product("bag of chips", 1.39),
                new Product("chocolate bar", 1.12),
                new Product("milk", 0.74),
                new Product("yoghurt", 0.72),
                new Product("bottle of water", 0.45)
            },
            [new Vendor("Furniture store").VendorName] = new List<Product>
            {
                new Product("armchair", 119.99),
                new Product("sofa", 205),
                new Product("table", 134.89),
                new Product("chair", 95.12),
                new Product("cushion", 24.75)
            },
            [new Vendor("Clothes store").VendorName] = new List<Product>
            {
                new Product("blouse", 34.78),
                new Product("pants", 37.98),
                new Product("T-shirt", 25.15),
                new Product("sweater", 75.86),
                new Product("jeans", 45.55)
            },
        };
        #endregion

        #region Get the vendors names method
        public static void GetVendorsNames(Dictionary<string, List<Product>> vendorsAndProducts)
        {
            Message.Print("This is the list of vendors:", ConsoleColor.Yellow);
            foreach (var item in vendorsAndProducts)
            {
                WriteLine($"{item.Key}\n");
            }
            WriteLine(Message.textDivider);
        }
        #endregion

        #region Get all products method
        public static void GetAllProducts(Dictionary<string, List<Product>> vendorsAndProducts)
        {
            List<Product> allProductsList = GetAllProductsAsList(vendorsAndProducts);
            string allProducts = allProductsList.PrintProductList();
            Message.Print($"These are all the available products from all the vendors:", ConsoleColor.Yellow);
            WriteLine($"{allProducts}{Message.textDivider}");
        }
        #endregion

        #region Get all products as a list method
        public static List<Product> GetAllProductsAsList(Dictionary<string, List<Product>> vendorsAndProducts)
        {
            List<Product> allProductsList = vendorsAndProducts.SelectMany(kvp => kvp.Value).ToList();
            return allProductsList;
        }
        #endregion
    }
}
