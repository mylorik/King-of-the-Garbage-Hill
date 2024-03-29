﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using King_of_the_Garbage_Hill.Game.GameLogic;
using King_of_the_Garbage_Hill.Game.MemoryStorage;

namespace King_of_the_Garbage_Hill.Game.Classes;

public class GameClass
{
    public GameClass(List<GamePlayerBridgeClass> playersList, ulong gameId, ulong creatorId, int turnLengthInSecond = 300, string gameMode = "Normal")
    {
        RoundNo = 1;
        Phrases = new CharactersUniquePhrase();
        PlayersList = playersList;
        GameId = gameId;
        TurnLengthInSecond = turnLengthInSecond;
        TimePassed = new Stopwatch();
        AllGameGlobalLogs = "";
        GlobalLogs =
            "<:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567>\n\n" +
            "Сервер игры<:e_:562879579694301184>- https://discord.gg/EpzznDgTe7\n" +
            "Правила <:e_:562879579694301184> <:e_:562879579694301184>- <#561293853076881409>\n" +
            "FAQ <:e_:562879579694301184> <:e_:562879579694301184> <:e_:562879579694301184> - <#561293980730523668>\n" +
            "Новости <:e_:562879579694301184><:e_:562879579694301184> - <#561293699309371393>\n" +
            "Разработчик<:e_:562879579694301184>- <@!181514288278536193>\n" +
            "ГеймДизайнер - <@!238337696316129280>\n" +
            "Арт Дизайнер - <@!207707809339539457>\n" +
            "GitHub <:e_:562879579694301184><:e_:562879579694301184><:e_:562879579694301184>- https://github.com/mylorik/King-of-the-Garbage-Hill" +
            "\n\n<:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567> <:e_:562879579694301184> <:sparta:561287745675329567>\n";
    
        IsCheckIfReady = true;
        SkipPlayersThisRound = 0;
        GameVersion = "Версия: 2.9.8 пожалуйста...";
        //GameVersion = "Версия: 2.9.9 helpпуда";
        GameMode = gameMode;
        CreatorId = creatorId;
        Teams = new List<TeamPlay>();
        ExploitPlayersList = new List<GamePlayerBridgeClass>();
        foreach (var player in PlayersList.Where(player => player.GameCharacter.Passive.All(x => x.PassiveName != "Exploit")))
        {
            ExploitPlayersList.Add(player);
        }
        RollExploit();
    }


    public int RoundNo { get; set; }
    public List<GamePlayerBridgeClass> PlayersList { get; set; }
    public ulong GameId { get; set; }
    public double TurnLengthInSecond { get; set; }
    public string GameVersion { get; set; }
    public Stopwatch TimePassed { get; set; }

    public CharactersUniquePhrase Phrases { get; set; }

    public bool IsCheckIfReady { get; set; }
    public bool IsFinished { get; set; } = false;
    public bool IsKratosEvent { get; set; } = false;

    /*
     * 1 - Turn
     * 2 - Counting
     * 3 - End
     */
    private string AllGameGlobalLogs { get; set; }

    public int SkipPlayersThisRound { get; set; }
    private string GlobalLogs { get; set; }
    public string GameMode { get; set; }
    public ulong CreatorId { get; set; }
    public List<TeamPlay> Teams { get; set; }
    public uint TestFightNumber { get; set; }

    public List<BotsBehavior.NanobotClass> NanobotsList { get; set; } = new();

    public bool IsAramPickPhase { get; set; }

    public List<GamePlayerBridgeClass> ExploitPlayersList { get; set; }
    public int LastExploit { get; set; } = -1;
    public int TotalExploit { get; set; } = 0;




    public void AddGlobalLogs(string str, string newLine = "\n")
    {
        GlobalLogs += str + newLine;
        AllGameGlobalLogs += str + newLine;
    }

    public string GetGlobalLogs()
    {
        return GlobalLogs;
    }

    public string GetAllGlobalLogs()
    {
        return AllGameGlobalLogs;
    }

    public void SetGlobalLogs(string str)
    {
        GlobalLogs = str;
    }

    public class TeamPlay
    {
        public TeamPlay(int teamId)
        {
            TeamId = teamId;
            TeamPlayers = new List<Guid>();
            TeamPlayersUsernames = new List<string>();
        }

        public int TeamId { get; set; }
        public List<Guid> TeamPlayers { get; set; }
        public List<string> TeamPlayersUsernames { get; set; }
    }

    internal List<Guid> GetTeammates(GamePlayerBridgeClass player)
    {
        return Teams.Find(x => x.TeamPlayers.Contains(player.GetPlayerId()))?.TeamPlayers.Where(y => y != player.GetPlayerId()).ToList();
    }

    public void RollExploit()
    {
        if (ExploitPlayersList.Count(x => x.Passives.IsExploitFixed) == ExploitPlayersList.Count)
        {
            return;
        }
        LastExploit++;
        if (LastExploit >= ExploitPlayersList.Count)
        {
            LastExploit = 0;
        }

        foreach (var player in ExploitPlayersList)
        {
            player.Passives.IsExploitable = false;
        }

        while (true)
        {
            if (LastExploit >= ExploitPlayersList.Count)
            {
                LastExploit = 0;
            }

            if (ExploitPlayersList[LastExploit].Passives.IsExploitFixed)
            {
                LastExploit++;
            }
            else
            {
                ExploitPlayersList[LastExploit].Passives.IsExploitable = true;
                break;
            }
        }
    }
}