﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using King_of_the_Garbage_Hill.DiscordFramework;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.DiscordMessages;
using King_of_the_Garbage_Hill.Game.GameGlobalVariables;
using King_of_the_Garbage_Hill.Helpers;
using King_of_the_Garbage_Hill.LocalPersistentData.FinishedGameLog;
using King_of_the_Garbage_Hill.LocalPersistentData.UsersAccounts;

namespace King_of_the_Garbage_Hill.Game.GameLogic;

public class CheckIfReady : IServiceSingleton
{
    private readonly UserAccounts _accounts;
    private readonly BotsBehavior _botsBehavior;
    private readonly FinishedGameLog _finishedGameLog;
    private readonly InGameGlobal _gameGlobal;
    private readonly GameUpdateMess _gameUpdateMess;
    private readonly Global _global;
    private readonly HelperFunctions _help;
    private readonly LoginFromConsole _logs;
    private readonly SecureRandom _random;
    private readonly CalculateRound _round;
    private readonly GameUpdateMess _upd;
    private bool _looping;


    private int finishedGames;
    public Timer LoopingTimer;

    public CheckIfReady(Global global, GameUpdateMess upd, CalculateRound round, FinishedGameLog finishedGameLog,
        GameUpdateMess gameUpdateMess, BotsBehavior botsBehavior, LoginFromConsole logs, UserAccounts accounts,
        InGameGlobal gameGlobal, HelperFunctions help, SecureRandom random)
    {
        _global = global;
        _upd = upd;
        _round = round;
        _finishedGameLog = finishedGameLog;
        _gameUpdateMess = gameUpdateMess;
        _botsBehavior = botsBehavior;
        _logs = logs;
        _accounts = accounts;
        _gameGlobal = gameGlobal;
        _help = help;
        _random = random;
        CheckTimer();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task CheckTimer()
    {
        LoopingTimer = new Timer
        {
            AutoReset = true,
            Interval = 100,
            Enabled = true
        };

        LoopingTimer.Elapsed += CheckIfEveryoneIsReady;
        return Task.CompletedTask;
    }

    private void HandlePostGameEvents(GameClass game)
    {
        var playerWhoWon = game.PlayersList.First();

        //if won phrases
        switch (playerWhoWon.Character.Name)
        {
            case "HardKitty":
                game.AddGlobalLogs("HarDKitty больше не одинок! Как много друзей!!!");

                var hard = _gameGlobal.HardKittyLoneliness.Find(x =>
                    x.GameId == game.GameId && x.PlayerId == playerWhoWon.GetPlayerId());

                if (hard != null)
                    foreach (var enemy in game.PlayersList)
                    {
                        var hardEnemy = hard.AttackHistory.Find(x => x.EnemyId == enemy.GetPlayerId());
                        if (hardEnemy != null)
                            game.PlayersList.Find(x => x.GetPlayerId() == hardEnemy.EnemyId).Status
                                .AddInGamePersonalLogs(
                                    $"HarDKitty больше не одинок! Вы принесли ему {hardEnemy.Times} очков.\n");
                    }

                break;
        }

        //if lost phrases
        foreach (var player in game.PlayersList.Where(x => x.Status.PlaceAtLeaderBoard != 1))
            switch (player.Character.Name)
            {
                case "HardKitty":
                    player.Status.AddInGamePersonalLogs("Даже имя мое написать нормально не можете");
                    break;
                case "Mit*suki*":
                    player.Status.AddInGamePersonalLogs("Блять, суки, че вы меня таким слабым сделали?");
                    break;
                case "Тигр":
                    player.Status.AddInGamePersonalLogs("Обоссанная игра, обоссанный баланс");
                    break;
            }

        //unique
        if (game.PlayersList.Any(x => x.Character.Name == "DeepList") &&
            game.PlayersList.Any(x => x.Character.Name == "mylorik"))
        {
            var mylorik = game.PlayersList.Find(x => x.Character.Name == "mylorik");
            var deepList = game.PlayersList.Find(x => x.Character.Name == "DeepList");

            var genius = true;
            foreach (var deepListPredict in deepList.Predict)
                genius = mylorik.Predict.Any(x =>
                    x.PlayerId == deepListPredict.PlayerId && x.CharacterName == deepListPredict.CharacterName);

            if (genius)
                game.AddGlobalLogs("DeepList & mylorik: Гении мыслят одинакого или одно целое уничтожает воду.");
        }

        //
        if (playerWhoWon.Status.PlaceAtLeaderBoardHistory.Find(x => x.GameRound == 10).Place != 1)
            if (game.PlayersList.Find(x => x.Status.PlaceAtLeaderBoard == 1).Status.GetScore() !=
                game.PlayersList.Find(x => x.Status.PlaceAtLeaderBoard == 2).Status.GetScore())
                game.AddGlobalLogs($"**{playerWhoWon.DiscordUsername}** вырывает **очко** на последних секундах!");
    }


    private async Task HandleLastRound(GameClass game)
    {
        game.IsCheckIfReady = false;
        //predict
        foreach (var player in from player in game.PlayersList
                 from predict in player.Predict
                 let enemy = game.PlayersList.Find(x => x.GetPlayerId() == predict.PlayerId)
                 where enemy.Character.Name == predict.CharacterName
                 select player)
        {
            player.Status.AddBonusPoints(3, "Предположение");
        }
        // predict


        //sort
        game.PlayersList = game.PlayersList.OrderByDescending(x => x.Status.GetScore()).ToList();
        for (var k = 0; k < game.PlayersList.Count; k++)
            game.PlayersList[k].Status.PlaceAtLeaderBoard = k + 1;
        //end sorting

        try
        {
            //case "AWDKA":
            var AWDKA = game.PlayersList.Find(x => x.Character.Name == "AWDKA");
            //trolling
            if (AWDKA != null)
            {
                var awdkaTroll = _gameGlobal.AwdkaTrollingList.Find(x =>
                    x.GameId == AWDKA.GameId &&
                    x.PlayerId == AWDKA.GetPlayerId());


                var enemy = awdkaTroll.EnemyList.Find(x =>
                    x.EnemyId == game.PlayersList.Find(y => y.Status.PlaceAtLeaderBoard == 1).GetPlayerId());

                var trolledText = "";
                if (enemy != null)
                {
                    var tolled = game.PlayersList.Find(x => x.GetPlayerId() == enemy.EnemyId);

                    trolledText = tolled.Character.Name switch
                    {
                        "DeepList" => "Лист Затроллился, хех",
                        "mylorik" => "Лорик Затроллился, МММ!",
                        "Глеб" => "Спящее Хуйло",
                        "LeCrisp" => "ЛеПуська Затроллилась",
                        "Толя" => "Раммус Продал Тормейл",
                        "HardKitty" => "Пакет Молока Пролился На Клавиатуру",
                        "Sirinoks" => "Айсик Затроллилась#",
                        "Mit*suki*" => "МитСУКИ Затроллился",
                        "AWDKA" => "AWDKA Затроллился сам по себе...",
                        "Осьминожка" => "Осьминожка Забулькался",
                        "Darksci" => "Даркси Нe Повeзло...",
                        "Братишка" => "Братишка Забулькался",
                        "Загадочный Спартанец в маске" => "Спатанец Затроллился!? А-я-йо...",
                        "Вампур" => "ВампYр Затроллился",
                        "Тигр" => "Тигр Обоссался, и кто теперь обоссан!?",
                        "Краборак" => "За**Краборак**чился",
                        _ => ""
                    };

                    var bonusTrolling = 0;

                    foreach (var predict in AWDKA.Predict)
                    {
                        var found = game.PlayersList.Find(x => predict.PlayerId == x.GetPlayerId() && predict.CharacterName == x.Character.Name);
                        if (found != null)
                        {
                            bonusTrolling += 2;
                        }
                    }

                    AWDKA.Status.AddBonusPoints(bonusTrolling + ((enemy.Score + 1) / 2), $"**Произошел Троллинг:** {trolledText} ");
                    game.Phrases.AwdkaTrolling.SendLog(AWDKA, true);
                }

                //sort
                game.PlayersList = game.PlayersList.OrderByDescending(x => x.Status.GetScore()).ToList();
                for (var k = 0; k < game.PlayersList.Count; k++)
                    game.PlayersList[k].Status.PlaceAtLeaderBoard = k + 1;
                //end sorting

                if (enemy != null && game.PlayersList.First().Character.Name == "AWDKA")
                    game.AddGlobalLogs($"**Произошел Троллинг:** {trolledText} ");
            }
            //end //trolling
        }

        catch (Exception exception)
        {
            _logs.Critical(exception.Message);
            _logs.Critical(exception.StackTrace);
        }

        foreach (var t in game.PlayersList)
            t.Status.PlaceAtLeaderBoardHistory.Add(
                new InGameStatus.PlaceAtLeaderBoardHistoryClass(game.RoundNo, t.Status.PlaceAtLeaderBoard));

        HandlePostGameEvents(game);


        if (game.PlayersList.First().Status.AutoMoveTimes >= 10) game.PlayersList.First().DiscordUsername = "НейроБот";

        if (game.PlayersList.First().Status.AutoMoveTimes >= 9 && game.PlayersList.First().Character.Name == "Тигр")
            game.PlayersList.First().DiscordUsername = "НейроБот";

        var isTeam = false;
        var wonScore = 0;
        var team1Score = 0;
        var team2Score = 0;
        var team3Score = 0;
        var wonTeam = 0;
        if (game.Teams.Count > 0)
        {
            isTeam = true;
            foreach (var player in game.PlayersList)
                if (game.Teams.Find(x => x.TeamPlayers.Contains(player.Status.PlayerId))!.TeamId == 1)
                    team1Score += player.Status.GetScore();
                else if (game.Teams.Find(x => x.TeamPlayers.Contains(player.Status.PlayerId))!.TeamId == 2)
                    team2Score += player.Status.GetScore();
                else
                    team3Score += player.Status.GetScore();

            if (team1Score == team2Score && team3Score == 0)
            {
                game.AddGlobalLogs("\n**Ничья**");
            }
            else if (team1Score == team2Score && team1Score == team3Score)
            {
                game.AddGlobalLogs("\n**Ничья**");
            }
            else
            {
                if (team1Score > team2Score && team1Score > team3Score)
                {
                    wonTeam = 1;
                    wonScore = team1Score;
                }

                if (team2Score > team1Score && team2Score > team3Score)
                {
                    wonTeam = 2;
                    wonScore = team2Score;
                }

                if (team3Score > team1Score && team3Score > team2Score)
                {
                    wonTeam = 3;
                    wonScore = team3Score;
                }


                game.AddGlobalLogs($"\n**Команда #{wonTeam}** победила набрав {wonScore} Очков!");

                if (wonTeam != 1)
                    game.AddGlobalLogs($"\nКоманда #1 Набрала {team1Score} Очков.");
                if (wonTeam != 2)
                    game.AddGlobalLogs($"Команда #2 Набрала {team2Score} Очков.");
                if (wonTeam != 3)
                    if (team3Score > 0)
                        game.AddGlobalLogs($"Команда #3 Набрала {team3Score} Очков.");
            }
        }
        else
        {
            game.AddGlobalLogs(
                game.PlayersList.FindAll(x => x.Status.GetScore() == game.PlayersList.First().Status.GetScore())
                    .Count > 1
                    ? "\n**Ничья**"
                    : $"\n**{game.PlayersList.First().DiscordUsername}** победил, играя за **{game.PlayersList.First().Character.Name}**");
            if (!game.PlayersList.First().IsBot())
                game.PlayersList.First().Status.SocketMessageFromBot.Channel
                    .SendMessageAsync("__**Победа! Теперь ты Король этой Мусорной Горы. Пока-что...**__");
        }

        //todo: need to redo this system
        //_finishedGameLog.CreateNewLog(game);


        foreach (var player in game.PlayersList)
        {
            await _gameUpdateMess.UpdateMessage(player);

            var account = _accounts.GetAccount(player.DiscordId);
            account.IsPlaying = false;
            player.GameId = 1000000;


            account.TotalPlays++;
            if (account.TotalPlays > 10) account.IsNewPlayer = false;
            account.TotalWins += player.Status.PlaceAtLeaderBoard == 1 ? 1 : (ulong)0;
            account.MatchHistory.Add(
                new DiscordAccountClass.MatchHistoryClass(player.Character.Name, player.Status.GetScore(),
                    player.Status.PlaceAtLeaderBoard));

            /*
            account.ZbsPoints += (player.Status.PlaceAtLeaderBoard - 6) * -1 + 1;
            if (player.Status.PlaceAtLeaderBoard == 1)
                account.ZbsPoints += 4;
            */

            var zbsPointsToGive = 0;
            switch (player.Status.PlaceAtLeaderBoard)
            {
                case 1:
                    zbsPointsToGive = 100;
                    break;
                case 2:
                    zbsPointsToGive = 50;
                    break;
                case 3:
                    zbsPointsToGive = 40;
                    break;
                case 4:
                    zbsPointsToGive = 30;
                    break;
                case 5:
                    zbsPointsToGive = 20;
                    break;
                case 6:
                    zbsPointsToGive = 10;
                    break;
            }

            if (player.Status.GetScore() == game.PlayersList.First().Status.GetScore())
                zbsPointsToGive = 100;

            if (isTeam)
            {
                if (game.Teams.Find(x => x.TeamId == wonTeam).TeamPlayers.Contains(player.Status.PlayerId))
                    zbsPointsToGive = 100;
                else
                    zbsPointsToGive = 50;
            }

            account.ZbsPoints += zbsPointsToGive;

            var characterStatistics =
                account.CharacterStatistics.Find(x =>
                    x.CharacterName == player.Character.Name);

            if (characterStatistics == null)
            {
                account.CharacterStatistics.Add(
                    new DiscordAccountClass.CharacterStatisticsClass(player.Character.Name,
                        player.Status.PlaceAtLeaderBoard == 1 ? 1 : (ulong)0));
            }
            else
            {
                characterStatistics.Plays++;
                characterStatistics.Wins += player.Status.PlaceAtLeaderBoard == 1 ? 1 : (ulong)0;
            }

            var performanceStatistics =
                account.PerformanceStatistics.Find(x =>
                    x.Place == player.Status.PlaceAtLeaderBoard);

            if (performanceStatistics == null)
                account.PerformanceStatistics.Add(
                    new DiscordAccountClass.PerformanceStatisticsClass(player.Status.PlaceAtLeaderBoard));
            else
                performanceStatistics.Times++;
            try
            {
                if (!player.IsBot())
                    await player.Status.SocketMessageFromBot.Channel.SendMessageAsync(
                        $"Спасибо за игру!\nВы заработали **{zbsPointsToGive}** ZBS points!\n\nВы можете потратить их в магазине - `*store`\nА вы знали? Это многопользовательская игра до 6 игроков! Вы можете начать игру с другом пинганув его! Например `*st @Boole`");
            }
            catch (Exception exception)
            {
                _logs.Critical(exception.Message);
                _logs.Critical(exception.StackTrace);
            }
        }

        await NotifyOwner(game);
        _global.GamesList.Remove(game);
    }

    private async Task NotifyOwner(GameClass game)
    {
        foreach (var player in game.PlayersList)
        {
            _global.WinRates.TryGetValue(player.Character.Name, out var winrate);
            if (winrate == null)
                _global.WinRates.TryAdd(player.Character.Name, new Global.WinRateClass(player.Character.Name));
            _global.WinRates.TryGetValue(player.Character.Name, out winrate);


            winrate.GameTimes++;

            switch (player.Status.PlaceAtLeaderBoard)
            {
                case 1:
                    winrate.Top1++;
                    break;
                case 2:
                    winrate.Top2++;
                    break;
                case 3:
                    winrate.Top3++;
                    break;
                case 4:
                    winrate.Top4++;
                    break;
                case 5:
                    winrate.Top5++;
                    break;
                case 6:
                    winrate.Top6++;
                    break;
            }

            winrate.WinRate = winrate.Top1 / winrate.GameTimes * 100;
            winrate.CharacterName = player.Character.Name;
            winrate.Elo = winrate.Top1 / winrate.GameTimes * 100 * 3 + winrate.Top2 / winrate.GameTimes * 100 * 2 + winrate.Top3 / winrate.GameTimes * 100 - winrate.Top4 / winrate.GameTimes * 100 - winrate.Top5 / winrate.GameTimes * 100 * 2 - winrate.Top6 / winrate.GameTimes * 100 * 3;
        }
        finishedGames++;

        //top1 winrate
        if (finishedGames % 1000 == 0)
        {
            var winRates = _global.WinRates.Values.ToList();

            var text = $"**--------------------------------------------------------------------**\nTotal Games: {_global.GetLastGamePlayingAndId()}\n**TOP1**\n";

            var index = 1;
            foreach (var winRate in winRates.OrderByDescending(x => x.WinRate))
            {
                text += $"{index}. {winRate.CharacterName}: {winRate.WinRate.ToString("0.##")}% ({winRate.Top1}/{winRate.GameTimes})\n";
                index++;
            }

            await _global.Client.GetGuild(561282595799826432).GetTextChannel(935324189437624340).SendMessageAsync(text);
        }

        //elo winrate
        if (finishedGames % 1000 == 0)
        {
            var winRates = _global.WinRates.Values.ToList();


            var text = $"**____**\n**ELO**\n";
            var index = 1;
            foreach (var winRate in winRates.OrderByDescending(x => x.Elo))
            {
                text += $"{index}. {winRate.CharacterName}: {(int)(winRate.Elo*10)}\n";
                index++;
            }
            text += "**--------------------------------------------------------------------**";
            await _global.Client.GetGuild(561282595799826432).GetTextChannel(935324189437624340).SendMessageAsync(text);
        }
        //elo winrate end

        try
        {
            if (game.GameMode == "ShowResult")
            {
                var channel = _global.Client.GetGuild(561282595799826432).GetTextChannel(930706511632691222);
                await channel.SendMessageAsync($"Game #{game.GameId}\n" +
                                               $"Vesrion: {game.GameVersion}\n" +
                                               $"1. **{game.PlayersList.First().Character.Name} - {game.PlayersList.First().Status.GetScore()}**\n" +
                                               $"2. {game.PlayersList[1].Character.Name} - {game.PlayersList[1].Status.GetScore()}\n" +
                                               $"3. {game.PlayersList[2].Character.Name} - {game.PlayersList[2].Status.GetScore()}\n" +
                                               $"4. {game.PlayersList[3].Character.Name} - {game.PlayersList[3].Status.GetScore()}\n" +
                                               $"5. {game.PlayersList[4].Character.Name} - {game.PlayersList[4].Status.GetScore()}\n" +
                                               $"6. {game.PlayersList[5].Character.Name} - {game.PlayersList[5].Status.GetScore()}\n<:e_:562879579694301184>\n");
            }
        }
        catch (Exception exception)
        {
            _logs.Critical(exception.Message);
            _logs.Critical(exception.StackTrace);
        }
        //top1 winrate end
    }


    private async void CheckIfEveryoneIsReady(object sender, ElapsedEventArgs e)
    {
        if (_looping) return;
        _looping = true;

        var games = _global.GamesList;

        for (var i = 0; i < games.Count; i++)
        {
            var game = games[i];

            //protection against double calculations
            if (!game.IsCheckIfReady) continue;

            //round 11 is the end of the game, no fights on round 11
            if (game.RoundNo == 11)
            {
                await HandleLastRound(game);
                continue;
            }

            var players = _global.GamesList[i].PlayersList;
            var readyTargetCount = players.Count(x => !x.IsBot());
            var readyCount = 0;
            foreach (var player in players.Where(x => !x.IsBot()))
            {
                //if (game.TimePassed.Elapsed.TotalSeconds < 30) continue;

                if (game.TimePassed.Elapsed.TotalSeconds > 30 && player.Status.TimesUpdated == 0)
                {
                    player.Status.TimesUpdated++;
                    await _upd.UpdateMessage(player);
                }

                if (game.TimePassed.Elapsed.TotalSeconds > 90 && player.Status.TimesUpdated == 4)
                {
                    player.Status.TimesUpdated++;
                    await _upd.UpdateMessage(player);
                }

                if (game.TimePassed.Elapsed.TotalSeconds > 150 && player.Status.TimesUpdated == 4)
                {
                    player.Status.TimesUpdated++;
                    await _upd.UpdateMessage(player);
                }

                if (game.TimePassed.Elapsed.TotalSeconds > 210 && player.Status.TimesUpdated == 4)
                {
                    player.Status.TimesUpdated++;
                    await _upd.UpdateMessage(player);
                }

                if (game.TimePassed.Elapsed.TotalSeconds > 270 && player.Status.TimesUpdated == 4)
                {
                    player.Status.TimesUpdated++;
                    await _upd.UpdateMessage(player);
                }

                if (game.TimePassed.Elapsed.TotalSeconds < 50 && !player.Status.ConfirmedSkip) continue;
                if (player.Status.IsReady && player.Status.ConfirmedPredict)
                    readyCount++;
            }


            if (readyCount != readyTargetCount &&
                !(game.TimePassed.Elapsed.TotalSeconds >= game.TurnLengthInSecond))
                continue;

            //Calculating the game
            game.IsCheckIfReady = false;


            //If did do anything - Block
            foreach (var t in players.Where(t =>
                         !t.IsBot() && !t.Status.IsAutoMove && t.Status.WhoToAttackThisTurn == Guid.Empty &&
                         t.Status.IsBlock == false && t.Status.IsSkip == false))
            {
                _logs.Warning($"\nWARN: {t.DiscordUsername} didn't do anything - Auto Move!\n");
                t.Status.IsAutoMove = true;
                var textAutomove = "Ты не походил. Использовался Авто Ход\n";
                t.Status.AddInGamePersonalLogs(textAutomove);
                t.Status.ChangeMindWhat = textAutomove;
            }

            //If did do anything - LvL up a random stat
            foreach (var t in players.Where(t => !t.IsBot() && t.Status.MoveListPage == 3))
            {
                _logs.Warning($"\nWARN: {t.DiscordUsername} didn't do anything - Auto LvL!\n");
                t.Status.IsAutoMove = true;
                var textAutomove = "Ты не походил. Использовался Авто Ход\n";
                t.Status.AddInGamePersonalLogs(textAutomove);
                t.Status.ChangeMindWhat = textAutomove;
            }

            //handle bots

            //AWDKA last
            if (game.PlayersList.Any(x => x.Character.Name == "AWDKA"))
            {
                var tempHard = game.PlayersList.Find(x => x.Character.Name == "AWDKA");
                var hardIndex = game.PlayersList.IndexOf(tempHard);

                for (var k = hardIndex; k < game.PlayersList.Count - 1; k++)
                    game.PlayersList[k] = game.PlayersList[k + 1];

                game.PlayersList[^1] = tempHard;
            }

            //выдаем место в таблице
            for (var k = 0; k < game.PlayersList.Count; k++) game.PlayersList[k].Status.PlaceAtLeaderBoard = k + 1;
            //end //AWDKA last

            foreach (var t in players.Where(x => x.IsBot() || x.Status.IsAutoMove))
                try
                {
                    await _botsBehavior.HandleBotBehavior(t, game);
                }
                catch (Exception exception)
                {
                    _logs.Critical(exception.Message);
                    _logs.Critical(exception.StackTrace);
                }

            if (game.PlayersList.Any(x => x.Character.Name == "HardKitty"))
            {
                var tempHard = game.PlayersList.Find(x => x.Character.Name == "HardKitty");
                var hardIndex = game.PlayersList.IndexOf(tempHard);

                for (var k = hardIndex; k < game.PlayersList.Count - 1; k++)
                    game.PlayersList[k] = game.PlayersList[k + 1];

                game.PlayersList[^1] = tempHard;
            }

            //выдаем место в таблице
            for (var k = 0; k < game.PlayersList.Count; k++) game.PlayersList[k].Status.PlaceAtLeaderBoard = k + 1;
            //end //AWDKA last


            foreach (var t in players.Where(t =>
                         t.Status.WhoToAttackThisTurn == Guid.Empty && t.Status.IsBlock == false &&
                         t.Status.IsSkip == false))
                _logs.Critical($"\nCRIT: {t.DiscordUsername} didn't do anything  and auto move didn't as well.!\n");

            //delete messages from prev round. No await.
            foreach (var player in game.PlayersList) await _help.DeleteItAfterRound(player);

            await _round.CalculateAllFights(game);
            //await Task.Delay(1);

            foreach (var t in players.Where(x => !x.IsBot()))
                try
                {
                    var extraText = "";
                    if (game.RoundNo <= 10) extraText = $"Раунд #{game.RoundNo}";

                    if (game.RoundNo == 8)
                    {
                        t.Status.ConfirmedPredict = false;
                        extraText = "Это последний раунд, когда можно сделать **предложение**!";
                    }

                    if (game.RoundNo == 9) t.Status.ConfirmedPredict = true;

                    await _upd.UpdateMessage(t, extraText);
                }
                catch (Exception exception)
                {
                    _logs.Critical(exception.Message);
                    _logs.Critical(exception.StackTrace);
                }

            game.IsCheckIfReady = true;
        }

        _looping = false;
    }
}