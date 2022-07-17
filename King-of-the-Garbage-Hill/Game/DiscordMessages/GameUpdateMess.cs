﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using King_of_the_Garbage_Hill.DiscordFramework;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.GameGlobalVariables;
using King_of_the_Garbage_Hill.Game.MemoryStorage;
using King_of_the_Garbage_Hill.Helpers;
using King_of_the_Garbage_Hill.LocalPersistentData.UsersAccounts;

namespace King_of_the_Garbage_Hill.Game.DiscordMessages;

public sealed class GameUpdateMess : ModuleBase<SocketCommandContext>, IServiceSingleton
{
    private readonly UserAccounts _accounts;

    private readonly InGameGlobal _gameGlobal;
    private readonly Global _global;
    private readonly HelperFunctions _helperFunctions;
    private readonly Logs _log;
    private readonly SecureRandom _random;
    private readonly CharactersPull _charactersPull;
    private readonly List<string> _vampyrGarlic = new() { "Никаких статов для тебя, поешь чеснока", "Иди отсюда, Вампур позорный", "А ну хватит кусаться!", "Клыки наточил?" };

    private readonly List<Emoji> _playerChoiceAttackList = new()
        { new Emoji("1⃣"), new Emoji("2⃣"), new Emoji("3⃣"), new Emoji("4⃣"), new Emoji("5⃣"), new Emoji("6⃣") };


    public GameUpdateMess(UserAccounts accounts, Global global, InGameGlobal gameGlobal,
        HelperFunctions helperFunctions, Logs log, SecureRandom random, CharactersPull charactersPull)
    {
        _accounts = accounts;
        _global = global;
        _gameGlobal = gameGlobal;
        _helperFunctions = helperFunctions;
        _log = log;
        _random = random;
        _charactersPull = charactersPull;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }


    public async Task ShowRulesAndChar(SocketUser user, GamePlayerBridgeClass player)
    {
        var allCharacters = _charactersPull.GetAllCharacters();
        var character = allCharacters.Find(x => x.Name == player.Character.Name);
        var pass = "";
        var characterPassivesList = character.Passive;
        foreach (var passive in characterPassivesList)
        {
            if (!passive.Visible) continue;
            pass += $"__**{passive.PassiveName}**__";
            pass += ": ";
            pass += passive.PassiveDescription;
            pass += "\n";
        }


        var embed = new EmbedBuilder();
        embed.WithColor(Color.DarkOrange);
        //if (character.Avatar != null)
        //     embed.WithImageUrl(character.Avatar);
        embed.AddField("Твой Персонаж:", $"Name: {character.Name}\n" +
                                         $"Интеллект: {character.GetIntelligenceString()}\n" +
                                         $"Сила: {character.GetStrength()}\n" +
                                         $"Скорость: {character.GetSpeed()}\n" +
                                         $"Психика: {character.GetPsyche()}\n");
        embed.AddField("Пассивки", $"{pass}");

        //if(character.Description.Length > 1)
        //    embed.WithDescription(character.Description);


        await user.SendMessageAsync("", false, embed.Build());
    }

    public async Task WaitMess(GamePlayerBridgeClass player, List<GamePlayerBridgeClass> players)
    {
        if (player.DiscordId <= 1000000) return;

        var globalAccount = _global.Client.GetUser(player.DiscordId);


        await ShowRulesAndChar(globalAccount, player);

        var mainPage = new EmbedBuilder();
        mainPage.WithAuthor(globalAccount);
        mainPage.WithFooter("Preparation time...");
        mainPage.WithColor(Color.DarkGreen);
        mainPage.AddField("Game is being ready", "**Please wait for the main menu**");


        var socketMsg = await globalAccount.SendMessageAsync("", false, mainPage.Build());

        player.Status.SocketMessageFromBot = socketMsg;
    }

    public string LeaderBoard(GamePlayerBridgeClass player)
    {
        var game = _global.GamesList.Find(x => x.GameId == player.GameId);
        if (game == null) return "ERROR 404";
        var players = "";
        var playersList = game.PlayersList;

        for (var i = 0; i < playersList.Count; i++)
        {
            players += CustomLeaderBoardBeforeNumber(player, playersList[i], game, i + 1);
            var sanitizedDiscordUsername = playersList[i].DiscordUsername.Replace("_", "\\_")
                .Replace("*", "\\*")
                .Replace("~", "\\~")
                .Replace("`", "\\`");

            var teamString = "";
            if (game.Teams.Any(x => x.TeamPlayers.Contains(playersList[i].Status.PlayerId)))
            {
                var teamId = game.Teams.Find(x => x.TeamPlayers.Contains(playersList[i].Status.PlayerId))!.TeamId;
                teamString = $"[{teamId}] ";
                if(teamId == game.Teams.Find(x => x.TeamPlayers.Contains(player.Status.PlayerId))!.TeamId)
                    teamString = $"**[{teamId}]** ";
            }

            players += $"{teamString}{i + 1}. {sanitizedDiscordUsername}";

            players += CustomLeaderBoardAfterPlayer(player, playersList[i], game);

            if (player.GetPlayerId() == playersList[i].GetPlayerId())
                players += $" = **{playersList[i].Status.GetScore()} Score**";


            players += "\n";
        }

        return players;
    }

