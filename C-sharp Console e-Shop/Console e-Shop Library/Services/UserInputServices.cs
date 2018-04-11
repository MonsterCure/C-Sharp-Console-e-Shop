using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using static System.Console;

using Console_e_Shop_Library.Entities;
using Console_e_Shop_Library.Repositories;
using Console_e_Shop_Library.Extensions;

namespace Console_e_Shop_Library.Services
{
    public static class UserInputServices
    {
        #region User login method
        public static User UserLogin()
        {
            Message.Logo();
            WriteLine("Welcome to The e-Shop\n");
            Message.Print("Please register by providing a user name (at least 5 characters, letters or numbers):", ConsoleColor.DarkCyan);
            string userName = ReadLine();
            User user;
            Regex regex = new Regex("^[a-zA-Z0-9]*$");
            if (!String.IsNullOrEmpty(userName) && userName.Length >= 5 && regex.IsMatch(userName))
            //if (!String.IsNullOrEmpty(userName) && userName.Length > 5 && userName.All(char.IsLetterOrDigit))
            {
                user = new User(userName);
                Message.Logo();
                Message.Print($"Thank you for registering, {user.UserName}!\n", ConsoleColor.DarkGreen);
                return user;
            }
            else
            {
                Message.Logo();
                Message.Print("Wait 3s to try again and input a correct username.\n", ConsoleColor.DarkRed);
                System.Threading.Thread.Sleep(3000);
                return UserLogin();
            }
        }
        #endregion

        #region Get a vendor's products method (by vendor name)
        public static void GetVendorsProducts(Dictionary<string, List<Product>> vendorsAndProducts, string vendorName)
        {
            List<Product> filteredProductsList = GetVendorsProductsAsList(vendorsAndProducts, vendorName);
            string vendorNameKey = vendorsAndProducts.Where(kvp => kvp.Key.ToLower().Contains(vendorName.ToLower())).FirstOrDefault().Key;
            if (!vendorName.ToLower().Trim().Contains(" ") && vendorName.ToLower().Trim().Contains("ore"))
            {
                vendorNameKey = "\"Stationery Store\", \"Furniture Store\" and \"Clothes Store\"";
            }
            if (filteredProductsList != null)
            {
                string filteredProducts = filteredProductsList.PrintProductList();
                Message.Print($"These are the products from the vendor \"{ vendorNameKey }\":", ConsoleColor.Yellow);
                WriteLine($"{filteredProducts}{Message.textDivider}");
            }
            else
            {
                Message.Print("The vendor name input didn't match any of the vendors in the catalog. Please try again.", ConsoleColor.DarkRed);
            }
        }
        #endregion

        #region Get a vendor's products as a list method (by vendor name)
        public static List<Product> GetVendorsProductsAsList(Dictionary<string, List<Product>> vendorsAndProducts, string vendorName)
        {
            List<Product> filteredProductsList = vendorsAndProducts.Where(kvp => kvp.Key.ToLower().Contains(vendorName.ToLower())).SelectMany(kvp => kvp.Value).ToList();
            if (filteredProductsList != null)
            {
                return filteredProductsList;
            }
            else
            {
                return filteredProductsList = null;
            }
        }
        #endregion

        #region Get products by name method
        public static void GetProductsByName(Dictionary<string, List<Product>> vendorsAndProducts, string productName)
        {
            List<Product> filteredProductsList = GetProductsByNameAsList(vendorsAndProducts, productName);
            if (filteredProductsList != null)
            {
                string filteredProducts = filteredProductsList.PrintProductList();
                Message.Print($"The search by keyword '{productName}' yielded the following products:", ConsoleColor.Yellow);
                WriteLine($"{filteredProducts}{Message.textDivider}");
            }
            else
            {
                Message.Print("The product name input didn't match any of the products in the catalog. Please try again.", ConsoleColor.DarkRed);
            }
        }
        #endregion

