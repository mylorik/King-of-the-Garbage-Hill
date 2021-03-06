﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.GameGlobalVariables;

namespace King_of_the_Garbage_Hill.Game.Characters
{
    public class Sirinoks : IServiceSingleton
    {
        private readonly InGameGlobal _gameGlobal;

        public Sirinoks(InGameGlobal gameGlobal)
        {
            _gameGlobal = gameGlobal;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public void HandleSirinoks(GamePlayerBridgeClass player)
        {
            //   throw new System.NotImplementedException();
        }

        public void HandleSirinoksAfter(GamePlayerBridgeClass player, GameClass game)
        {
            //обучение
            if (player.Status.IsLostThisCalculation != Guid.Empty)
            {
                var playerSheLostLastTime =
                    game.PlayersList.Find(x => x.Status.PlayerId == player.Status.IsLostThisCalculation);
                var intel = new List<StatsClass>
                {
                    new StatsClass(1, playerSheLostLastTime.Character.GetIntelligence()),
                    new StatsClass(2, playerSheLostLastTime.Character.GetStrength()),
                    new StatsClass(3, playerSheLostLastTime.Character.GetSpeed()),
                    new StatsClass(4, playerSheLostLastTime.Character.GetPsyche())
                };
                var best = intel.OrderByDescending(x => x.Number).ToList()[0];

                var siri = _gameGlobal.SirinoksTraining.Find(x =>
                    x.GameId == game.GameId && x.PlayerId == player.Status.PlayerId);

                if (siri == null)
                    _gameGlobal.SirinoksTraining.Add(new TrainingClass(player.Status.PlayerId, game.GameId, best.Index,
                        best.Number));
                else
                    siri.Training.Add(new TrainingSubClass(best.Index, best.Number));
            }

            //обучение end
        }

        public class StatsClass
        {
            public int Index;
            public int Number;

            public StatsClass(int index, int number)
            {
                Index = index;
                Number = number;
            }
        }


        public class TrainingClass
        {
            public ulong GameId;
            public Guid PlayerId;
            public List<TrainingSubClass> Training = new List<TrainingSubClass>();

            public TrainingClass(Guid playerId, ulong gameId, int index, int number)
            {
                PlayerId = playerId;
                GameId = gameId;
                Training.Add(new TrainingSubClass(index, number));
            }
        }

        public class TrainingSubClass
        {
            public int StatIndex;
            public int StatNumber;


            public TrainingSubClass(int statIndex, int statNumber)
            {
                StatIndex = statIndex;
                StatNumber = statNumber;
            }
        }
    }
}