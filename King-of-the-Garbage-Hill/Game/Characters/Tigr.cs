﻿using System.Collections.Generic;
using System.Threading.Tasks;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.GameGlobalVariables;
using King_of_the_Garbage_Hill.Game.MemoryStorage;

namespace King_of_the_Garbage_Hill.Game.Characters
{
    public class Tigr : IServiceSingleton
    {
        private readonly InGameGlobal _gameGlobal;
        private readonly Global _global;
        private readonly CharactersUniquePhrase _phrase;

        public Tigr(InGameGlobal gameGlobal, Global global,
            CharactersUniquePhrase phrase)
        {
            _gameGlobal = gameGlobal;
            _global = global;
            _phrase = phrase;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public void HandleTigr(GameBridgeClass player)
        {
            //   throw new System.NotImplementedException();
        }

        public async Task HandleTigrAfter(GameBridgeClass player, GameClass game)
        {
            //3-0 обоссан: 
            if (player.Status.IsWonLastTime != 0)
            {
                var tigr = _gameGlobal.TigrThreeZeroList.Find(x =>
                    x.GameId == game.GameId && x.PlayerDiscordId == player.DiscordAccount.DiscordId);


                if (tigr == null)
                {
                    _gameGlobal.TigrThreeZeroList.Add(new ThreeZeroClass(player.DiscordAccount.DiscordId, game.GameId,
                        player.Status.IsWonLastTime));
                }
                else
                {
                    var enemy = tigr.FriendList.Find(x => x.EnemyId == player.Status.IsWonLastTime);
                    if (enemy != null)
                    {
                        enemy.WinsSeries++;

                        if (enemy.WinsSeries >= 3 && enemy.IsUnique)
                        {
                            player.Status.AddRegularPoints(2);
                            await _phrase.TigrThreeZero.SendLog(player);

                            var enemyAcc = game.PlayersList.Find(x =>
                                x.DiscordAccount.DiscordId == player.Status.IsWonLastTime);

                            if (enemyAcc != null)
                            {
                                enemyAcc.Character.Intelligence--;
                                enemyAcc.Character.Psyche--;
                                enemy.IsUnique = false;
                            }
                            else
                            {
                                await _global.Client.GetUser(181514288278536193).GetOrCreateDMChannelAsync().Result
                                    .SendMessageAsync("TIGR - enemy == null");
                            }
                        }
                    }
                    else
                    {
                        await _global.Client.GetUser(181514288278536193).GetOrCreateDMChannelAsync().Result
                            .SendMessageAsync("TIGR - enemy == null");
                    }
                }
            }
            else
            {
                var tigr = _gameGlobal.TigrThreeZeroList.Find(x =>
                    x.GameId == game.GameId && x.PlayerDiscordId == player.DiscordAccount.DiscordId);

                var enemy = tigr?.FriendList.Find(x => x.EnemyId == player.Status.IsLostLastTime);

                if (enemy != null && enemy.IsUnique) enemy.WinsSeries = 0;
            }
            //end 3-0 обоссан: 
        }

        public class ThreeZeroClass
        {
            public ulong GameId;
            public ulong PlayerDiscordId;
            public List<ThreeZeroSubClass> FriendList = new List<ThreeZeroSubClass>();

            public ThreeZeroClass(ulong playerDiscordId, ulong gameId, ulong enemyId)
            {
                PlayerDiscordId = playerDiscordId;
                GameId = gameId;
                FriendList.Add(new ThreeZeroSubClass(enemyId));
            }
        }

        public class ThreeZeroSubClass
        {
            public ulong EnemyId;
            public int WinsSeries;
            public bool IsUnique;

            public ThreeZeroSubClass(ulong enemyId)
            {
                EnemyId = enemyId;
                WinsSeries = 1;
                IsUnique = true;
            }
        }
    }
}