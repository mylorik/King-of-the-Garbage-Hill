﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.GameGlobalVariables;

namespace King_of_the_Garbage_Hill.Game.Characters
{
    public class Awdka : IServiceSingleton
    {
        private readonly InGameGlobal _gameGlobal;

        public Awdka(InGameGlobal gameGlobal)
        {
            _gameGlobal = gameGlobal;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }



        public void HandleAwdkaAfter(GamePlayerBridgeClass player, GameClass game)
        {
            //Произошел троллинг:
            if (player.Status.IsWonThisCalculation != Guid.Empty && player.Status.WhoToAttackThisTurn == player.Status.IsWonThisCalculation)
            {
                var awdka = _gameGlobal.AwdkaTrollingList.Find(x =>
                    x.GameId == player.GameId &&
                    x.PlayerId == player.Status.PlayerId);

                var enemy = awdka.EnemyList.Find(x => x.EnemyId == player.Status.IsWonThisCalculation);

                if (enemy == null)
                    awdka.EnemyList.Add(new TrollingSubClass(player.Status.IsWonThisCalculation,
                        game.PlayersList.Find(x => x.Status.PlayerId == player.Status.IsWonThisCalculation).Status
                            .GetScore()));
                else
                    enemy.Score = game.PlayersList.Find(x => x.Status.PlayerId == player.Status.IsWonThisCalculation)
                        .Status.GetScore();
            }
            //end Произошел троллинг:

            //Я пытаюсь!
            if (player.Status.IsLostThisCalculation != Guid.Empty)
            {
                var awdka = _gameGlobal.AwdkaTryingList.Find(x => x.GameId == player.GameId && x.PlayerId == player.Status.PlayerId);


                var enemy = awdka.TryingList.Find(x => x.EnemyPlayerId == player.Status.IsLostThisCalculation);
                if (enemy == null)
                    awdka.TryingList.Add(new TryingSubClass(player.Status.IsLostThisCalculation));
                else
                    enemy.Times++;
            }
            //Я пытаюсь!
        }

        public class TrollingClass
        {
            public List<TrollingSubClass> EnemyList = new();

            public ulong GameId;
            public Guid PlayerId;

            public TrollingClass(Guid playerId, ulong gameId)
            {
                PlayerId = playerId;
                GameId = gameId;
            }
        }

        public class TrollingSubClass
        {
            public Guid EnemyId;
            public int Score;

            public TrollingSubClass(Guid enemyId, int score)
            {
                EnemyId = enemyId;
                Score = score;
            }
        }

        public class TeachToPlayHistory
        {
            public ulong GameId;
            public Guid PlayerId;
            public List<TeachToPlayHistoryListClass> History = new();

            public TeachToPlayHistory(Guid playerId, ulong gameId)
            {
                GameId = gameId;
                PlayerId = playerId;
            }
        }

        public class TeachToPlayHistoryListClass
        {
            public Guid EnemyPlayerId;
            public string Text;
            public int Stat;

            public TeachToPlayHistoryListClass(Guid enemyPlayerId, string text, int stat)
            {
                EnemyPlayerId = enemyPlayerId;
                Text = text;
                Stat = stat;
            }
        }

        public class TryingClass
        {
            public ulong GameId;
            public Guid PlayerId;
            public List<TryingSubClass> TryingList = new();

            public TryingClass(Guid playerId, ulong gameId)
            {
                PlayerId = playerId;
                GameId = gameId;
            }
        }

        public class TryingSubClass
        {
            public Guid EnemyPlayerId;
            public bool IsUnique;
            public int Times;

            public TryingSubClass(Guid enemyPlayerId)
            {
                EnemyPlayerId = enemyPlayerId;
                Times = 1;
                IsUnique = false;
            }
        }
    }
}