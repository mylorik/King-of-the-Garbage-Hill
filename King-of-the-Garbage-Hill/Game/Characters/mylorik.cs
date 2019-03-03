﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.DiscordMessages;
using King_of_the_Garbage_Hill.Helpers;

namespace King_of_the_Garbage_Hill.Game.Characters
{
    public class Mylorik : IServiceSingleton
    {
     
        private readonly SecureRandom _rand;

        private readonly GameUpdateMess _upd;
        private readonly InGameGlobal _gameGlobal;

        public Mylorik(GameUpdateMess upd, SecureRandom rand, InGameGlobal gameGlobal)
        {
            _upd = upd;
            _rand = rand;
            _gameGlobal = gameGlobal;
        }

        public Task InitializeAsync() => Task.CompletedTask;

        
        public void HandleMylorikRevenge(GameBridgeClass player1, ulong player2Id, ulong gameId)
        {
            var mylorik = _gameGlobal.MylorikRevenge.Find(x =>
                x.GameId == gameId && x.PlayerDiscordId == player1.DiscordAccount.DiscordId);


            if (mylorik == null)
            {
                _gameGlobal.MylorikRevenge.Add(new MylorikRevengeClass(player1.DiscordAccount.DiscordId, gameId,
                    player2Id));
            }
            else
            {
                if (mylorik.EnemyListDiscordId.All(x => x.EnemyDiscordId != player2Id))
                {
                    mylorik.EnemyListDiscordId.Add(new MylorikRevengeClassSub(player2Id));
                    return;
                }

                var find = mylorik.EnemyListDiscordId.Find(x =>
                    x.EnemyDiscordId == player2Id && x.IsUnique);
                if (find != null)
                {
                    player1.Status.Score++;
                    find.IsUnique = false;
                }
            }
        }
        public async Task HandleMylorik(GameBridgeClass player, GameClass game)
        {
            //Boole


            await Task.CompletedTask;
            //end Boole
        }


        public void HandleMylorikAfter(GameBridgeClass player)
        {
            //Revenge
            if (player.Status.IsWonLastTime != 0)
                HandleMylorikRevenge(player, player.Status.IsWonLastTime, player.DiscordAccount.GameId);
            //end Revenge

            //Spanish
            if (player.Status.IsWonLastTime == 0)
            {
                var rand = _rand.Random(1, 2);

                if (rand == 1) player.Character.Justice.JusticeForNextRound--;
            }

            //end Spanish
        }

        public class MylorikRevengeClass
        {
            public List<MylorikRevengeClassSub> EnemyListDiscordId;
            public ulong GameId;
            public ulong PlayerDiscordId;

            public MylorikRevengeClass(ulong playerDiscordId, ulong gameID, ulong firstLost)
            {
                PlayerDiscordId = playerDiscordId;
                EnemyListDiscordId = new List<MylorikRevengeClassSub> {new MylorikRevengeClassSub(firstLost)};
                GameId = gameID;
            }
        }

        public class MylorikRevengeClassSub
        {
            public ulong EnemyDiscordId;
            public bool IsUnique;

            public MylorikRevengeClassSub(ulong enemyDiscordId)
            {
                EnemyDiscordId = enemyDiscordId;
                IsUnique = true;
            }
        }

    }
}