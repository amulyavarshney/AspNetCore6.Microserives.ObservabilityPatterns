using RetailService.Models;
using RetailService.ViewModels;

namespace RetailService.Services
{
    public interface IRetailService
    {
        Task<IEnumerable<ItemViewModel>> GetAsync();
        Task<ItemViewModel> GetByIdAsync(int id);
        Task<ItemViewModel> CreateAsync(ItemCreateViewModel item);
        Task<ItemViewModel> UpdateAsync(int id, ItemUpdateViewModel item);
        Task DeleteAsync(int id);
    }
}
