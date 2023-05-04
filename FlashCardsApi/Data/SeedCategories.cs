using FlashCardsApi.Models;

namespace FlashCardsApi.Data;

    public class SeedCategories
    {
        public static void Initialize(FlashCardsContext context)
        {
           
            context.Database.EnsureCreated();

            if (context.Categories.Any())
            {
                return;   // DB has been seeded
            }

            var categories = new Category[]
            {
                new Category{ CategoryName = "Small"},
                new Category{ CategoryName = "Medium"},
                new Category{ CategoryName = "Large"},
            };

            foreach (Category ctgr in categories)
            {
                context.Categories.Add(ctgr);
            }
            context.SaveChanges();

            if (context.Languages.Any())
            {
                return;   // DB has been seeded
            }

            var languages = new Language[]
            {
                    new Language{ LanguageName = "Arabic", LanguageCode = "Ar" },
                    new Language{ LanguageName = "English", LanguageCode = "En" }
            };

            foreach (Language lang in languages)
            {
                context.Languages.Add(lang);
            }
            context.SaveChanges();

    }
}