        #region Get products by name as a list method
        public static List<Product> GetProductsByNameAsList(Dictionary<string, List<Product>> vendorsAndProducts, string productName)
        {
            string[] productNames = vendorsAndProducts.SelectMany(kvp => kvp.Value).Select(product => product.ProductName.ToString()).ToArray();
            string productNameChecked = vendorsAndProducts.SelectMany(kvp => kvp.Value).Where(product => product.ProductName.Split(' ').Contains(productName)).FirstOrDefault().ProductName;
            List<Product> filteredProductsList = vendorsAndProducts.SelectMany(kvp => kvp.Value).Where(product => product.ProductName.Split(' ').Contains(productName)).ToList();
            if (filteredProductsList != null)
            {
                return filteredProductsList;
            }
            else
            {
                return filteredProductsList = null;
            }
        }
        #endregion

        #region Get products by search input method
        public static void GetProductsBySearchInput(Dictionary<string, List<Product>> vendorsAndProducts, string[] searchParameters)
        {
            IEnumerable<Product> filteredProductsList;
            List<Product> orderedProductsList;
            string vendorName = vendorsAndProducts.Where(kvp => kvp.Key.ToLower().Contains(searchParameters[1].ToLower())).FirstOrDefault().Key;
            //string productName = vendorsAndProducts.SelectMany(kvp => kvp.Value).Where(product => product.ProductName.ToLower().Split(' ').Contains(searchParameters[1].ToLower())).FirstOrDefault().ProductName;
            string[] productNames = vendorsAndProducts.SelectMany(kvp => kvp.Value).Select(product => product.ProductName.ToString()).ToArray();
            if (searchParameters[0].ToLower() == "v" && vendorName != null)
            {
                filteredProductsList = vendorsAndProducts.Where(kvp => kvp.Key.ToLower().Contains(searchParameters[1].ToLower())).SelectMany(kvp => kvp.Value);
            }
            else if (searchParameters[0].ToLower() == "p" && productNames.Contains(searchParameters[1]))
            {
                filteredProductsList = vendorsAndProducts.SelectMany(kvp => kvp.Value).Where(product => product.ProductName.ToLower().Split(' ').Contains(searchParameters[1].ToLower()));
            }
            else
            {
                filteredProductsList = null;
            }

            if (searchParameters[2].ToLower() == "n" && filteredProductsList != null)
            {
                filteredProductsList = filteredProductsList.OrderBy(product => product.ProductName);
            }
            else if (searchParameters[2].ToLower() == "p" && filteredProductsList != null)
            {
                filteredProductsList = filteredProductsList.OrderBy(product => product.ProductPrice);
            }
            else
            {
                filteredProductsList = null;
            }

            if (searchParameters[3].ToLower() == "a" && filteredProductsList != null)
            {
                orderedProductsList = filteredProductsList.ToList();
            }
            else if (searchParameters[3].ToLower() == "d" && filteredProductsList != null)
            {
                orderedProductsList = filteredProductsList.Reverse().ToList();
            }
            else
            {
                orderedProductsList = null;
            }

            if (filteredProductsList != null && orderedProductsList != null)
            {
                string filteredProducts = orderedProductsList.PrintProductList();
                WriteLine($"{Message.textDivider}");
                Message.Print($"These are the products that match your search:", ConsoleColor.Yellow);
                WriteLine($"{filteredProducts}{Message.textDivider}");
            }
            else
            {
                Message.Print("Your input was incorrect. Please try again.", ConsoleColor.DarkRed);
            }
        }
        #endregion

        #region Search product catalog method
        public static void SearchProductCatalog(Dictionary<string, List<Product>> vendorsAndProducts)
        {
            VendorsRepository.GetAllProducts(vendorsAndProducts);
            Message.Print("This is the list of vendors, for your convenience:", ConsoleColor.Yellow);
            VendorsRepository.GetVendorsNames(vendorsAndProducts);
            WriteLine("In order to search the product catalog, please follow these instructions:\n\n- to search by vendor name input V and to search by product name input P,\n- then input the vendor name (just the first word of the name) or the product name or name of a part (just one word),\n- to view the products sorted by name input N or to view them sorted by price input P,\n- to view them in ascending order input A and to view them in descending order input D.\n\nExample search input:\n\nV bakery P A");
            Message.Print("Enter your input:", ConsoleColor.DarkCyan);
            string userSearchInput = ReadLine();
            string[] searchParameters = userSearchInput.Split(' ');
            if (searchParameters.Length == 4)
            {
                UserInputServices.GetProductsBySearchInput(vendorsAndProducts, searchParameters);
            }
            else
            {
                Message.Print("Please try again and input the information correctly.", ConsoleColor.DarkRed);
            }
        }
        #endregion

