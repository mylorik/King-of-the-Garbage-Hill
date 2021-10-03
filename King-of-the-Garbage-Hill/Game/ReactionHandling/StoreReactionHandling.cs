﻿using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using King_of_the_Garbage_Hill.Game.Store;
using King_of_the_Garbage_Hill.LocalPersistentData.UsersAccounts;

namespace King_of_the_Garbage_Hill.Game.ReactionHandling
{
    public class StoreReactionHandling : IServiceSingleton
    {
        private readonly StoreLogic _storeLogic;
        private readonly UserAccounts _userAccounts;

        public StoreReactionHandling(UserAccounts userAccounts, StoreLogic storeLogic)
        {
            _userAccounts = userAccounts;
            _storeLogic = storeLogic;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task ReactionAddedStore(SocketMessageComponent button)
        {
            try
            {
                var title_str = button.Message.Embeds.FirstOrDefault()?.Title;

                if (title_str == null)
                    //await channel.SendMessageAsync("ERROR: Embed Title == null");
                    return;

                var title = title_str.Split(" - ");

                if (title.Length < 2)
                    //await channel.SendMessageAsync("ERROR: Embed Title len < 2");
                    return;

                if (title[0] != "Магазин") return;

                var account = _userAccounts.GetAccount(button.User.Id);
                var character = account.CharacterChance.Find(x => x.CharacterName == title[1]);

                if (character == null)
                {
                    await button.Channel.SendMessageAsync($"ERROR: character named {title[1]} was not found");
                    return;
                }

                var cost = 0;

                switch (button.Data.CustomId)
                {
                    //Уменьшить шанс на 1% - 20 ZP
                    case "attack-one":
                        cost = 20;
                        if (character.Multiplier <= 0.0)
                        {
                            await button.Channel.SendMessageAsync(
                                $"У персонажа {character.CharacterName} и так минимальный бонусный шанс - {character.Multiplier}");
                            return;
                        }

                        if (account.ZbsPoints < cost)
                        {
                            await button.Channel.SendMessageAsync($"У тебя недостаточно ZBS Points, нужно {cost}.");
                            return;
                        }

                        character.Multiplier -= 0.01;
                        character.Changes++;
                        account.ZbsPoints -= cost;

                        await button.Channel.SendMessageAsync(
                            $"Готово. Бонусный шанш {character.CharacterName} = {character.Multiplier}");

                        await button.Message.ModifyAsync(message =>
                        {
                            message.Content = "";
                            message.Embed = null;
                            message.Embed = _storeLogic.GetStoreEmbed(character, account, button.User).Build();
                        });
                        break;

                    //Увеличить шанс на 1% - 20 ZP
                    case "attack-two":
                        cost = 20;
                        if (character.Multiplier >= 2.0)
                        {
                            await button.Channel.SendMessageAsync(
                                $"У персонажа {character.CharacterName} и так максимальный бонусный шанс - {character.Multiplier}");
                            return;
                        }

                        if (account.ZbsPoints < cost)
                        {
                            await button.Channel.SendMessageAsync($"У тебя недостаточно ZBS Points, нужно {cost}.");
                            return;
                        }

                        character.Multiplier += 0.01;
                        character.Changes++;
                        account.ZbsPoints -= cost;

                        await button.Channel.SendMessageAsync(
                            $"Готово. Бонусный шанш {character.CharacterName} = {character.Multiplier}");

                        await button.Message.ModifyAsync(message =>
                        {
                            message.Content = "";
                            message.Embed = null;
                            message.Embed = _storeLogic.GetStoreEmbed(character, account, button.User).Build();
                        });
                        break;

                    //Вернуть все ZBS Points за этого персонажа - 10 ZP
                    case "attack-three":
                        cost = 0;
                        if (account.ZbsPoints < cost)
                        {
                            await button.Channel.SendMessageAsync($"У тебя недостаточно ZBS Points, нужно {cost}.");
                            return;
                        }

                        character.Multiplier = 1.0;
                        var zbsPointsToReturn = character.Changes * 20;
                        account.ZbsPoints += zbsPointsToReturn;
                        account.ZbsPoints -= cost;
                        character.Changes = 0;

                        await button.Channel.SendMessageAsync(
                            $"Готово. Бонусный шанш {character.CharacterName} = {character.Multiplier}\n" +
                            $"Ты вернул {zbsPointsToReturn} ZBS Points");

                        await button.Message.ModifyAsync(message =>
                        {
                            message.Content = "";
                            message.Embed = null;
                            message.Embed = _storeLogic.GetStoreEmbed(character, account, button.User).Build();
                        });
                        break;

                    //Вернуть все ZBS Points за ВСЕХ персонажей - 50 ZP
                    case "attack-four":
                        cost = 0;
                        if (account.ZbsPoints < cost)
                        {
                            await button.Channel.SendMessageAsync($"У тебя недостаточно ZBS Points, нужно {cost}.");
                            return;
                        }

                        zbsPointsToReturn = 0;
                        account.ZbsPoints -= cost;

                        foreach (var c in account.CharacterChance)
                        {
                            c.Multiplier = 1.0;
                            zbsPointsToReturn += c.Changes * 20;
                            c.Changes = 0;
                        }

                        account.ZbsPoints += zbsPointsToReturn;
                        await button.Channel.SendMessageAsync(
                            $"Готово. Бонусный шанш **Всех Персонажей** = {character.Multiplier}\n" +
                            $"Ты вернул {zbsPointsToReturn} ZBS Points");

                        await button.Message.ModifyAsync(message =>
                        {
                            message.Content = "";
                            message.Embed = null;
                            message.Embed = _storeLogic.GetStoreEmbed(character, account, button.User).Build();
                        });
                        break;
                }
            }
            catch
            {
                //ingored
            }
        }
    }
}