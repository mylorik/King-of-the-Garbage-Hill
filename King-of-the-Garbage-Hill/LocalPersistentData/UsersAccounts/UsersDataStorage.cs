﻿using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using King_of_the_Garbage_Hill.DiscordFramework;
using King_of_the_Garbage_Hill.Game.Classes;
using Newtonsoft.Json;

namespace King_of_the_Garbage_Hill.LocalPersistentData.UsersAccounts;

public sealed class UserAccountsDataStorage : IServiceSingleton
{
    //Save all DiscordAccountClass

    private readonly LoginFromConsole _logs;

    public UserAccountsDataStorage(LoginFromConsole log)
    {
        _logs = log;
    }

    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }


    public void SaveAccountSettings(DiscordAccountClass accounts, string idString, string json)
    {
        var filePath = $@"DataBase/UserAccounts/discordAccount-{idString}.json";
        try
        {
            File.WriteAllText(filePath, json);
        }
        catch (Exception exception)
        {
            _logs.Critical(exception.Message);
            _logs.Critical(exception.StackTrace);
        }
    }


    public void SaveAccountSettings(DiscordAccountClass accounts, ulong userId)
    {
        var filePath = $@"DataBase/UserAccounts/discordAccount-{userId}.json";
        try
        {
            var json = JsonConvert.SerializeObject(accounts, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
        catch (Exception exception)
        {
            _logs.Critical(exception.Message);
            _logs.Critical(exception.StackTrace);
        }
    }


    public ConcurrentDictionary<ulong, DiscordAccountClass> LoadAllAccounts()
    {
        var dick = new ConcurrentDictionary<ulong, DiscordAccountClass>();
        var filePaths = Directory.GetFiles(@"DataBase/UserAccounts");

        foreach (var file in filePaths)
        {
            var id = Convert.ToUInt64(file.Split("-")[1].Split(".")[0]);
            if (id == 0) continue;

            var json = File.ReadAllText(file);


            try
            {
                var acc = JsonConvert.DeserializeObject<DiscordAccountClass>(json);
                dick.GetOrAdd(id, acc);
            }
            catch (Exception exception)
            {
                _logs.Critical(exception.Message);
                _logs.Critical(exception.StackTrace);

                var newList = new DiscordAccountClass();
                SaveAccountSettings(newList, $"{id}-BACK_UP", json);
                dick.GetOrAdd(id, _ => newList);
            }
        }

        return dick;
    }
}