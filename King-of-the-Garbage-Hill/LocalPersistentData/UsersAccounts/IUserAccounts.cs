﻿using System.Collections.Generic;
using Discord;

namespace King_of_the_Garbage_Hill.LocalPersistentData.UsersAccounts
{
    public interface IUserAccounts
    {
        List<AccountSettings> GetOrAddUserAccountsForGuild(ulong userId);
        AccountSettings GetAccount(IUser user);
        AccountSettings GetAccount(ulong userId);
        // AccountSettings GetBotAccount(ulong botId);
        AccountSettings GetOrCreateAccount(IUser user);
        void SaveAccounts(ulong userId);
        void SaveAccounts(IUser user);
        List<AccountSettings> GetAllAccount();
        AccountSettings CreateUserAccount(IUser user);

    }
}