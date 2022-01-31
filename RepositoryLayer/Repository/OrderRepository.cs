using ModelLayer;
using MongoDB.Driver;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<OrderModel> Order;

        public OrderRepository(IDBSetting db)
        {
            var client = new MongoClient(db.ConnectionString);
            var database = client.GetDatabase(db.DatabaseName);
            Order = database.GetCollection<OrderModel>("Order");
        }
        public async Task<OrderModel> AddOrder(OrderModel order)
        {
            try
            {
                var check = await this.Order.Find(x => x.orderID == order.orderID).SingleOrDefaultAsync();
                if (check == null)
                {
                    await this.Order.InsertOneAsync(order);
                    return order;

                }
                return null;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CancleOrder(OrderModel del)
        {
            try
            {
                await this.Order.FindOneAndDeleteAsync(x => x.orderID == del.orderID);
                return true;

            }
            catch (ArgumentNullException e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<OrderModel> GetOrder()
        {
            //Order.Aggregate({ "$project": { "totalPay": { "$multiply": ["$discountPrice", "$quantity"]  } } } );
            return Order.Find(FilterDefinition<OrderModel>.Empty).ToList();
        }
    }
}
