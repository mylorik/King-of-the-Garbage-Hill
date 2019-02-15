﻿using System.Collections.Generic;

namespace King_of_the_Garbage_Hill.DiscordFramework.Language
{
    public class JustineLanguage
    {
        public uint LanguageId { get; set; }
        public string LanguageName { get; set; }
        public Dictionary<string, string> Resources { get; set; }
        public Dictionary<string, List<string>> ResourcePools { get; set; }
    }
}