    public string CustomLeaderBoardBeforeNumber(GamePlayerBridgeClass player1, GamePlayerBridgeClass player2,
        GameClass game, int number)
    {
        var customString = "";

        switch (player1.Character.Name)
        {
            case "Осьминожка":
                var octoTentacles = _gameGlobal.OctopusTentaclesList.Find(x =>
                    x.GameId == game.GameId && x.PlayerId == player1.GetPlayerId());

                if (!octoTentacles.LeaderboardPlace.Contains(number)) customString += "🐙";


                break;

            case "Братишка":
                var shark = _gameGlobal.SharkJawsLeader.Find(x =>
                    x.GameId == game.GameId && x.PlayerId == player1.GetPlayerId());

                if (!shark.FriendList.Contains(number)) customString += "🐙";
                break;
        }

        return customString + " ";
    }

    public string CustomLeaderBoardAfterPlayer(GamePlayerBridgeClass me, GamePlayerBridgeClass other, GameClass game)
    {
        var customString = "";
        //|| me.DiscordId == 238337696316129280 || me.DiscordId == 181514288278536193


        switch (me.Character.Name)
        {
            case "AWDKA":
                if (other.GetPlayerId() == me.GetPlayerId()) break;

                var awdka = _gameGlobal.AwdkaTryingList.Find(x =>
                    x.GameId == game.GameId && x.PlayerId == me.GetPlayerId());
                var awdkaTrainingHistory = _gameGlobal.AwdkaTeachToPlayHistory.Find(x =>
                    x.GameId == game.GameId && x.PlayerId == me.GetPlayerId());

                var awdkaTrying = awdka.TryingList.Find(x => x.EnemyPlayerId == other.GetPlayerId());

                if (awdkaTrying != null)
                {
                    if (!awdkaTrying.IsUnique) customString += " <:bronze:565744159680626700>";
                    else customString += " <:plat:565745613208158233>";
                }


                if (awdkaTrainingHistory != null)
                {
                    var awdkaTrainingHistoryEnemy =
                        awdkaTrainingHistory.History.Find(x => x.EnemyPlayerId == other.GetPlayerId());
                    if (awdkaTrainingHistoryEnemy != null)
                    {
                        var statText = awdkaTrainingHistoryEnemy.Text switch
                        {
                            "1" => "Интеллект",
                            "2" => "Сила",
                            "3" => "Скорость",
                            "4" => "Психика",
                            _ => ""
                        };
                        customString += $" (**{statText} {awdkaTrainingHistoryEnemy.Stat}** ?)";
                    }
                }
                //(<:volibir:894286361895522434> сила 10 ?)


                break;
            case "Братишка":
                var shark = _gameGlobal.SharkJawsWin.Find(x =>
                    x.GameId == game.GameId && x.PlayerId == me.GetPlayerId());
                if (!shark.FriendList.Contains(other.GetPlayerId()) &&
                    other.GetPlayerId() != me.GetPlayerId())
                    customString += " <:jaws:565741834219945986>";
                break;

            case "Darksci":
                var dar = _gameGlobal.DarksciLuckyList.Find(x =>
                    x.GameId == game.GameId &&
                    x.PlayerId == me.GetPlayerId());

                if (!dar.TouchedPlayers.Contains(other.GetPlayerId()) &&
                    other.GetPlayerId() != me.GetPlayerId())
                    customString += " <:Darksci:565598465531576352>";


                break;
            case "Вампур":
                var vamp = _gameGlobal.VampyrHematophagiaList.Find(x =>
                    x.GameId == me.GameId && x.PlayerId == me.GetPlayerId());
                var target = vamp.Hematophagia.Find(x => x.EnemyId == other.GetPlayerId());
                if (target != null)
                    customString += " <:Y_:562885385395634196>";
                break;

            case "HardKitty":
                var hardKitty = _gameGlobal.HardKittyDoebatsya.Find(x =>
                    x.GameId == me.GameId && x.PlayerId == me.GetPlayerId());
                if (hardKitty != null)
                {
                    var lostSeries = hardKitty.LostSeries.Find(x => x.EnemyPlayerId == other.GetPlayerId());
                    if (lostSeries != null)
                    {
                        switch (lostSeries.Series)
                        {
                            case > 9:
                                customString += $" <:LoveLetter:998306315342454884> - {lostSeries.Series}";
                                break;
                            case > 0:
                                customString += $" <:393:563063205811847188> - {lostSeries.Series}";
                                break;
                        }
                    }
                }

                break;
            case "Sirinoks":
                var siri = _gameGlobal.SirinoksFriendsList.Find(x =>
                    x.GameId == me.GameId && x.PlayerId == me.GetPlayerId());

                if (siri != null)
                    if (!siri.FriendList.Contains(other.GetPlayerId()) &&
                        other.GetPlayerId() != me.GetPlayerId())
                        customString += " <:fr:563063244097585162>";
                break;
            case "Загадочный Спартанец в маске":

                var SpartanShame = _gameGlobal.SpartanShame.Find(x =>
                    x.GameId == game.GameId && x.PlayerId == me.GetPlayerId());

                if (!SpartanShame.FriendList.Contains(other.GetPlayerId()) &&
                    other.GetPlayerId() != me.GetPlayerId())
                    customString += " <:yasuo:895819754428833833>";

                if (SpartanShame.FriendList.Contains(other.GetPlayerId()) &&
                    other.GetPlayerId() != me.GetPlayerId() && other.Character.Name == "mylorik")
                    customString += " <:Spartaneon:899847724936089671>";


                var SpartanMark = _gameGlobal.SpartanMark.Find(x =>
                    x.GameId == me.GameId && x.PlayerId == me.GetPlayerId());

                if (SpartanMark.FriendList.Contains(other.GetPlayerId()))
                    customString += " <:sparta:561287745675329567>";


                break;


            case "DeepList":

                //tactic
                var deep = _gameGlobal.DeepListDoubtfulTactic.Find(x =>
                    x.PlayerId == me.GetPlayerId() && me.GameId == x.GameId);
                if (deep != null)
                    if (deep.FriendList.Contains(other.GetPlayerId()) &&
                        other.GetPlayerId() != me.GetPlayerId())
                        customString += " <:yo_filled:902361411840266310>";
                //end tactic

                //сверхразум
                var currentList = _gameGlobal.DeepListSupermindKnown.Find(x =>
                    x.PlayerId == me.GetPlayerId() && x.GameId == me.GameId);
                if (currentList != null)
                    if (currentList.KnownPlayers.Contains(other.GetPlayerId()))
                        customString +=
                            $" PS: - {other.Character.Name} (I: {other.Character.GetIntelligence()} | " +
                            $"St: {other.Character.GetStrength()} | Sp: {other.Character.GetSpeed()} | " +
                            $"Ps: {other.Character.GetPsyche()} | J: {other.Character.Justice.GetFullJusticeNow()})";
                //end сверхразум


                //стёб
                var currentDeepList =
                    _gameGlobal.DeepListMockeryList.Find(x =>
                        x.PlayerId == me.GetPlayerId() && game.GameId == x.GameId);

                if (currentDeepList != null)
                {
                    var currentDeepList2 =
                        currentDeepList.WhoWonTimes.Find(x => x.EnemyPlayerId == other.GetPlayerId());

                    if (currentDeepList2 != null)
                    {
                        if (currentDeepList2.Times == 1)
                            customString += " **лол**";
                        if (currentDeepList2.Triggered)
                            customString += " **кек**";
                    }
                }

                //end стёб


                break;

            case "mylorik":
                var mylorik = _gameGlobal.MylorikRevenge.Find(x =>
                    x.GameId == me.GameId && x.PlayerId == me.GetPlayerId());
                var find = mylorik?.EnemyListPlayerIds.Find(x =>
                    x.EnemyPlayerId == other.GetPlayerId());

                if (find != null && find.IsUnique) customString += " <:sparta:561287745675329567>";
                if (find != null && !find.IsUnique) customString += " ❌";

                var mylorikSpartan =
                    _gameGlobal.MylorikSpartan.Find(x => x.GameId == me.GameId && x.PlayerId == me.GetPlayerId());

                var mylorikEnemy = mylorikSpartan.Enemies.Find(x => x.EnemyId == other.GetPlayerId());

                if (mylorikEnemy != null)
                    if (mylorikEnemy.LostTimes > 0)
                        switch (mylorikEnemy.LostTimes)
                        {
                            case 1:
                                customString += " <:broken_shield:902044789917241404>";
                                break;
                            case 2:
                                customString +=
                                    " <:broken_shield:902044789917241404><:broken_shield:902044789917241404>";
                                break;
                            case 3:
                                customString +=
                                    " <:broken_shield:902044789917241404><:broken_shield:902044789917241404>🍰🍰";
                                break;
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                            case 8:
                            case 9:
                            case 10:
                                customString +=
                                    " <:broken_shield:902044789917241404><:broken_shield:902044789917241404><:broken_shield:902044789917241404><:broken_shield:902044789917241404><:broken_shield:902044789917241404><:broken_shield:902044789917241404><:broken_shield:902044789917241404>🎂 **НЯМ!**";
                                break;
                        }

                break;
            case "Тигр":
                var tigr1 = _gameGlobal.TigrTwoBetterList.Find(x =>
                    x.PlayerId == me.GetPlayerId() && x.GameId == me.GameId);

                if (tigr1 != null)
                    //if (tigr1.FriendList.Contains(other.GetPlayerId()) && other.GetPlayerId() != me.GetPlayerId())
                    if (tigr1.FriendList.Contains(other.GetPlayerId()))
                        customString += " <:pepe_down:896514760823144478>";

                var tigr2 = _gameGlobal.TigrThreeZeroList.Find(x =>
                    x.GameId == me.GameId && x.PlayerId == me.GetPlayerId());

                var enemy = tigr2?.FriendList.Find(x => x.EnemyPlayerId == other.GetPlayerId());

                if (enemy != null)
                {
                    if (enemy.WinsSeries == 1)
                        customString += " 1:0";
                    else if (enemy.WinsSeries == 2)
                        customString += " 2:0";
                    else if (enemy.WinsSeries >= 3) customString += " 3:0, обоссан";
                }

                break;
        }

        var knownClass = me.Status.KnownPlayerClass.Find(x => x.EnemyId == other.GetPlayerId());
        if (knownClass != null && me.Character.Name != "AWDKA")
            customString += $" {knownClass.Text}";


        if (game.RoundNo == 11)
            customString += $" (as **{other.Character.Name}**) = {other.Status.GetScore()} Score";

        if (me.PlayerType == 2)
        {
            customString += $" (as **{other.Character.Name}**) = {other.Status.GetScore()} Score";
            customString +=
                $" (I: {other.Character.GetIntelligence()} | St: {other.Character.GetStrength()} | Sp: {other.Character.GetSpeed()} | Ps: {other.Character.GetPsyche()})";
        }

        var predicted = me.Predict.Find(x => x.PlayerId == other.GetPlayerId());
        if (predicted != null)
            customString += $"<:e_:562879579694301184>|<:e_:562879579694301184>{predicted.CharacterName} ?";

        return customString;
    }

