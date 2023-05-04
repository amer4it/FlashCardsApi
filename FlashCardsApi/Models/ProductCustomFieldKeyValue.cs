namespace FlashCardsApi.Models
{
    public class ProductCustomFieldKeyValue
    {
        public int ID { get; set; }

        public string? Key { get; set; }

        public string? Value { get; set; }
        
        public int ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
