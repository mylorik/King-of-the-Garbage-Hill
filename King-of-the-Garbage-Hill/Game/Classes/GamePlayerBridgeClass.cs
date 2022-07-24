﻿using System;
using System.Collections.Generic;

namespace King_of_the_Garbage_Hill.Game.Classes;

public class GamePlayerBridgeClass
{
    public GamePlayerBridgeClass(CharacterClass character, InGameStatus status, ulong discordId, ulong gameId, string discordUsername, int playerType)
    {
        Character = character;
        Status = status;
        DiscordId = discordId;
        GameId = gameId;
        DiscordUsername = discordUsername;
        PlayerType = playerType;
        character.UpdateIntelligenceResist();
        character.UpdateStrengthResist();
        character.UpdateSpeedResist();
        character.UpdatePsycheResist();
    }
    public CharacterClass Character { get; set; }

    public InGameStatus Status { get; set; }

    public ulong DiscordId { get; set; }
    public ulong GameId { get; set; }

    public string DiscordUsername { get; set; }

/*
0 == Normal
1 == Casual
2 == Admin
404 == Bot
*/
    public int PlayerType { get; set; }
    public List<ulong> DeleteMessages { get; set; } = new();
    public List<PredictClass> Predict { get; set; } = new();


    public bool IsBot()
    {
        return PlayerType == 404 || Status.SocketMessageFromBot == null;
    }

    public void MinusPsycheLog(GameClass game)
    {
        game.AddGlobalLogs($"\n{DiscordUsername} психанул");
    }

    public Guid GetPlayerId()
    {
        return Status.PlayerId;
    }

    public bool isTeamMember(GameClass game, Guid player2)
    {
        var team = game.Teams.Find(x => x.TeamPlayers.Contains(GetPlayerId()));
        return team.TeamPlayers.Contains(player2);
    }
}