    public async Task EndGame(SocketMessageComponent button)
    {
        _helperFunctions.SubstituteUserWithBot(button.User.Id);
        var globalAccount = _global.Client.GetUser(button.User.Id);
        var account = _accounts.GetAccount(globalAccount);
        account.IsPlaying = false;


        //  await socketMsg.DeleteAsync();
        await globalAccount.SendMessageAsync("Спасибо за игру!\nА вы знали? Это многопользовательская игра до 6 игроков! Вы можете начать игру с другом пинганв его! Например `*st @Boole`");
    }

    private static IEnumerable<string> Split(string str, int chunkSize)
    {
        return Enumerable.Range(0, str.Length / chunkSize)
            .Select(i => str.Substring(i * chunkSize, chunkSize));
    }


    public string HandleCasualNormalSkillShow(string text, GamePlayerBridgeClass player, GameClass game)
    {
        if (player.PlayerType == 0)
            foreach (var p in game.PlayersList)
            {
                if (p.GetPlayerId() == player.GetPlayerId())
                    continue;
                foreach (var passive in p.Character.Passive)
                    if (passive.PassiveName != "Запах мусора" && passive.PassiveName != "Чернильная завеса" && passive.PassiveName != "Еврей" && passive.PassiveName != "2kxaoc")
                        text = text.Replace($"{passive.PassiveName}", "❓");
            }


        if (text.Contains("Класс:"))
        {
            var temp = "";
            var jewSplit = text.Split('\n');
            var totalClass = 0;

            foreach (var line in jewSplit)
                if (!line.Contains("Класс:"))
                {
                    temp += line + "\n";
                }
                else
                {
                    var classText = line.Split(' ')[1].Replace("+", "");
                    totalClass += Convert.ToInt32(classText);
                }

            temp = temp.Remove(temp.Length - 1);
            temp += $"Класс: +{totalClass} *Cкилла*\n";
            text = temp;
        }


        if (text.Contains("Cкилла"))
        {
            var temp = "";
            var jewSplit = text.Split('\n');

            foreach (var line in jewSplit)
                if (!line.Contains("Cкилла"))
                    temp += line + "\n";

            foreach (var line in jewSplit)
                if (line.Contains("Cкилла"))
                    temp += line + "\n";

            text = temp;
        }


        if (text.Contains("__**бонусных**__ очков"))
        {
            var temp = "";
            var jewSplit = text.Split('\n');

            foreach (var line in jewSplit)
                if (!line.Contains("__**бонусных**__ очков"))
                    temp += line + "\n";

            foreach (var line in jewSplit)
                if (line.Contains("__**бонусных**__ очков"))
                    temp += line + "\n";

            text = temp;
        }


        if (text.Contains("Евреи..."))
        {
            var temp = "";
            var jewSplit = text.Split('\n');

            foreach (var line in jewSplit)
                if (!line.Contains("Евреи..."))
                    temp += line + "\n";

            foreach (var line in jewSplit)
                if (line.Contains("Евреи..."))
                    temp += line + "\n";

            text = temp;
        }

        
        if (text.Contains("**обычных** очков"))
        {
            var temp = "";
            var jewSplit = text.Split('\n');

            foreach (var line in jewSplit)
                if (!line.Contains("**обычных** очков"))
                    temp += line + "\n";

            foreach (var line in jewSplit)
                if (line.Contains("**обычных** очков"))
                    temp += line + "\n";

            text = temp;
        }


        if (text.Contains("**очков**"))
        {
            var temp = "";
            var jewSplit = text.Split('\n');

            foreach (var line in jewSplit)
                if (!line.Contains("**очков**"))
                    temp += line + "\n";

            foreach (var line in jewSplit)
                if (line.Contains("**очков**"))
                    temp += line + "\n";

            text = temp;
        }

        return text.Replace("\n\n", "\n");
    }


