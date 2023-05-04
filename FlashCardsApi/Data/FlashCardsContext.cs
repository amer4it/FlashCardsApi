using FlashCardsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace FlashCardsApi.Data
{
    public class FlashCardsContext:DbContext
    {
            
            public FlashCardsContext(DbContextOptions<FlashCardsContext> options) : base(options)
            {
            }

            public DbSet<Category> Categories { get; set; }
            public DbSet<Language> Languages { get; set; }
            public DbSet<Product> Products { get; set; } 
            public DbSet<ProductCustomFieldKeyValue> ProductCustomFieldKeyValues { get; set; } 
            public DbSet<ProductTranslation> ProductTranslations { get; set; } 
        
    }
}
