namespace FlashCardsApi.Models
{
    public class Product
    {
        public int ID { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime StartDate { get; set; }

        //public DateTime Duration { get; set; }

        public int Duration { get; set; }

        public int Price { get; set; }

        public string? CustomFieldText { get; set; }

        //int ProductCustomFieldId { get; set; }

        public int CategoryId { get; set; } 

        public Category? Category { get; set; }

        public ICollection<ProductCustomFieldKeyValue>? ProductCustomFieldKeysValues { get; set; }

        public ICollection<ProductTranslation>? ProductTranslations { get; set; }

    }
}
