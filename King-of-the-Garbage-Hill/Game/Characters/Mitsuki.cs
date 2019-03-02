﻿using System.Threading.Tasks;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.DiscordMessages;
using King_of_the_Garbage_Hill.Helpers;

namespace King_of_the_Garbage_Hill.Game.Characters
{
    public class Mitsuki : IServiceSingleton
    {
 
        private readonly SecureRandom _rand;

        private readonly GameUpdateMess _upd;

        public Mitsuki(GameUpdateMess upd, SecureRandom rand)
        {
            _upd = upd;
            _rand = rand;
 
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public void HandleMitsuki(GameBridgeClass player)
        {
            throw new System.NotImplementedException();
        }

        public void HandleMitsukiAfter(GameBridgeClass player)
        {
            throw new System.NotImplementedException();
        }
    }
}
