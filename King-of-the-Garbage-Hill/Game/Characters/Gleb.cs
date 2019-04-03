﻿using System;
using System.Threading.Tasks;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.GameGlobalVariables;


namespace King_of_the_Garbage_Hill.Game.Characters
{
    public class Gleb : IServiceSingleton
    {
        private readonly InGameGlobal _gameGlobal;

        public Gleb( InGameGlobal gameGlobal)
        {
            _gameGlobal = gameGlobal;
        }
        public Task InitializeAsync() => Task.CompletedTask;

        public void HandleGleb(GamePlayerBridgeClass player)
        {
           // throw new System.NotImplementedException();
        }

        public void HandleGlebAfter(GamePlayerBridgeClass player)
        {
            //skip check
           var skip = _gameGlobal.GlebSkipList.Find(x =>
                x.PlayerId == player.Status.PlayerId && x.GameId == player.DiscordAccount.GameId);
           if (skip != null && player.Status.WhoToAttackThisTurn != Guid.Empty)
           {
               player.Status.IsSkip = false;
               _gameGlobal.GlebSkipList.Remove(skip);
           }
        }

        public class GlebSkipClass
        {
            public Guid PlayerId;
            public ulong GameId;

            public GlebSkipClass(Guid playerId, ulong gameId)
            {
                PlayerId = playerId;
                GameId = gameId;

            }
        }
    }
}