        #region Order a product method
        public static void OrderProduct(User user, Dictionary<string, List<Product>> vendorsAndProducts)
        {
            Message.Print("To order a product, first type in the vendor name form the list:", ConsoleColor.DarkCyan);
            VendorsRepository.GetVendorsNames(vendorsAndProducts);
            string vendorName = ReadLine();
            string vendorNameKey = vendorsAndProducts.Where(kvp => kvp.Key.ToLower().Contains(vendorName.ToLower())).FirstOrDefault().Key;
            if (!string.IsNullOrEmpty(vendorNameKey) && !string.IsNullOrEmpty(vendorName))
            {
                UserInputServices.GetVendorsProducts(vendorsAndProducts, vendorName);
            }
            else
            {
                Message.Print("Please try again and enter the vendor name correctly.", ConsoleColor.DarkRed);
                OrderProduct(user, vendorsAndProducts);
            }
            Message.Print("Now, type in the number of the product and its quantity, separated by a space:", ConsoleColor.DarkCyan);
            string[] orderString = ReadLine().Split();
            List<Product> vendorsProducts = UserInputServices.GetVendorsProductsAsList(vendorsAndProducts, vendorName);
            if (orderString.Length == 2 && Int32.TryParse(orderString[0], out int productNumber) && productNumber > 0 && productNumber <= vendorsProducts.Count && Int32.TryParse(orderString[1], out int productQuantity) && productQuantity > 0)
            {
                Product product = vendorsProducts[productNumber - 1];
                Order order = new Order(product.ProductName, productQuantity, product.ProductPrice);
                user.AddToShoppingCart(order);
                Message.Print($"Successfully created order for {productQuantity} {product.ProductName}((e)s), totaling {(productQuantity * product.ProductPrice).PrintFormattedMKDPrice()}", ConsoleColor.DarkGreen);
            }
            else
            {
                Message.Print("Please try again and correctly input the necessary information.", ConsoleColor.DarkRed);
            }
        }
        #endregion

        #region View products in cart method
        public static void ViewProductsInCart(User user)
        {
            if (user.GetShoppingCart().Count > 0)
            {
                Message.Print("These are the orders you have made so far:", ConsoleColor.Yellow);
                user.GetShoopingCartProducts();
                WriteLine(Message.textDivider);
            }
            else
            {
                Message.Print("Your shopping cart is empty.", ConsoleColor.Yellow);
            }
        }
        #endregion

        #region Remove order method
        public static void RemoveOrder(User user)
        {
            if (user.GetShoppingCart().Count > 0)
            {
                Message.Print("Choose which of the orders you want to remove and type in the number:", ConsoleColor.DarkCyan);
                user.GetShoopingCartProducts();
                string orderNumberString = ReadLine();
                if (Int32.TryParse(orderNumberString, out int orderNumber) && orderNumber > 0 && orderNumber <= user.GetShoppingCart().Count)
                {
                    Message.Print($"Successfully removed order for {user.GetShoppingCart()[orderNumber - 1].ProductQuantity} {user.GetShoppingCart()[orderNumber - 1].ProductName}((e)s), totaling {user.GetShoppingCart()[orderNumber - 1].GetOrderPrice().PrintFormattedMKDPrice()}", ConsoleColor.DarkGreen);
                    user.RemoveFromShoppingCart(orderNumber);
                }
                else
                {
                    Message.Print("Please try again and correctly input the necessary information.", ConsoleColor.DarkRed);
                }
            }
            else
            {
                Message.Print("There are no orders to be removed because your shopping cart is empty.", ConsoleColor.Yellow);
            }
        }
        #endregion

