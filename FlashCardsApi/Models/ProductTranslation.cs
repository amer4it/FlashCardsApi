namespace FlashCardsApi.Models
{
    public class ProductTranslation
    {
        public int ID { get; set; }
        
        public int TranslationLanguageId { get; set; } 
        
        public string? ProductName { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public Language? Language { get; set; }    
   
    }
}
