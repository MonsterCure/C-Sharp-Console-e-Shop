using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_e_Shop_Library.Entities
{
    public class Vendor
    {
        public int Id { get; private set; }
        private static int _count = 1;
        public string VendorName { get; set; }
        public List<Vendor> vendors { get; set; } = new List<Vendor>(); //C# 6.0 feature: auto-property initializer
        private List<Product> _vendorProducts = new List<Product>();

        public Vendor(string vendorName)
        {
            this.Id = _count++;
            this.VendorName = vendorName;
            vendors.Add(this);
        }

        public void SetVendorsProducts(List<Product> products) => _vendorProducts = products;  //C# 6.0 feature: expression bodied method
    }
}
