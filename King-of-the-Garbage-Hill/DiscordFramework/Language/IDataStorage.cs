﻿using System.Collections.Generic;

namespace King_of_the_Garbage_Hill.DiscordFramework.Language
{
    public interface IDataStorage
    {
        void Store(object obj, string key);

        void Store(object obj, string group, string key);

        T Get<T>(string key);

        T Get<T>(string group, string key);

        IEnumerable<T> GetGroup<T>(string group);

        void Delete(string key);

        void Delete(string group, string key);

        IEnumerable<JustineLanguage> GetLanguages();
    }
}
