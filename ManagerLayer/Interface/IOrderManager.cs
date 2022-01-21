using ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interface
{
    public interface IOrderManager
    {
        Task<OrderModel> AddOrder(OrderModel wish);
        Task<bool> CancleOrder(OrderModel del);
        IEnumerable<OrderModel> GetOrder();
    }
}
