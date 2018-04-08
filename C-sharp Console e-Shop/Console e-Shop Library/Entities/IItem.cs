using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_e_Shop_Library.Entities
{
    public interface IItem
    {
        string ProductName { get; set; }
        double ProductPrice { get; set; }
    }
}
