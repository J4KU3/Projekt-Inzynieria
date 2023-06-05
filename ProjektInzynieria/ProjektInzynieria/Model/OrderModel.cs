using ProjektInzynieria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektInzynieria.Model
{
    public class OrderModel: Orders
    {
        public OrderModel(Orders Copyorders)
        {
            OrderID = Copyorders.OrderID;
            ClientID = Copyorders.ClientID;
            employeeID = Copyorders.employeeID;
            OrderDate = Copyorders.OrderDate;
            ItemName = Copyorders.ItemName;
            ItemColor = Copyorders.ItemColor;
            Project = Copyorders.Project;
            Price = Copyorders.Price;
            
        }
    }
}