        #region Enter shipping address method
        public static void EnterShippingAddress(User user)
        {
            WriteLine("Orders can only be shipped to Skopje, Bitola, Ohrid and Stip,\nusing these two shipping providers:\n- Makedonska Posta (MP)\n- DELCO (D).\n\nEnter your address and preffered shipping provider in the following format:\n- street, number, place, shipping provider\n- example: Partizanski Odredi, 1, Skopje, MP.\n");
            string userShippingInput = ReadLine();
            string[] shippingParameters = userShippingInput.Split(',');
            if (shippingParameters.Length == 4 && shippingParameters[0].Trim().Length > 1 && !shippingParameters[0].Trim().Any(c => char.IsDigit(c)) && shippingParameters[1].Trim().Any(c => char.IsDigit(c)) && new[] { "skopje", "bitola", "ohrid", "stip" }.Any(c => shippingParameters[2].Trim().ToLower().Contains(c)) && new[] { "MP", "D", "mp", "d" }.Any(c => shippingParameters[3].Trim().Contains(c)))
            {
                string shippingProvider = "";
                if (new[] { "MP", "mp" }.Any(c => shippingParameters[3].Trim().Contains(c)))
                {
                    shippingProvider = "Makedonska Posta";
                }
                else if (new[] { "D", "d" }.Any(c => shippingParameters[3].Trim().Contains(c)))
                {
                    shippingProvider = "DELCO";
                }
                string shippingInfo = $"{shippingParameters[0].Trim()}, {shippingParameters[1].Trim()}, {shippingParameters[2].Trim()}, through {shippingProvider}";
                user.OnOrderShipped(shippingInfo);
            }
            else
            {
                Message.Print("Please try again and input the information correctly.", ConsoleColor.DarkRed);
                EnterShippingAddress(user);
            }
        }
        #endregion

        #region Enter payment method method
        public static void EnterPaymentMethod(User user)
        {
            WriteLine("\nChoose a payment method:\n- credit card (enter C)\n- PayPal (enter P)\n");
            string userPaymentInput = ReadLine();
            string paymentInfo;
            if (userPaymentInput.ToLower() == "c")
            {
                paymentInfo = "credit card";
                user.OnOrderPaid(paymentInfo);
            }
            else if (userPaymentInput.ToLower() == "p")
            {
                paymentInfo = "PayPal";
                user.OnOrderPaid(paymentInfo);
            }
            else
            {
                Message.Print("Please try again and input the information correctly.", ConsoleColor.DarkRed);
                EnterPaymentMethod(user);
            }
        }
        #endregion

        #region Get orders by price method
        public static void GetOrdersByPrice(User user)
        {
            if (user.PurchasedOrders.Count == 0)
            {
                Message.Print("You have no purchased orders.", ConsoleColor.Yellow);
            }
            else if (user.PurchasedOrders.Count > 0)
            {
                Message.Print("\nTo view your orders below 1.000,00 MKD, enter 1\n\nTo view your orders above 1.000,00 MKD, enter 2\n\nTo view all your orders, enter 3\n", ConsoleColor.Yellow);
                string userInput = ReadLine();
                if (Int32.TryParse(userInput, out int userInputNumber) && userInputNumber > 0 && userInputNumber <= 3)
                {
                    switch (userInputNumber)
                    {
                        case 1:
                            user.GetCheapOrders();
                            WriteLine(Message.textDivider);
                            break;
                        case 2:
                            user.GetExpensiveOrders();
                            WriteLine(Message.textDivider);
                            break;
                        case 3:
                            user.GetCheapOrders();
                            WriteLine(Message.textDivider);
                            user.GetExpensiveOrders();
                            WriteLine(Message.textDivider);
                            break;
                    }
                }
                else
                {
                    Message.Print("Please try again and input 1, 2 or 3.", ConsoleColor.DarkRed);
                    GetOrdersByPrice(user);
                }
            }
        }
        #endregion
    }
}
