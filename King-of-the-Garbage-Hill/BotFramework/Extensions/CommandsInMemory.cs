﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

namespace King_of_the_Garbage_Hill.BotFramework.Extensions
{
    public class CommandsInMemory : IServiceSingleton
    {
        public uint MaximumCommandsInRam = 1000;


        //not data

        public List<CommandRam> CommandList { get; set; } = new List<CommandRam>();

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public class CommandRam
        {
            public IUserMessage BotSocketMsg;

            public ulong MessageUserId;


            public CommandRam(IUserMessage userSocketMsg, IUserMessage botSocketMsg)
            {
                MessageUserId = userSocketMsg.Id;
                BotSocketMsg = botSocketMsg;
            }
        }
    }
}