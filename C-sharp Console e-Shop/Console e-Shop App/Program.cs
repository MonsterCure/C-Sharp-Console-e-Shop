using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Console_e_Shop_Library.Entities;
using Console_e_Shop_Library.Extensions;
using static System.Console; //C# 6.0 feature: using static

namespace Console_e_Shop_App
{
    class Program
    {
        static void Main(string[] args)
        {
            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            #region Vendors and Products
            Vendor stationeryStore = new Vendor("Stationery store");
            Vendor bakery = new Vendor("Bakery");
            Vendor supermarket = new Vendor("Supermarket");
            Vendor furnitureStore = new Vendor("Furniture store");
            Vendor clothesStore = new Vendor("Clothes store");

            List<Product> stationeryStoreProducts = new List<Product>
            {
                new Product("book", 4.57),
                new Product("pen", 0.97),
                new Product("pencil", 1.13),
                new Product("A4 paper", 0.02),
                new Product("envelope", 0.15),
                new Product("bookmark", 5.25)
            };
            stationeryStore.SetVendorsProducts(stationeryStoreProducts);

            List<Product> bakeryProducts = new List<Product>
            {
                new Product("bread", 0.49),
                new Product("pretzel", 0.35),
                new Product("bagel", 0.45),
                new Product("donut", 0.42),
                new Product("scone", 0.39)
            };
            bakery.SetVendorsProducts(bakeryProducts);

            List<Product> supermarketProducts = new List<Product>
            {
                new Product("bag of chips", 1.39),
                new Product("chocolate bar", 1.12),
                new Product("milk", 0.74),
                new Product("yoghurt", 0.72),
                new Product("bottle of water", 0.45)
            };
            supermarket.SetVendorsProducts(supermarketProducts);

            List<Product> furnitureStoreProducts = new List<Product>
            {
                new Product("armchair", 119.99),
                new Product("sofa", 205),
                new Product("table", 134.89),
                new Product("chair", 95.12),
                new Product("cushion", 24.75)
            };
            furnitureStore.SetVendorsProducts(furnitureStoreProducts);

            List<Product> clothesStoreProducts = new List<Product>
            {
                new Product("blouse", 34.78),
                new Product("pants", 37.98),
                new Product("T-shirt", 25.15),
                new Product("sweater", 75.86),
                new Product("jeans", 45.55)
            };
            clothesStore.SetVendorsProducts(clothesStoreProducts);

            Dictionary<string, List<Product>> vendorsAndProducts = new Dictionary<string, List<Product>>() //C# 6.0 feature: dictionary initializer
            {
                [stationeryStore.VendorName] = stationeryStoreProducts,
                [bakery.VendorName] = bakeryProducts,
                [supermarket.VendorName] = supermarketProducts,
                [furnitureStore.VendorName] = furnitureStoreProducts,
                [clothesStore.VendorName] = clothesStoreProducts
            };
            /*vendorsAndProducts.Add(stationeryStore.VendorName, stationeryStoreProducts);
            vendorsAndProducts.Add(bakery.VendorName, bakeryProducts);
            vendorsAndProducts.Add(supermarket.VendorName, supermarketProducts);
            vendorsAndProducts.Add(furnitureStore.VendorName, furnitureStoreProducts);
            vendorsAndProducts.Add(clothesStore.VendorName, clothesStoreProducts);*/
            #endregion

            var user = UserLogin();

            user.OrderPaid += OrderPaidHandler;
            user.OrderShipped += OrderShippedHandler;

            EShopProgram(vendorsAndProducts, user);
        }


        public static string textDivider = "======================================================================================================================";

        #region Logo printing method
        public static void Logo()
        {
            Clear();
            string shopName = "                              ___ _  _ ____                ____    ____ _  _ ____ ___ \n                               |  |__| |___                |___ __ [__  |__| |  | |__]\n                               |  |  | |___                |___    ___] |  | |__| |   \n                                                                                      ";
            Write($"{textDivider}\n{shopName}\n{textDivider}\n\n");
        }
        #endregion

        #region Message printing method
        public static void PrintMessage(string message, ConsoleColor color)
        {
            ForegroundColor = color;
            WriteLine($"\n{message}\n");
            ResetColor();
            WriteLine(textDivider);
        }
        #endregion

        #region Get the vendors names method
        public static void GetVendorsNames(Dictionary<string, List<Product>> vendorsAndProducts)
        {
            PrintMessage("This is the list of vendors:", ConsoleColor.Yellow);
            foreach (var item in vendorsAndProducts)
            {
                WriteLine($"{item.Key}\n");
            }
            WriteLine(textDivider);
        }
        #endregion

