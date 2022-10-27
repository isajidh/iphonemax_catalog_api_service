using System;
using Ecom.Api;

namespace Ecom.Catalog.Service.Entities
{

    // Item entity that implements IEntity
    public class Item : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

    }
}