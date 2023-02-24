using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetailService.Context;
using RetailService.Models;
using RetailService.MQ;
using RetailService.ViewModels;

namespace RetailService.Services
{
    public class RetailService : IRetailService
    {
        private readonly RetailContext _context;
        private readonly IRabbitMQManager _rabbitMQManager;
        public RetailService(RetailContext context, IRabbitMQManager rabbitMQManager)
        {
            _context = context;
            _rabbitMQManager = rabbitMQManager;
        }
        public async Task<IEnumerable<ItemViewModel>> GetAsync()
        {
            return await _context.Items.Select(item => new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                BasePrice = item.BasePrice,
                Quantity = item.Quantity,
                TaxRate = item.TaxRate,

            }).ToListAsync();
        }
        public async Task<ItemViewModel> GetByIdAsync(int id)
        {
            var item = await GetItemAsync(id);
            PublishMessage(Command.Get, $"Get Item", $"Get an Existing Item");
            return ToViewModel(item);
        }
        public async Task<ItemViewModel> CreateAsync(ItemCreateViewModel item)
        {
            var itemEntity = new Item
            {
                Name = item.Name,
                Description = item.Description,
                BasePrice = item.BasePrice,
                Quantity = item.Quantity,
                TaxRate = item.TaxRate,
            };
            await _context.AddAsync(itemEntity);
            await _context.SaveChangesAsync();

            PublishMessage(Command.Post, $"New Item is Added", $"Item with id: {itemEntity.Id} is added at Rs.{itemEntity.BasePrice} for {itemEntity.Quantity} units excluding {itemEntity.TaxRate}% tax.");
            return ToViewModel(itemEntity);
        }
        public async Task<ItemViewModel> UpdateAsync(int id, ItemUpdateViewModel item)
        {
            var itemEntity = await GetItemAsync(id);
            itemEntity.Description = item.Description;
            itemEntity.BasePrice = item.BasePrice;
            itemEntity.Quantity = item.Quantity;
            itemEntity.TaxRate = item.TaxRate;
            await _context.SaveChangesAsync();

            PublishMessage(Command.Put, "Update Item", $"Item with id: {itemEntity.Id} is updated at Rs.{itemEntity.BasePrice} for {itemEntity.Quantity} units excluding {itemEntity.TaxRate}% tax.");
            return ToViewModel(itemEntity);
        }

        public async Task DeleteAsync(int id)
        {
            var item = await GetItemAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            PublishMessage(Command.Delete, "Delete Item", $"Item with id: {item.Id} is deleted at Rs.{item.BasePrice} for {item.Quantity} units excluding {item.TaxRate}% tax.");
        }
        private async Task<Item> GetItemAsync(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
            if (item is null)
            {
                throw new Exception($"Could not find the Item with id: {id}");
            }
            return item;
        }

        private ItemViewModel ToViewModel(Item item)
        {
            return new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                BasePrice = item.BasePrice,
                Quantity = item.Quantity,
                TaxRate = item.TaxRate,
            };
        }

        //  Send message to Message Service
        private void PublishMessage(Command command, string title, string body)
        {
            var messageModel = new Message
            {
                Id = Guid.NewGuid(),
                Command = command,
                Title = title,
                Body = body,
                CreatedOn = DateTime.UtcNow,
            };
            _rabbitMQManager.Publish(messageModel, "ms-exchange", "topic", "ms-b-routing");
        }
    }
}
