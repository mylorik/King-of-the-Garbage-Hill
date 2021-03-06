﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using King_of_the_Garbage_Hill.BotFramework;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.DiscordMessages;
using King_of_the_Garbage_Hill.LocalPersistentData.FinishedGameLog;
using King_of_the_Garbage_Hill.LocalPersistentData.UsersAccounts;

namespace King_of_the_Garbage_Hill.Game.GameLogic
{
    public class CheckIfReady : IServiceSingleton
    {
        private readonly BotsBehavior _botsBehavior;
        private readonly FinishedGameLog _finishedGameLog;
        private readonly GameUpdateMess _gameUpdateMess;
        private readonly Global _global;
        private readonly LoginFromConsole _logs;
        private readonly CalculateRound _round;
        private readonly GameUpdateMess _upd;
        public Timer LoopingTimer;
        private readonly UserAccounts _accounts;

        public CheckIfReady(Global global, GameUpdateMess upd, CalculateRound round, FinishedGameLog finishedGameLog,
            GameUpdateMess gameUpdateMess, BotsBehavior botsBehavior, LoginFromConsole logs, UserAccounts accounts)
        {
            _global = global;
            _upd = upd;
            _round = round;
            _finishedGameLog = finishedGameLog;
            _gameUpdateMess = gameUpdateMess;
            _botsBehavior = botsBehavior;
            _logs = logs;
            _accounts = accounts;
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
                Interval = 7000,
                Enabled = true
            };

            LoopingTimer.Elapsed += CheckIfEveryoneIsReady;
            return Task.CompletedTask;
        }

        private async void CheckIfEveryoneIsReady(object sender, ElapsedEventArgs e)
        {
            var games = _global.GamesList;


            for (var i = 0; i < games.Count; i++)
            {
                var game = games[i];


                if (game.RoundNo == 11)
                {
                    //sort
                    game.PlayersList = game.PlayersList.OrderByDescending(x => x.Status.GetScore()).ToList();
                    for (var k = 0; k < game.PlayersList.Count; k++)
                        game.PlayersList[k].Status.PlaceAtLeaderBoard = k + 1;
                    //end sorting


                    game.WhoWon = game.PlayersList[0].Status.PlayerId;
                    game.AddPreviousGameLogs(
                        game.PlayersList.FindAll(x => x.Status.GetScore() == game.PlayersList[0].Status.GetScore())
                            .Count > 1
                            ? "\n**Ничья**"
                            : $"\n**{game.PlayersList[0].DiscordUsername}** победил, играя за **{game.PlayersList[0].Character.Name}**");


                    _finishedGameLog.CreateNewLog(game);


                    foreach (var player in game.PlayersList)
                    {
                        await _gameUpdateMess.UpdateMessage(player, game);

                        var account = _accounts.GetAccount(player.DiscordId);
                        account.IsPlaying = false;
                        player.GameId = 1000000;


                        account.TotalPlays++;
                        account.TotalWins += player.Status.PlaceAtLeaderBoard == 1 ? 1 : (ulong)0;
                        account.MatchHistory.Add(
                            new DiscordAccountClass.MatchHistoryClass(player.Character.Name, player.Status.GetScore(), player.Status.PlaceAtLeaderBoard));
                        account.ZbsPoints += (player.Status.PlaceAtLeaderBoard - 6) * -1 + 1;
                        if (player.Status.PlaceAtLeaderBoard == 1)
                            account.ZbsPoints += 4;

                        var characterStatistics =
                            account.CharacterStatistics.Find(x =>
                                x.CharacterName == player.Character.Name);

                        if (characterStatistics == null)
                        {
                            account.CharacterStatistics.Add(
                                new DiscordAccountClass.CharacterStatisticsClass(player.Character.Name,
                                    player.Status.PlaceAtLeaderBoard));
                        }
                        else
                        {
                            characterStatistics.Plays++;
                            characterStatistics.Wins += player.Status.PlaceAtLeaderBoard == 1 ? 1 : (ulong) 0;
                        }

                        var performanceStatistics =
                            account.PerformanceStatistics.Find(x =>
                                x.Place == player.Status.PlaceAtLeaderBoard);

                        if (performanceStatistics == null)
                            account.PerformanceStatistics.Add(
                                new DiscordAccountClass.PerformanceStatisticsClass(player.Status.PlaceAtLeaderBoard));
                        else
                            performanceStatistics.Times++;

                        if (!player.IsBot())
                            await player.Status.SocketMessageFromBot.Channel.SendMessageAsync("ты кончил.");
                    }

                    game.IsCheckIfReady = false;
                    _global.GamesList.Remove(game);

                    Console.WriteLine("_______________________________________________");
                    continue;
                }

                if (!game.IsCheckIfReady) continue;

                var players = _global.GamesList[i].PlayersList;
                var readyTargetCount = players.Count;
                var readyCount = 0;

                Console.WriteLine(" ");
                foreach (var t in players)
                {
                    await _botsBehavior.HandleBotBehavior(t, game);


                    if (t.Status.IsReady && t.Status.MoveListPage != 3 && game.TimePassed.Elapsed.TotalSeconds > 13)
                        readyCount++;
                    else
                        _logs.Info("NOT READY: = " + t.DiscordUsername);

                    if (t.Status.SocketMessageFromBot == null) continue;
                    //   if (game.TurnLengthInSecond - game.TimePassed.Elapsed.TotalSeconds >= -6)
                    //       await _upd.UpdateMessage(t);
                }

                _logs.Info($"(#{game.GameId}) readyCount = " + readyCount);
                Console.WriteLine(" ");

                if (readyCount != readyTargetCount &&
                    !(game.TimePassed.Elapsed.TotalSeconds >= game.TurnLengthInSecond) ||
                    game.GameStatus != 1) continue;

                //another way of protecting a frame perfect  bug
                if (!game.IsCheckIfReady) continue;

                game.IsCheckIfReady = false;


                try
                {
                    await _round.DeepListMind(game);

                    foreach (var t in players)
                        if (t.Status.SocketMessageFromBot != null)
                        {
                            await _upd.UpdateMessage(t);
                            await _upd.SendMsgAndDeleteIt(t, $"Раунд #{game.RoundNo}", 3);
                        }
                }
                catch (Exception f)
                {
                    await _global.Client.GetUser(181514288278536193).GetOrCreateDMChannelAsync().Result
                        .SendMessageAsync("CheckIfEveryoneIsReady ==>  await _round.DeepListMind(game);\n" +
                                          $"{f.StackTrace}");
                }

                game.IsCheckIfReady = true;
            }
        }
    }
}