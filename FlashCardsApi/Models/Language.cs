﻿namespace FlashCardsApi.Models
{
    public class Language
    {
        public int ID { get; set; }

        public string? LanguageName { get; set; }

        public string? LanguageCode { get; set; }

        public ICollection<ProductTranslation>? ProductTranslations { get; set; }

    }
}