    public string HandleIsNewPlayerDescription(string text, GamePlayerBridgeClass player)
    {
        var account = _accounts.GetAccount(player.DiscordId);
        if (account.IsNewPlayer) text = text.Replace("⟶", "⟶ победил");

        text = text.Replace(player.DiscordUsername, $"**{player.DiscordUsername}**");

        return text;
    }

    //Page 1
    public EmbedBuilder FightPage(GamePlayerBridgeClass player)
    {
        var game = _global.GamesList.Find(x => x.GameId == player.GameId);
        var character = player.Character;

        var embed = new EmbedBuilder();
        embed.WithColor(Color.Blue);
        embed.WithTitle("King of the Garbage Hill");
        embed.WithFooter($"{GetTimeLeft(player)}");
        var roundNumber = game.RoundNo;


        if (roundNumber > 10) roundNumber = 10;

        var multiplier = roundNumber switch
        {
            <= 4 => 1,
            <= 9 => 2,
            _ => 4
        };
        //Претендент русского сервера
        if (player.Status.GetInGamePersonalLogs().Contains("Претендент русского сервера")) multiplier *= 3;
        //end Претендент русского сервера

        game = _global.GamesList.Find(x => x.GameId == player.GameId);


        var desc = HandleIsNewPlayerDescription(game.GetGlobalLogs(), player);


        embed.WithDescription($"{desc}" +
                              "**▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬**\n" +
                              $"**Интеллект:** {character.GetIntelligenceString()}\n" +
                              $"**Сила:** {character.GetStrengthString()}\n" +
                              $"**Скорость:** {character.GetSpeedString()}\n" +
                              $"**Психика:** {character.GetPsycheString()}\n" +
                              "**▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬**\n" +
                              $"*Справедливость: **{character.Justice.GetFullJusticeNow()}***\n" +
                              $"*Мораль: {character.GetMoral()}*\n" +
                              $"*Скилл: {character.GetSkillDisplay()} (Мишень: **{character.GetCurrentSkillClassTarget()}**)*\n" +
                              $"*Класс:* {character.GetClassStatDisplayText()}\n" +
                              "**▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬▬**\n" +
                              $"Множитель очков: **x{multiplier}**\n" +
                              "<:e_:562879579694301184>\n" +
                              $"{LeaderBoard(player)}");


        var splitLogs = player.Status.InGamePersonalLogsAll.Split("|||");

        var text = "";
        if (splitLogs.Length > 1 && splitLogs[^2].Length > 3 && game.RoundNo > 1)
        {
            text = splitLogs[^2];
            text = HandleCasualNormalSkillShow(text, player, game);
            embed.AddField("События прошлого раунда:", $"{text}");
        }
        else
        {
            embed.AddField("События прошлого раунда:", "В прошлом раунде ничего не произошло. Странно...");
        }

        text = player.Status.GetInGamePersonalLogs().Length >= 2
            ? $"{player.Status.GetInGamePersonalLogs()}"
            : "Еще ничего не произошло. Наверное...";
        text = HandleCasualNormalSkillShow(text, player, game);
        embed.AddField("События этого раунда:", text);


       
        embed.WithThumbnailUrl(character.AvatarCurrent);

        return embed;
    }

