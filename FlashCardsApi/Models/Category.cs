using Microsoft.EntityFrameworkCore;

namespace FlashCardsApi.Models
{
    public class Category
    {
        public int ID { get; set; }

        public string? CategoryName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
