﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.MemoryStorage;
using King_of_the_Garbage_Hill.LocalPersistentData.UsersAccounts;

namespace King_of_the_Garbage_Hill.Helpers
{
    public sealed class HelperFunctions : IServiceSingleton
    {
        private readonly UserAccounts _accounts;

        private readonly List<string> _characterNames = new List<string>
        {
            "UselessCrab",
            "Daumond",
            "MegaVova99",
            "YasuoOnly",
            "PETYX",
            "Drone",
            "Boole",
            "Shark",
            "EloBoost",
            "Ratata",
            "R.I.D.",
            "2kEloPro",
            "Razer",
            "SpartanHero",
            "AllMight",
            "Nezuko",
            "Naruto1999"
        };

        private readonly CharactersPull _charactersPull;
        private readonly Global _global;
        private readonly SecureRandom _secureRandom;
       

        public HelperFunctions(CharactersPull charactersPull, Global global, UserAccounts accounts,
            SecureRandom secureRandom)
        {
            _charactersPull = charactersPull;
            _global = global;
            _accounts = accounts;
            _secureRandom = secureRandom;
        }


        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DeleteBotAndUserMessage(IUserMessage botMessage, SocketMessage userMessage,
            int timeInSeconds)
        {
            var seconds = timeInSeconds * 1000;
            await Task.Delay(seconds);
            await botMessage.DeleteAsync();
            await userMessage.DeleteAsync();
        }

        public async Task DeleteMessOverTime(IUserMessage message, int timeInSeconds)
        {
            var seconds = timeInSeconds * 1000;
            await Task.Delay(seconds);
            await message.DeleteAsync();
        }

        public void SubstituteUserWithBot(ulong discordId)
        {
            
            var prevGame = _global.GamesList.Find(
                x => x.PlayersList.Any(m => m.DiscordAccount.DiscordId == discordId));

            if (prevGame != null)
            {
                var freeBot = GetFreeBot(prevGame.PlayersList, prevGame.GameId);
                freeBot.GameId = prevGame.GameId;
             
                var leftUser = prevGame.PlayersList.Find(x => x.DiscordAccount.DiscordId == discordId);

                leftUser.DiscordAccount = freeBot;
                leftUser.Status.SocketMessageFromBot = null;

                _accounts.GetAccount(discordId).GameId = 1000000000000000000;
            }
            
        }

        public DiscordAccountClass GetFreeBot(List<GamePlayerBridgeClass> playerList, ulong newGameId)
        {
            DiscordAccountClass account;
            string name;

            do
            {
                var index = _secureRandom.Random(0, _characterNames.Count - 1);
                name = _characterNames[index];
            } while (playerList.Any(x => x.DiscordAccount.DiscordUserName == name));

            ulong botId = 1;
            do
            {
                account = _accounts.GetAccount(botId);
                botId++;
            } while (account.IsPlaying);

            account.DiscordUserName = name;

            return account;
        }
    }
}