    //Page 2
    /*public EmbedBuilder LogsPage(GamePlayerBridgeClass player)
   {
      var game = _global.GamesList.Find(x => x.GameId == player.GameId);

       var embed = new EmbedBuilder();
       embed.WithTitle("Логи");
       embed.WithDescription(game.GetAllGlobalLogs());
       embed.WithColor(Color.Green);
       embed.WithFooter($"{GetTimeLeft(player)}");
       embed.WithCurrentTimestamp();

       return embed;
}*/

    //Page 3
    public EmbedBuilder LvlUpPage(GamePlayerBridgeClass player)
    {
        var character = player.Character;
        var embed = new EmbedBuilder();
        var text = "__Подними один из статов на 1:__";
        if (player.Character.Name == "Вампур_")
        {
            text = "**Понизить** один из статов на 1!";
        }
        embed.WithColor(Color.Blue);
        embed.WithFooter($"{GetTimeLeft(player)}");
        embed.WithCurrentTimestamp();
        embed.AddField("_____",
            $"{text}\n \n" +
            $"1. **Интеллект:** {character.GetIntelligence()}\n" +
            $"2. **Сила:** {character.GetStrength()}\n" +
            $"3. **Скорость:** {character.GetSpeed()}\n" +
            $"4. **Психика:** {character.GetPsyche()}\n");

        
        embed.WithThumbnailUrl(character.AvatarCurrent);

        return embed;
    }


