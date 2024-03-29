﻿using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace King_of_the_Garbage_Hill.DiscordFramework;

internal class SendMessagesUsingConsole
{
    internal static void ConsoleInput(DiscordShardedClient client)
    {
        var input = string.Empty;


        while (input != null && input.Trim().ToLower() != "block")
        {
            input = Console.ReadLine();

            if (input != null && input.Trim().ToLower() == "mess")
                ConsoleSendMessage(client);
        }
    }

    private static async void ConsoleSendMessage(BaseSocketClient client)
    {
        try
        {
            Console.WriteLine("Guilds: ");
            var guild = GetSelectedGuild(client.Guilds);
            var textChannel = GetSelectedTextChannel(guild.TextChannels);
            var msg = string.Empty;


            while (msg != null && msg.Trim() == string.Empty)
            {
                Console.WriteLine("Сообщение: ");
                msg = Console.ReadLine();
            }


            if (msg == null) return;
            var prefixCheck = msg.ToCharArray();
            if (prefixCheck[0] == '*')
            {
                var mesToDel = await textChannel.SendMessageAsync(msg);
                await mesToDel.DeleteAsync();


                Console.WriteLine("Команда выполненая каппатан бу!");
            }
            else
            {
                await textChannel.SendMessageAsync(msg);
                Console.WriteLine("Отправлено!");
            }
        }
        catch
        {
            Console.WriteLine("Осьминожки не могут сюда писать()");
        }
    }

    private static SocketTextChannel GetSelectedTextChannel(IEnumerable<SocketTextChannel> channels)
    {
        var textChannels = channels.ToList();
        var index = textChannels.Count - 1;
        for (var i = 0; i <= index; i++) Console.WriteLine($"{i} - {textChannels[i].Name}");
        var selectedIndex = -1;
        while (selectedIndex < 0 || selectedIndex > index)
        {
            var success = int.TryParse(Console.ReadLine()?.Trim(), out selectedIndex);
            if (!success || selectedIndex < 0 || selectedIndex > index)
            {
                Console.WriteLine("Incorrect index.");
                selectedIndex = -1;
            }
        }

        return textChannels[selectedIndex];
    }

    private static SocketGuild GetSelectedGuild(IEnumerable<SocketGuild> guilds)
    {
        var socketGuilds = guilds.ToList();
        var index = socketGuilds.Count - 1;
        for (var i = 0; i <= index; i++) Console.WriteLine($"{i} - {socketGuilds[i].Name}");
        var selectedIndex = -1;
        while (selectedIndex < 0 || selectedIndex > index)
        {
            var success = int.TryParse(Console.ReadLine()?.Trim(), out selectedIndex);
            if (success && selectedIndex >= 0 && selectedIndex <= index) continue;
            Console.WriteLine("Incorrect index.");
            selectedIndex = -1;
        }

        return socketGuilds[selectedIndex];
    }
}