        #region Get a vendor's products method
        public static void GetVendorsProducts(Dictionary<string, List<Product>> vendorsAndProducts, string vendorName)
        {
            List<Product> filteredProductsList = GetVendorsProductsAsList(vendorsAndProducts, vendorName);
            string vendorNameKey = vendorsAndProducts.Where(kvp => kvp.Key.ToLower().Contains(vendorName.ToLower())).FirstOrDefault().Key;
            if(!vendorName.ToLower().Trim().Contains(" ") && vendorName.ToLower().Trim().Contains("ore"))
            {
                vendorNameKey = "\"Stationery Store\", \"Furniture Store\" and \"Clothes Store\"";
            }
            if (filteredProductsList != null)
            {
                string filteredProducts = filteredProductsList.PrintProductList();
                PrintMessage($"These are the products from the vendor \"{ vendorNameKey }\":", ConsoleColor.Yellow);
                WriteLine($"{filteredProducts}{textDivider}");
            }
            else
            {
                PrintMessage("The vendor name input didn't match any of the vendors in the catalog. Please try again.", ConsoleColor.DarkRed);
            }
        }
        #endregion

        #region Get a vendor's products as a list method
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
            if(filteredProductsList != null)
            {
                string filteredProducts = filteredProductsList.PrintProductList();
                PrintMessage($"The search by keyword '{productName}' yielded the following products:", ConsoleColor.Yellow);
                WriteLine($"{filteredProducts}{textDivider}");
            }
            else
            {
                PrintMessage("The product name input didn't match any of the products in the catalog. Please try again.", ConsoleColor.DarkRed);
            }
        }
        #endregion