    public SelectMenuBuilder GetAttackMenu(GamePlayerBridgeClass player, GameClass game)
    {
        var isDisabled = player.Status.IsBlock || player.Status.IsSkip || player.Status.IsReady || game.RoundNo > 10;
        var placeHolder = "Выбрать цель";

        if (player.Status.IsSkip) placeHolder = "Что-то заставило тебя скипнуть...";

        if (player.Status.IsBlock) placeHolder = "Ты поставил блок!";

        if (player.Status.IsAutoMove) placeHolder = "Ты использовал Авто Ход!";

        if (game.RoundNo > 10) placeHolder = "gg wp";

        if (player.Status.IsReady)
        {
            var target = game.PlayersList.Find(x => x.GetPlayerId() == player.Status.WhoToAttackThisTurn);
            if (target != null) placeHolder = $"Ты напал на {target.DiscordUsername}";
        }

        if (!player.Status.ConfirmedPredict)
        {
            isDisabled = true;
            placeHolder = "Подтвердите свои предложение перед атакой!";
        }

        if (!player.Status.ConfirmedSkip)
        {
            isDisabled = true;
            placeHolder = "Что-то заставило тебя скипнуть...";
        }

        if (!player.Status.ConfirmedSkip && player.Character.Name == "Тигр")
        {
            isDisabled = true;
            placeHolder = "Обжаловать бан...";
        }

        var attackMenu = new SelectMenuBuilder()
            .WithMinValues(1)
            .WithMaxValues(1)
            .WithCustomId("attack-select")
            .WithDisabled(isDisabled)
            .WithPlaceholder(placeHolder);


        if (game != null)
            for (var i = 0; i < _playerChoiceAttackList.Count; i++)
            {
                var playerToAttack = game.PlayersList.Find(x => x.Status.PlaceAtLeaderBoard == i + 1);
                if (playerToAttack == null) continue;
                if (playerToAttack.DiscordId != player.DiscordId)
                    attackMenu.AddOption("Напасть на " + playerToAttack.DiscordUsername, $"{i + 1}",
                        emote: _playerChoiceAttackList[i]);
            }

        return attackMenu;
    }
    
    public SelectMenuBuilder GetDopaMenu(GamePlayerBridgeClass player, GameClass game)
    {
        var isDisabled = !(player.Status.IsBlock || player.Status.WhoToAttackThisTurn != Guid.Empty);

        var placeHolder = "Второе Действие";

        if (player.Status.IsSkip) placeHolder = "당신을 건너 뛰게 만든 무언가"; //сон

        if (game.RoundNo > 10) placeHolder = "ㅈㅈ"; //gg

        if (player.Status.IsReady)
        {
            var target = game.PlayersList.Find(x => x.GetPlayerId() == player.Status.WhoToAttackThisTurn);
            if (target != null) placeHolder = $"Ты напал на {target.DiscordUsername}";
        }

        if (!player.Status.ConfirmedPredict)
        {
            isDisabled = true;
            placeHolder = "Подтвердите свои предложение перед атакой!";
        }

        if (!player.Status.ConfirmedSkip)
        {
            isDisabled = true;
            placeHolder = "당신을 건너 뛰게 만든 무언가"; //сон
        }


        var attackMenu = new SelectMenuBuilder()
            .WithMinValues(1)
            .WithMaxValues(1)
            .WithCustomId("dopa-attack-select")
            .WithDisabled(isDisabled)
            .WithPlaceholder(placeHolder);

        if (game != null)
            for (var i = 0; i < _playerChoiceAttackList.Count; i++)
            {
                var playerToAttack = game.PlayersList.Find(x => x.Status.PlaceAtLeaderBoard == i + 1);
                if (playerToAttack == null) continue;
                if (playerToAttack.DiscordId != player.DiscordId)
                    attackMenu.AddOption("Напасть на " + playerToAttack.DiscordUsername, $"{i + 1}",
                        emote: _playerChoiceAttackList[i]);
            }

        return attackMenu;
    }

    public SelectMenuBuilder GetPredictMenu(GamePlayerBridgeClass player, GameClass game)
    {
        var predictMenu = new SelectMenuBuilder()
            .WithMinValues(1)
            .WithMaxValues(1)
            .WithCustomId("predict-1")
            .WithDisabled(game.RoundNo >= 9)
            .WithPlaceholder("Сделать предположение");

        if (game != null)
            for (var i = 0; i < _playerChoiceAttackList.Count; i++)
            {
                var playerToAttack = game.PlayersList.Find(x => x.Status.PlaceAtLeaderBoard == i + 1);
                if (playerToAttack == null) continue;
                if (playerToAttack.DiscordId != player.DiscordId)
                    predictMenu.AddOption(playerToAttack.DiscordUsername + " это...",
                        playerToAttack.DiscordUsername,
                        emote: _playerChoiceAttackList[i]);
            }


        return predictMenu;
    }


    public async Task<SelectMenuBuilder> GetLvlUpMenu(GamePlayerBridgeClass player, GameClass game)
    {
        var placeholderText = "Выбор прокачки";
        if (player.Character.Name == "Вампур_")
        {
            placeholderText = _vampyrGarlic[_random.Random(0, _vampyrGarlic.Count - 1)];
        }
        var charMenu = new SelectMenuBuilder()
            .WithMinValues(1)
            .WithMaxValues(1)
            .WithCustomId("char-select")
            .WithPlaceholder(placeholderText)
            .AddOption("Интеллект", "1")
            .AddOption("Сила", "2")
            .AddOption("Скорость", "3")
            .AddOption("Психика", "4");


        //Да всё нахуй эту игру Part #4
        if (game.RoundNo == 9 && player.Character.GetPsyche() == 4 && player.Character.Name == "Darksci")
        {
            charMenu = new SelectMenuBuilder()
                .WithMinValues(1)
                .WithMaxValues(1)
                .WithCustomId("char-select")
                .WithPlaceholder("\"Выбор\" прокачки")
                .AddOption("Психика", "4");
            await _helperFunctions.SendMsgAndDeleteItAfterRound(player, "Riot Games: бери smite и не выебывайся");
        }
        //end Да всё нахуй эту игру: Part #4


        return charMenu;
    }


