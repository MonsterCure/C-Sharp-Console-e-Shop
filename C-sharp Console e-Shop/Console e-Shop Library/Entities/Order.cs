using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console_e_Shop_Library.Extensions;

namespace Console_e_Shop_Library.Entities
{
    public class Order : IItem
    {
        public int Id { get; private set; }
        private static int _count = 1;
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public double ProductPrice { get; set; }
        private List<Order> _orders = new List<Order>();

        public Order(string productName, int quantity, double productPrice)
        {
            this.Id = _count++;
            this.ProductName = productName;
            this.ProductQuantity = quantity;
            this.ProductPrice = productPrice;
            _orders.Add(this);
        }

        public void SetProductQuantity(int productQuantity)
        {
            if (productQuantity <= 0)
            {
                productQuantity = 0;
            }
            this.ProductQuantity = productQuantity;
        }

        public double GetOrderPrice()
        {
            double price = this.ProductPrice * this.ProductQuantity;
            return price;
        }

        public string GetOrderPriceString()
        {
            double price = this.ProductPrice * this.ProductQuantity;
            return price.PrintFormattedMKDPrice();
        }
    }
}
