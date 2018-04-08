using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Console_e_Shop_Library.Extensions;

namespace Console_e_Shop_Library.Entities
{
    public delegate void OrderShipped(string message);
    public delegate void OrderPaid(string message);

    public class User
    {
        public int Id { get; private set; }
        private static int _count = 1;
        public string UserName { get; set; }
        private static List<User> _users = new List<User>();
        private List<Order> _shoppingCart = new List<Order>();
        public List<Tuple<double, List<Order>>> PurchasedOrders { get; set; } = new List<Tuple<double, List<Order>>>(); //C# 6.0 feature: auto-property initializer

        public User(string userName)
        {
            this.Id = _count++;
            this.UserName = userName;
            _users.Add(this);
        }

        public List<Order> GetShoppingCart() => _shoppingCart; //C# 6.0 feature: expression bodied method

        public void AddToShoppingCart(Order order) => _shoppingCart.Add(order); //C# 6.0 feature: expression bodied method

        public void RemoveFromShoppingCart(int orderNumber) => _shoppingCart.RemoveAt(orderNumber - 1); //C# 6.0 feature: expression bodied method

        public void GetShoopingCartProducts() => this._shoppingCart.PrintOrderListAsReceipt(); //C# 6.0 feature: expression bodied method

        public void EmptyShoppingCart() => this._shoppingCart.RemoveRange(0, this._shoppingCart.Count); //C# 6.0 feature: expression bodied method

        public double GetTotalPrice()
        {
            double totalPrice = 0;
            foreach (var order in _shoppingCart)
            {
                totalPrice += order.GetOrderPrice();
            }
            return totalPrice;
        }


        public void AddToPurchasedOrders()
        {
            //List<Order> copy = this._shoppingCart.ToList();
            //Tuple<double, List<Order>> item = new Tuple<double, List<Order>>(this.GetTotalPrice(), copy);
            Tuple<double, List<Order>> item = new Tuple<double, List<Order>>(this.GetTotalPrice(), this._shoppingCart.ToList());
            PurchasedOrders.Add(item);
        }

        public void PrintOrderReceipt()
        {
            this.AddToPurchasedOrders();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Order receipt for user: {this.UserName}\n");
            this._shoppingCart.PrintOrderListAsReceipt();
            Console.WriteLine("{0, 80}", $"Total price: {this.GetTotalPrice().PrintFormattedMKDPrice()}\n");
            Console.ResetColor();
            this.EmptyShoppingCart();
        }

        public void GetCheapOrders()
        {
            List<Tuple<double, List<Order>>> CheapOrders = new List<Tuple<double, List<Order>>>();
            foreach (var order in this.PurchasedOrders)
            {
                if ((order.Item1 * 61.8) > 0 && (order.Item1 * 61.8) <= 1000)
                {
                    Tuple<double, List<Order>> item = new Tuple<double, List<Order>>(order.Item1, order.Item2);
                    CheapOrders.Add(item);
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nOrders BELOW 1.000,00 MKD:");
            Console.ResetColor();
            if (CheapOrders.Count > 0)
            {
                foreach (var order in CheapOrders)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("_______________________________________________________________________________________________________");
                    Console.ResetColor();
                    order.Item2.PrintOrderListAsReceipt();
                    Console.WriteLine($"Total price: {order.Item1.PrintFormattedMKDPrice()}");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n_______________________________________________________________________________________________________\n");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("\nYou have no such purchased orders.\n");
            }
        }

        public void GetExpensiveOrders()
        {
            List<Tuple<double, List<Order>>> ExpensiveOrders = new List<Tuple<double, List<Order>>>();
            foreach (var order in this.PurchasedOrders)
            {
                if ((order.Item1 * 61.8) > 1000)
                {
                    Tuple<double, List<Order>> item = new Tuple<double, List<Order>>(order.Item1, order.Item2);
                    ExpensiveOrders.Add(item);
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nOrders ABOVE 1.000,00 MKD:");
            Console.ResetColor();
            if (ExpensiveOrders.Count > 0)
            {
                foreach (var order in ExpensiveOrders)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("_______________________________________________________________________________________________________");
                    Console.ResetColor();
                    order.Item2.PrintOrderListAsReceipt();
                    Console.WriteLine($"Total price: {order.Item1.PrintFormattedMKDPrice()}");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\n_______________________________________________________________________________________________________\n");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.WriteLine("\nYou have no such purchased orders.\n");
            }
        }

        private OrderShipped _handlerShipped;
        public event OrderShipped OrderShipped
        {
            add
            {
                _handlerShipped += value;
            }
            remove
            {
                _handlerShipped -= value;
            }
        }

        public void OnOrderShipped(string message) => _handlerShipped?.Invoke(message); //C# 6.0 feature: expression bodied method
                                                                                        //C# 6.0 feature: conditional access (null propragator)

        private OrderPaid _handlerPaid;
        public event OrderPaid OrderPaid
        {
            add
            {
                _handlerPaid += value;
            }
            remove
            {
                _handlerPaid -= value;
            }
        }

        public void OnOrderPaid(string message) => _handlerPaid?.Invoke(message); //C# 6.0 feature: expression bodied method
                                                                                  //C# 6.0 feature: conditional access (null propragator)
    }
}