    public ButtonBuilder GetMoralToPointsButton(GamePlayerBridgeClass player, GameClass game)
    {
        var disabled = game is not { RoundNo: <= 10 };
        var extraText = "";
        if (game.RoundNo == 10) extraText = " (Конец игры)";

        //if (player.Character.Name == "Братишка")
        //    return new ButtonBuilder($"Буууууууль", "moral", ButtonStyle.Secondary, isDisabled: true);
        if (player.Character.Name == "DeepList")
            return new ButtonBuilder($"Интересует только скилл", "moral", ButtonStyle.Secondary, isDisabled: true);

        if (player.Character.GetMoral() >= 20)
            return new ButtonBuilder($"на 10 бонусных очков{extraText}", "moral", ButtonStyle.Secondary, isDisabled: disabled);
        if (player.Character.GetMoral() >= 13)
            return new ButtonBuilder($"на 5 бонусных очков{extraText}", "moral", ButtonStyle.Secondary, isDisabled: disabled);
        if (player.Character.GetMoral() >= 8)
            return new ButtonBuilder($"на 2 бонусных очков{extraText}", "moral", ButtonStyle.Secondary, isDisabled: disabled);
        if (player.Character.GetMoral() >= 5)
            return new ButtonBuilder($"на 1 бонусных очка{extraText}", "moral", ButtonStyle.Secondary, isDisabled: disabled);
        return new ButtonBuilder("Недостаточно очков Морали", "moral", ButtonStyle.Secondary, isDisabled: true);
    }

    public ButtonBuilder GetMoralToSkillButton(GamePlayerBridgeClass player, GameClass game)
    {
        if (!player.Status.ConfirmedPredict)
            return new ButtonBuilder("Я подтверждаю свои предположения", "confirm-prefict", ButtonStyle.Primary, isDisabled: false, emote: Emote.Parse("<a:bratishka:900962522276958298>"));
        if (!player.Status.ConfirmedSkip)
            return new ButtonBuilder("Я подтверждаю пропуск хода", "confirm-skip", ButtonStyle.Primary, isDisabled: false, emote: Emote.Parse("<a:bratishka:900962522276958298>"));


        var disabled = game is not { RoundNo: <= 10 };
        var extraText = "";
        if (game.RoundNo == 10 && player.Character.GetMoral() < 3) extraText = " (Конец игры)";

        //if (player.Character.Name == "Братишка")
        //    return new ButtonBuilder($"Ничего не понимает...", "skill", ButtonStyle.Secondary, isDisabled: true);

        if (player.Character.GetMoral() >= 20)
            return new ButtonBuilder($"Обменять 20 Морали на 100 Cкилла{extraText}", "skill", ButtonStyle.Secondary, isDisabled: disabled);
        if (player.Character.GetMoral() >= 13)
            return new ButtonBuilder($"Обменять 13 Морали на 50 Cкилла{extraText}", "skill", ButtonStyle.Secondary, isDisabled: disabled);
        if (player.Character.GetMoral() >= 8)
            return new ButtonBuilder($"Обменять 8 Морали на 30 Cкилла{extraText}", "skill", ButtonStyle.Secondary, isDisabled: disabled);
        if (player.Character.GetMoral() >= 5)
            return new ButtonBuilder($"Обменять 5 Морали на 18 Cкилла{extraText}", "skill", ButtonStyle.Secondary, isDisabled: disabled);
        if (player.Character.GetMoral() >= 3)
            return new ButtonBuilder($"Обменять 3 Морали на 10 Cкилла{extraText}", "skill", ButtonStyle.Secondary, isDisabled: disabled);
        if (player.Character.GetMoral() >= 2)
            return new ButtonBuilder($"Обменять 2 Морали на 6 Cкилла{extraText}", "skill", ButtonStyle.Secondary, isDisabled: disabled);
        if (player.Character.GetMoral() >= 1)
            return new ButtonBuilder($"Обменять 1 Морали на 4 Cкилла{extraText}", "skill", ButtonStyle.Secondary, isDisabled: disabled);


        return new ButtonBuilder("Недостаточно очков Морали", "skill", ButtonStyle.Secondary, isDisabled: true);
    }

