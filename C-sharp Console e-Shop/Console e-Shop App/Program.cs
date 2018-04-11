using System;
using System.Collections.Generic;

using static System.Console; //C# 6.0 feature: using static

using Console_e_Shop_Library.Entities;
using Console_e_Shop_Library.Repositories;
using Console_e_Shop_Library.Services;

namespace Console_e_Shop_App
{
    class Program
    {
        static void Main(string[] args)
        {
            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var user = UserInputServices.UserLogin();

            user.OrderPaid += OrderPaidHandler;
            user.OrderShipped += OrderShippedHandler;

            EShopProgram(VendorsRepository.VendorsAndProducts, user);
        }
        
        #region e-Shop main program method
        public static void EShopProgram(Dictionary<string, List<Product>> vendorsAndProducts, User user)
        {
            WriteLine("Choose what you want to do:\n\n1. Get a list of all the vendors\n2. Search the product catalog by vendor name or product name\n3. Create an order for a product\n4. View the products in the shopping cart\n5. Remove an order for a product\n6. Get an order receipt, proceed to payment and provide a shipping address\n7. View your purchased orders\n8. Exit the shop\n");

            string userInput = ReadLine();
            if (!Int32.TryParse(userInput, out int userInputNumber) || userInputNumber < 0 || userInputNumber > 9)
            {
                Message.Logo();
                Message.Print("Please try again and input a number between 1 and 8", ConsoleColor.DarkRed);
                EShopProgram(vendorsAndProducts, user);
            }
            else
            {
                Message.Logo();
                switch (userInputNumber)
                {
                    case 1:
                        VendorsRepository.GetVendorsNames(vendorsAndProducts);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 2:
                        UserInputServices.SearchProductCatalog(vendorsAndProducts);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 3:
                        UserInputServices.OrderProduct(user, vendorsAndProducts);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 4:
                        UserInputServices.ViewProductsInCart(user);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 5:
                        UserInputServices.RemoveOrder(user);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 6:
                        if (user.GetShoppingCart().Count > 0)
                        {
                            user.PrintOrderReceipt();
                            WriteLine($"{Message.textDivider}\n");
                            UserInputServices.EnterShippingAddress(user);
                            UserInputServices.EnterPaymentMethod(user);
                        }
                        else
                        {
                            Message.Print("You have no products in your shopping cart yet.", ConsoleColor.Yellow);
                        }
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 7:
                        UserInputServices.GetOrdersByPrice(user);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 8:
                        Message.Print($"Thank you for visiting our shop, {user.UserName}! Please visit us again.", ConsoleColor.DarkGreen);
                        Environment.Exit(0);
                        break;
                    default:
                        Message.Logo();
                        Message.Print("Please try again and input a number between 1 and 8", ConsoleColor.DarkRed);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                }

            }
        }
        #endregion

        #region Event handler method for the shipping
        public static void OrderShippedHandler(string message) => Message.Print($"Your order will be shipped to {message}.", ConsoleColor.DarkGreen); //C# 6.0 feature: expression bodied method
        #endregion

        #region Event handler method for the payment
        public static void OrderPaidHandler(string message) => Message.Print($"Your payment through {message} was successful. Thank you for shopping with us!", ConsoleColor.DarkGreen); //C# 6.0 feature: expression bodied method
        #endregion

        #region Global error handler method
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Message.Print("A critical error occured. Press any key to exit the application.", ConsoleColor.DarkRed);
            Environment.Exit(1);
        }
        #endregion
    }
}
