﻿using MyStartup.Data.Entities;
using MyStartup.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyStartup.Helpers
{
    public interface IOrderHelper:IGenericHelper<Order>
    {
        Task<IQueryable<Order>> GetOrdersAsync(string userName);

        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName);

        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);

        Task DeleteDetailTempAsync(int id);

        Task DeleteOrderAsync(int id);

        Task<bool> ConfirmOrderAsync(string userName);

        Task DeliverOrder(DeliverViewModel model);

        Task<Order> GetOrdersAsync(int id);
    }
}