    public async Task<ComponentBuilder> GetGameButtons(GamePlayerBridgeClass player, GameClass game, SelectMenuBuilder predictMenu = null)
    {
        var components = new ComponentBuilder();
        components.WithButton(GetBlockButton(player, game));
        components.WithButton(GetAutoMoveButton(player, game));
        components.WithButton(GetChangeMindButton(player, game));
        components.WithButton(GetEndGameButton());

        components.WithSelectMenu(GetAttackMenu(player, game), 1);

        components.WithButton(GetMoralToSkillButton(player, game), 2);

        if(player.Character.GetMoral() >= 3)
            if (player.Status.ConfirmedPredict && player.Status.ConfirmedSkip)
                components.WithButton(GetMoralToPointsButton(player, game), 2);

        components.WithSelectMenu(predictMenu ?? GetPredictMenu(player, game), 3);

        switch (player.Character.Name)
        {
            case "Darksci":
                var darksciType = _gameGlobal.DarksciTypeList.Find(x => x.PlayerId == player.GetPlayerId() && game.GameId == x.GameId);
                if (game.RoundNo == 1 && !darksciType.Triggered)
                {
                    components.WithButton(new ButtonBuilder("Мне никогда не везёт...", "stable-Darksci", ButtonStyle.Primary), 4);
                    components.WithButton(new ButtonBuilder("Мне сегодня повезёт!", "not-stable-Darksci", ButtonStyle.Danger), 4);
                    if (!darksciType.Sent)
                    {
                        darksciType.Sent = true;
                        await _helperFunctions.SendMsgAndDeleteItAfterRound(player, "Нажмешь синюю кнопку - и сказке конец. Выберешь красную - и узнаешь насколько глубока нора Даркси.");
                    }
                }
                break;

            case "Dopa":
                components.WithSelectMenu(GetDopaMenu(player, game), 4);
                break;
        }

        return components;
    }


    public ButtonBuilder GetBlockButton(GamePlayerBridgeClass player, GameClass game)
    {
        var playerIsReady = player.Status.IsBlock || player.Status.IsSkip || player.Status.IsReady || game.RoundNo > 10;
        return new ButtonBuilder("Блок", "block", ButtonStyle.Success, isDisabled: playerIsReady);
    }

    public ButtonBuilder GetEndGameButton()
    {
        return new ButtonBuilder("Завершить Игру", "end", ButtonStyle.Danger);
    }

    public ButtonBuilder GetChangeMindButton(GamePlayerBridgeClass player, GameClass game)
    {
        if(player.Character.Name == "Dopa")
            return new ButtonBuilder("선택 변경", "change-mind", ButtonStyle.Secondary, isDisabled: true);

        if (player.Status.IsReady && player.Status.IsAbleToChangeMind && !player.Status.IsSkip)
            return new ButtonBuilder("Изменить свой выбор", "change-mind", ButtonStyle.Secondary, isDisabled: false);

        return new ButtonBuilder("Изменить свой выбор", "change-mind", ButtonStyle.Secondary, isDisabled: true);
    }

    public ButtonBuilder GetAutoMoveButton(GamePlayerBridgeClass player, GameClass game)
    {
        var enabled = player.Status.IsAutoMove || player.Status.IsSkip || player.Status.IsReady || player.Character.Name == "Dopa";

        if (game.TimePassed.Elapsed.TotalSeconds < 29 && player.DiscordId != 238337696316129280 && player.DiscordId != 181514288278536193) enabled = true;

        return new ButtonBuilder("Авто Ход", "auto-move", ButtonStyle.Secondary, isDisabled: enabled);
    }

    public async Task UpdateMessage(GamePlayerBridgeClass player, string extraText = "")
    {
        if (player.IsBot()) return;

        var game = _global.GamesList.Find(x => x.GameId == player.GameId);
        var embed = new EmbedBuilder();
        var builder = new ComponentBuilder();

        switch (player.Status.MoveListPage)
        {
            case 1:
                embed = FightPage(player);
                builder = await GetGameButtons(player, game);
                break;
            case 2:
                // RESERVED

                /*embed = LogsPage(player);
                builder = new ComponentBuilder();*/
                break;
            case 3:
                embed = LvlUpPage(player);
                builder = new ComponentBuilder().WithSelectMenu(await GetLvlUpMenu(player, game));

                //Да всё нахуй эту игру Part #5
                if (game.RoundNo == 9 && player.Character.GetPsyche() == 4 && player.Character.Name == "Darksci")
                    builder.WithButton("Riot style \"choice\"", "crutch", row: 1, style: ButtonStyle.Secondary,
                        disabled: true);
                //end Да всё нахуй эту игру: Part #5
                break;
        }
        await _helperFunctions.ModifyGameMessage(player, embed, builder, extraText);
    }





    public string GetTimeLeft(GamePlayerBridgeClass player)
    {
        var game = _global.GamesList.Find(x => x.GameId == player.GameId);

        if (game == null) 
            return "ERROR";
        var time = $"({(int)game.TimePassed.Elapsed.TotalSeconds}/{game.TurnLengthInSecond}с)";
        if (player.Status.IsReady)
            return $"Ожидаем других игроков • {time} | {game.GameVersion}";
        var toReturn = $"{time} | {game.GameVersion}";
        if (player.Character.Name == "mylorik" || player.Character.Name == "DeepList")
        {
            toReturn += " | (x+х)*19";
        }
        return toReturn;
    }
}