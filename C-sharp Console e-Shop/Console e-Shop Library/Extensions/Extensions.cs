using System;
using System.Collections.Generic;
using System.Globalization;

using Console_e_Shop_Library.Entities;

namespace Console_e_Shop_Library.Extensions
{
    public static class Extensions
    {
        /* Extension method that prints a double as a price in MKD */
        public static string PrintFormattedMKDPrice(this double price)
        {
            price = price * 61.8;
            CultureInfo mk = new CultureInfo("mk-MK");
            var numberFormatInfo = (NumberFormatInfo) mk.NumberFormat.Clone();
            numberFormatInfo.CurrencySymbol = "MKD";
            return price.ToString("C", numberFormatInfo);
        }

        /* Extension method that returns a string of information for a list of Product objects */
        public static string PrintProductList(this List<Product> list)
        {
            string products = "";
            int number = 1;
            foreach (var product in list)
            {
                products += $"Product {number}: {product.ProductName}   Price: {product.ProductPrice.PrintFormattedMKDPrice()}\n\n";
                number++;
            }
            return products;
        }

        /* Extension method that returns a string of information for a list of Order objects */
        public static string PrintOrderList(this List<Order> list)
        {
            string products = "";
            int number = 1;
            foreach (var product in list)
            {
                products += $"{number}. {product.ProductName} Quantity: {product.ProductQuantity} Price: {product.GetOrderPriceString()}\n";
                number++;
            }
            return products;
        }

        /* Extension method that prints a list of Order objects using a two-dimensional string matrix*/
        public static void PrintOrderListAsReceipt(this List<Order> list)
        {
            string[,] products = new String[list.Count, 4];
            string[] productsTitleArray = "No. Product Quantity Price".Split();
            for (int i = 0; i < productsTitleArray.Length; i++)
            {
                Console.Write("{0, -25}", productsTitleArray[i]);
            }
            Console.WriteLine("\n-------------------------------------------------------------------------------------------------------\n");
            for (int row = 0; row < products.GetLength(0); row++)
            {
                for (int column = 0; column < products.GetLength(1); column++)
                {
                    string productName = list[row].ProductName;
                    int productQuantity = list[row].ProductQuantity;
                    string price = list[row].GetOrderPrice().PrintFormattedMKDPrice();
                    string[] productsArray = $"{row + 1}.-{productName}-{productQuantity}-{price}".Split('-');
                    products[row, column] = $"{productsArray[column]}";
                    Console.Write("{0, -25}", products[row, column]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("-------------------------------------------------------------------------------------------------------\n");
        }
    }
}
