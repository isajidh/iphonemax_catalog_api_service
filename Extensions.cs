using Ecom.Catalog.Service.Dtos;
using Ecom.Catalog.Service.Entities;

namespace Ecom.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}