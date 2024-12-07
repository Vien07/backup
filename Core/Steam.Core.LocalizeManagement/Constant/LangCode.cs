
using Steam.Core.Base.Constant;


namespace Steam.Core.LocalizeManagement.Constants
{
    public static class LangItemConstants
    {
        public class LangItem
        {
            public string LanguageCode { get; set; }
            public string ShortCode { get; set; }
            public string Name { get; set; }
        }
        public static List<LangItem> DefaultList = new List<LangItem>
        {
            new LangItem{ LanguageCode = "vi-VN", ShortCode = "vi", Name = "Vietnamese (Vietnam)" },
            new LangItem{ LanguageCode = "af-ZA", ShortCode = "af", Name = "Afrikaans (South Africa)" },
            new LangItem{ LanguageCode = "ar-SA", ShortCode = "ar", Name = "Arabic (Saudi Arabia)" },
            //new LangItem{ LanguageCode = "ar-EG", ShortCode = "ar", Name = "Arabic (Egypt)" },
            new LangItem{ LanguageCode = "az-AZ", ShortCode = "az", Name = "Azerbaijani (Azerbaijan)" },
            new LangItem{ LanguageCode = "bn-IN", ShortCode = "bn", Name = "Bengali (India)" },
            //new LangItem{ LanguageCode = "bn-BD", ShortCode = "bn", Name = "Bengali (Bangladesh)" },
            new LangItem{ LanguageCode = "cs-CZ", ShortCode = "cs", Name = "Czech (Czech Republic)" },
            new LangItem{ LanguageCode = "da-DK", ShortCode = "da", Name = "Danish (Denmark)" },
            new LangItem{ LanguageCode = "de-DE", ShortCode = "de", Name = "German (Germany)" },
            //new LangItem{ LanguageCode = "de-AT", ShortCode = "de", Name = "German (Austria)" },
            new LangItem{ LanguageCode = "el-GR", ShortCode = "el", Name = "Greek (Greece)" },
            new LangItem{ LanguageCode = "en-US", ShortCode = "en", Name = "English (United States)" },
            //new LangItem{ LanguageCode = "en-GB", ShortCode = "en", Name = "English (United Kingdom)" },
            //new LangItem{ LanguageCode = "en-CA", ShortCode = "en", Name = "English (Canada)" },
            //new LangItem{ LanguageCode = "en-AU", ShortCode = "en", Name = "English (Australia)" },
            new LangItem{ LanguageCode = "es-ES", ShortCode = "es", Name = "Spanish (Spain)" },
            //new LangItem{ LanguageCode = "es-MX", ShortCode = "es", Name = "Spanish (Mexico)" },
            //new LangItem{ LanguageCode = "es-AR", ShortCode = "es", Name = "Spanish (Argentina)" },
            new LangItem{ LanguageCode = "fr-FR", ShortCode = "fr", Name = "French (France)" },
            //new LangItem{ LanguageCode = "fr-CA", ShortCode = "fr", Name = "French (Canada)" },
            new LangItem{ LanguageCode = "he-IL", ShortCode = "he", Name = "Hebrew (Israel)" },
            new LangItem{ LanguageCode = "hi-IN", ShortCode = "hi", Name = "Hindi (India)" },
            new LangItem{ LanguageCode = "hu-HU", ShortCode = "hu", Name = "Hungarian (Hungary)" },
            new LangItem{ LanguageCode = "id-ID", ShortCode = "id", Name = "Indonesian (Indonesia)" },
            new LangItem{ LanguageCode = "it-IT", ShortCode = "it", Name = "Italian (Italy)" },
            new LangItem{ LanguageCode = "ja-JP", ShortCode = "ja", Name = "Japanese (Japan)" },
            new LangItem{ LanguageCode = "ko-KR", ShortCode = "ko", Name = "Korean (South Korea)" },
            new LangItem{ LanguageCode = "ms-MY", ShortCode = "ms", Name = "Malay (Malaysia)" },
            new LangItem{ LanguageCode = "nl-NL", ShortCode = "nl", Name = "Dutch (Netherlands)" },
            new LangItem{ LanguageCode = "no-NO", ShortCode = "no", Name = "Norwegian (Norway)" },
            new LangItem{ LanguageCode = "pl-PL", ShortCode = "pl", Name = "Polish (Poland)" },
            //new LangItem{ LanguageCode = "pt-BR", ShortCode = "pt", Name = "Portuguese (Brazil)" },
            new LangItem{ LanguageCode = "pt-PT", ShortCode = "pt", Name = "Portuguese (Portugal)" },
            new LangItem{ LanguageCode = "ru-RU", ShortCode = "ru", Name = "Russian (Russia)" },
            new LangItem{ LanguageCode = "sv-SE", ShortCode = "sv", Name = "Swedish (Sweden)" },
            new LangItem{ LanguageCode = "th-TH", ShortCode = "th", Name = "Thai (Thailand)" },
            new LangItem{ LanguageCode = "tr-TR", ShortCode = "tr", Name = "Turkish (Turkey)" },
            new LangItem{ LanguageCode = "uk-UA", ShortCode = "uk", Name = "Ukrainian (Ukraine)" },
            new LangItem{ LanguageCode = "zh-CN", ShortCode = "zh", Name = "Chinese (Simplified, China)" },
            //new LangItem{ LanguageCode = "zh-TW", ShortCode = "zh", Name = "Chinese (Traditional, Taiwan)" },
        };

    }
}