        #region Get products by name as a list method
        public static List<Product> GetProductsByNameAsList(Dictionary<string, List<Product>> vendorsAndProducts, string productName)
        {
            string[] productNames = vendorsAndProducts.SelectMany(kvp => kvp.Value).Select(product => product.ProductName.ToString()).ToArray();
            string productNameChecked = vendorsAndProducts.SelectMany(kvp => kvp.Value).Where(product => product.ProductName.Split(' ').Contains(productName)).FirstOrDefault().ProductName;
            List<Product> filteredProductsList = vendorsAndProducts.SelectMany(kvp => kvp.Value).Where(product => product.ProductName.Split(' ').Contains(productName)).ToList();
            if(filteredProductsList != null)
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
                WriteLine($"{textDivider}");
                PrintMessage($"These are the products that match your search:", ConsoleColor.Yellow);
                WriteLine($"{filteredProducts}{textDivider}");
            }
            else
            {
                PrintMessage("Your input was incorrect. Please try again.", ConsoleColor.DarkRed);
            }
        }
        #endregion

        #region Get all products method
        public static void GetAllProducts(Dictionary<string, List<Product>> vendorsAndProducts)
        {
            List<Product> allProductsList = GetAllProductsAsList(vendorsAndProducts);
            string allProducts = allProductsList.PrintProductList();
            PrintMessage($"These are all the available products from all the vendors:", ConsoleColor.Yellow);
            WriteLine($"{allProducts}{textDivider}");
        }
        #endregion

        #region Get all products as a list method
        public static List<Product> GetAllProductsAsList(Dictionary<string, List<Product>> vendorsAndProducts)
        {
            List<Product> allProductsList = vendorsAndProducts.SelectMany(kvp => kvp.Value).ToList();
            return allProductsList;
        }
        #endregion

        #region User login method
        public static User UserLogin()
        {
            Logo();
            WriteLine("Welcome to The e-Shop\n");
            PrintMessage("Please register by providing a user name (at least 5 characters, letters or numbers):", ConsoleColor.DarkCyan);
            string userName = ReadLine();
            User user;
            Regex regex = new Regex("^[a-zA-Z0-9]*$");
            if (!String.IsNullOrEmpty(userName) && userName.Length >= 5 && regex.IsMatch(userName))
            //if (!String.IsNullOrEmpty(userName) && userName.Length > 5 && userName.All(char.IsLetterOrDigit))
            {
                user = new User(userName);
                Logo();
                PrintMessage($"Thank you for registering, {user.UserName}!\n", ConsoleColor.DarkGreen);
                return user;
            }
            else
            {
                Logo();
                PrintMessage("Wait 3s to try again and input a correct username.\n", ConsoleColor.DarkRed);
                System.Threading.Thread.Sleep(3000);
                return UserLogin();
            }
        }
        #endregion

        #region Search product catalog method
        public static void SearchProductCatalog(Dictionary<string, List<Product>> vendorsAndProducts)
        {
            GetAllProducts(vendorsAndProducts);
            PrintMessage("This is the list of vendors, for your convenience:", ConsoleColor.Yellow);
            GetVendorsNames(vendorsAndProducts);
            WriteLine("In order to search the product catalog, please follow these instructions:\n\n- to search by vendor name input V and to search by product name input P,\n- then input the vendor name (just the first word of the name) or the product name or name of a part (just one word),\n- to view the products sorted by name input N or to view them sorted by price input P,\n- to view them in ascending order input A and to view them in descending order input D.\n\nExample search input:\n\nV bakery P A");
            PrintMessage("Enter your input:", ConsoleColor.DarkCyan);
            string userSearchInput = ReadLine();
            string[] searchParameters = userSearchInput.Split(' ');
            if (searchParameters.Length == 4)
            {
                GetProductsBySearchInput(vendorsAndProducts, searchParameters);
            }
            else
            {
                PrintMessage("Please try again and input the information correctly.", ConsoleColor.DarkRed);
            }
        }
        #endregion

        #region Order a product method
        public static void OrderProduct(User user, Dictionary<string, List<Product>> vendorsAndProducts)
        {
            PrintMessage("To order a product, first type in the vendor name form the list:", ConsoleColor.DarkCyan);
            GetVendorsNames(vendorsAndProducts);
            string vendorName = ReadLine();
            string vendorNameKey = vendorsAndProducts.Where(kvp => kvp.Key.ToLower().Contains(vendorName.ToLower())).FirstOrDefault().Key;
            if (!string.IsNullOrEmpty(vendorNameKey) && !string.IsNullOrEmpty(vendorName))
            {
                GetVendorsProducts(vendorsAndProducts, vendorName);
            }
            else
            {
                PrintMessage("Please try again and enter the vendor name correctly.", ConsoleColor.DarkRed);
                OrderProduct(user, vendorsAndProducts);
            }
            PrintMessage("Now, type in the number of the product and its quantity, separated by a space:", ConsoleColor.DarkCyan);
            string[] orderString = ReadLine().Split();
            List<Product> vendorsProducts = GetVendorsProductsAsList(vendorsAndProducts, vendorName);
            if (orderString.Length == 2 && Int32.TryParse(orderString[0], out int productNumber) && productNumber > 0 && productNumber <= vendorsProducts.Count && Int32.TryParse(orderString[1], out int productQuantity) && productQuantity > 0)
            {
                Product product = vendorsProducts[productNumber - 1];
                Order order = new Order(product.ProductName, productQuantity, product.ProductPrice);
                user.AddToShoppingCart(order);
                PrintMessage($"Successfully created order for {productQuantity} {product.ProductName}((e)s), totaling {(productQuantity * product.ProductPrice).PrintFormattedMKDPrice()}", ConsoleColor.DarkGreen);
            }
            else
            {
                PrintMessage("Please try again and correctly input the necessary information.", ConsoleColor.DarkRed);
            }
        }
        #endregion

        #region View products in cart method
        public static void ViewProductsInCart(User user)
        {
            if (user.GetShoppingCart().Count > 0)
            {
                PrintMessage("These are the orders you have made so far:", ConsoleColor.Yellow);
                user.GetShoopingCartProducts();
                WriteLine(textDivider);
            }
            else
            {
                PrintMessage("Your shopping cart is empty.", ConsoleColor.Yellow);
            }
        }
        #endregion

        #region Remove order method
        public static void RemoveOrder(User user)
        {
            if(user.GetShoppingCart().Count > 0)
            {
                PrintMessage("Choose which of the orders you want to remove and type in the number:", ConsoleColor.DarkCyan);
                user.GetShoopingCartProducts();
                string orderNumberString = ReadLine();
                if (Int32.TryParse(orderNumberString, out int orderNumber) && orderNumber > 0 && orderNumber <= user.GetShoppingCart().Count)
                {
                    PrintMessage($"Successfully removed order for {user.GetShoppingCart()[orderNumber - 1].ProductQuantity} {user.GetShoppingCart()[orderNumber - 1].ProductName}((e)s), totaling {user.GetShoppingCart()[orderNumber - 1].GetOrderPrice().PrintFormattedMKDPrice()}", ConsoleColor.DarkGreen);
                    user.RemoveFromShoppingCart(orderNumber);
                }
                else
                {
                    PrintMessage("Please try again and correctly input the necessary information.", ConsoleColor.DarkRed);
                }
            }
            else
            {
                PrintMessage("There are no orders to be removed because your shopping cart is empty.", ConsoleColor.Yellow);
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
                if (new[] { "MP", "mp"}.Any(c => shippingParameters[3].Trim().Contains(c)))
                {
                    shippingProvider = "Makedonska Posta";
                } else if (new[] { "D", "d" }.Any(c => shippingParameters[3].Trim().Contains(c)))
                {
                    shippingProvider = "DELCO";
                }
                string shippingInfo = $"{shippingParameters[0].Trim()}, {shippingParameters[1].Trim()}, {shippingParameters[2].Trim()}, through {shippingProvider}";
                user.OnOrderShipped(shippingInfo);
            }
            else
            {
                PrintMessage("Please try again and input the information correctly.", ConsoleColor.DarkRed);
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
                PrintMessage("Please try again and input the information correctly.", ConsoleColor.DarkRed);
                EnterPaymentMethod(user);
            }
        }
        #endregion

        #region Get orders by price method
        public static void GetOrdersByPrice(User user)
        {
            if (user.PurchasedOrders.Count == 0)
            {
                PrintMessage("You have no purchased orders.", ConsoleColor.Yellow);
            } else if(user.PurchasedOrders.Count > 0)
            {
                PrintMessage("\nTo view your orders below 1.000,00 MKD, enter 1\n\nTo view your orders above 1.000,00 MKD, enter 2\n\nTo view all your orders, enter 3\n", ConsoleColor.Yellow);
                string userInput = ReadLine();
                if (Int32.TryParse(userInput, out int userInputNumber) && userInputNumber > 0 && userInputNumber <= 3)
                {
                    switch (userInputNumber)
                    {
                        case 1:
                            user.GetCheapOrders();
                            WriteLine(textDivider);
                            break;
                        case 2:
                            user.GetExpensiveOrders();
                            WriteLine(textDivider);
                            break;
                        case 3:
                            user.GetCheapOrders();
                            WriteLine(textDivider);
                            user.GetExpensiveOrders();
                            WriteLine(textDivider);
                            break;
                    }
                }
                else
                {
                    PrintMessage("Please try again and input 1, 2 or 3.", ConsoleColor.DarkRed);
                    GetOrdersByPrice(user);
                }
            }
        }
        #endregion

        #region e-Shop main program method
        public static void EShopProgram(Dictionary<string, List<Product>> vendorsAndProducts, User user)
        {
            WriteLine("Choose what you want to do:\n\n1. Get a list of all the vendors\n2. Search the product catalog by vendor name or product name\n3. Create an order for a product\n4. View the products in the shopping cart\n5. Remove an order for a product\n6. Get an order receipt, proceed to payment and provide a shipping address\n7. View your purchased orders\n8. Exit the shop\n");

            string userInput = ReadLine();
            if (!Int32.TryParse(userInput, out int userInputNumber) || userInputNumber < 0 || userInputNumber > 9)
            {
                Logo();
                PrintMessage("Please try again and input a number between 1 and 8", ConsoleColor.DarkRed);
                EShopProgram(vendorsAndProducts, user);
            }
            else
            {
                Logo();
                switch (userInputNumber)
                {
                    case 1:
                        GetVendorsNames(vendorsAndProducts);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 2:
                        SearchProductCatalog(vendorsAndProducts);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 3:
                        OrderProduct(user, vendorsAndProducts);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 4:
                        ViewProductsInCart(user);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 5:
                        RemoveOrder(user);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 6:
                        if (user.GetShoppingCart().Count > 0)
                        {
                            user.PrintOrderReceipt();
                            WriteLine($"{textDivider}\n");
                            EnterShippingAddress(user);
                            EnterPaymentMethod(user);
                        }
                        else
                        {
                            PrintMessage("You have no products in your shopping cart yet.", ConsoleColor.Yellow);
                        }
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 7:
                        GetOrdersByPrice(user);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                    case 8:
                        PrintMessage($"Thank you for visiting our shop, {user.UserName}! Please visit us again.", ConsoleColor.DarkGreen);
                        Environment.Exit(0);
                        break;
                    default:
                        Logo();
                        PrintMessage("Please try again and input a number between 1 and 8", ConsoleColor.DarkRed);
                        EShopProgram(vendorsAndProducts, user);
                        break;
                }

            }
        }
        #endregion

        #region Event handler method for the shipping
        public static void OrderShippedHandler(string message) => PrintMessage($"Your order will be shipped to {message}.", ConsoleColor.DarkGreen); //C# 6.0 feature: expression bodied method
        #endregion

        #region Event handler method for the payment
        public static void OrderPaidHandler(string message) => PrintMessage($"Your payment through {message} was successful. Thank you for shopping with us!", ConsoleColor.DarkGreen); //C# 6.0 feature: expression bodied method
        #endregion

        #region Global error handler method
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            PrintMessage("A critical error occured. Press any key to exit the application.", ConsoleColor.DarkRed);
            Environment.Exit(1);
        }
        #endregion
    }
}
