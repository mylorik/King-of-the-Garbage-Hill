﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using King_of_the_Garbage_Hill.DiscordFramework;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Game.GameGlobalVariables;
using King_of_the_Garbage_Hill.Helpers;

namespace King_of_the_Garbage_Hill.Game.GameLogic
{
    public class CalculateRound : IServiceSingleton
    {
        private readonly CharacterPassives _characterPassives;
        private readonly InGameGlobal _gameGlobal;
        private readonly Global _global;
        private readonly LoginFromConsole _logs;
        
        
        private readonly SecureRandom _rand;

        public CalculateRound(SecureRandom rand, CharacterPassives characterPassives,
            InGameGlobal gameGlobal, Global global, LoginFromConsole logs)
        {
            _rand = rand;
            _characterPassives = characterPassives;
            _gameGlobal = gameGlobal;
            _global = global;
            _logs = logs;
        }

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public string GetLostContrText(CharacterClass me, CharacterClass target)
        {
            if (me.GetClassStatInt() == 0 && target.GetClassStatInt() == 2)
            {
                return "Вас обманул";
            }

            if (me.GetClassStatInt() == 1 && target.GetClassStatInt() == 0)
            {
                return "Вас пресанул";
            }

            if (me.GetClassStatInt() == 2 && target.GetClassStatInt() == 1)
            {
                return "Вас обогнал";
            }

            return "буль?";
        }

        //пристрій судного дня
        public async Task DeepListMind(GameClass game)
        {
            _logs.Critical("");
            _logs.Info($"calculating game #{game.GameId}, round #{game.RoundNo}");

            var watch = new Stopwatch();
            watch.Start();

            game.TimePassed.Stop();
            game.GameStatus = 2;
            var roundNumber = game.RoundNo + 1;
            if (roundNumber > 10)
            {
                roundNumber = 10;
            }

            /*
            1-4 х1
            5-9 х2
            10  х4
             */
            game.AddGameLogs($"\n__**Раунд #{roundNumber}**__:\n\n");
            game.SetPreviousGameLogs($"\n__**Раунд #{roundNumber}**__:\n\n");


            foreach (var player in game.PlayersList)
            {
                var pointsWined = 0;
                var randomForTooGood = 50;
                var isTooGoodPlayer = false;
                var isTooGoodEnemy = false;
                var isContrLost = 0;
                var isTooGoodLost = 0;
                var playerIamAttacking = game.PlayersList.Find(x => x.Status.PlayerId == player.Status.WhoToAttackThisTurn);


                //if block => no one gets points, and no redundant playerAttacked variable
                if (player.Status.IsBlock || player.Status.IsSkip)
                {
                    _characterPassives.HandleCharacterAfterCalculations(player, game);
                    player.Status.IsWonThisCalculation = Guid.Empty;
                    player.Status.IsLostThisCalculation = Guid.Empty;
                    player.Status.IsFighting = Guid.Empty;
                    continue;
                }


                //This is a bug!
                if (playerIamAttacking == null)
                {
                    _characterPassives.HandleCharacterAfterCalculations(player, game);
                    player.Status.IsWonThisCalculation = Guid.Empty;
                    player.Status.IsLostThisCalculation = Guid.Empty;
                    player.Status.IsFighting = Guid.Empty;
                    await _global.Client.GetUser(181514288278536193).CreateDMChannelAsync().Result
                        .SendMessageAsync("playerIamAttacking == null\n" +
                                          $"");
                    continue;
                }

                //умный
                if (player.Character.GetClassStatInt() == 0 && playerIamAttacking.Character.Justice.GetJusticeNow() == 0)
                {
                    player.Character.AddExtraSkill(player.Status, "Класс: ", 4, true);
                }



                playerIamAttacking.Status.IsFighting = player.Status.PlayerId;
                player.Status.IsFighting = playerIamAttacking.Status.PlayerId;


                await _characterPassives.HandleCharacterWithKnownEnemyBeforeCalculations(player, game);
                await _characterPassives.HandleCharacterWithKnownEnemyBeforeCalculations(playerIamAttacking, game);
                //т.е. он получил урон, какие у него дебаффы на этот счет 
                await _characterPassives.HandleEveryAttackOnHim(playerIamAttacking, player, game);
                //т.е. я его аттакую, какие у меня бонусы на это
                await _characterPassives.HandleEveryAttackFromMe(player, playerIamAttacking, game);


                if (!player.Status.IsAbleToWin) pointsWined = -50;
                if (!playerIamAttacking.Status.IsAbleToWin) pointsWined = 50;


                game.AddPreviousGameLogs(
                    $"{player.DiscordUsername} <:war:561287719838547981> {playerIamAttacking.DiscordUsername}",
                    "");


                //if block => no one gets points
                if (playerIamAttacking.Status.IsBlock && player.Status.IsAbleToWin)
                {
                    // var logMess =  await _characterPassives.HandleBlock(player, playerIamAttacking, game);

                    var logMess = " ⟶ *Бой не состоялся (Блок)...*";

                    game.AddPreviousGameLogs(logMess);

                    //Спарта - никогда не теряет справедливость, атакуя в блок.
                    if (player.Character.Name != "mylorik")
                    {
                        //end Спарта
                        player.Character.Justice.AddJusticeForNextRound(-1);
                        player.Status.AddBonusPoints(-1, "Блок: ");
                    }

                    playerIamAttacking.Character.Justice.AddJusticeForNextRound();

                    _characterPassives.HandleCharacterAfterCalculations(player, game);
                    _characterPassives.HandleCharacterAfterCalculations(playerIamAttacking, game);

                    player.Status.IsWonThisCalculation = Guid.Empty;
                    player.Status.IsLostThisCalculation = Guid.Empty;
                    playerIamAttacking.Status.IsWonThisCalculation = Guid.Empty;
                    playerIamAttacking.Status.IsLostThisCalculation = Guid.Empty;
                    playerIamAttacking.Status.IsFighting = Guid.Empty;
                    player.Status.IsFighting = Guid.Empty;

                    continue;
                }


                if (playerIamAttacking.Status.IsSkip)
                {
                    game.SkipPlayersThisRound++;
                    game.AddPreviousGameLogs(" ⟶ *Бой не состоялся (Скип)...*");

                    _characterPassives.HandleCharacterAfterCalculations(player, game);
                    _characterPassives.HandleCharacterAfterCalculations(playerIamAttacking, game);

                    player.Status.IsWonThisCalculation = Guid.Empty;
                    player.Status.IsLostThisCalculation = Guid.Empty;
                    playerIamAttacking.Status.IsWonThisCalculation = Guid.Empty;
                    playerIamAttacking.Status.IsLostThisCalculation = Guid.Empty;
                    playerIamAttacking.Status.IsFighting = Guid.Empty;
                    player.Status.IsFighting = Guid.Empty;

                    continue;
                }

                //round 1 (contr)


                //быстрый
                if (playerIamAttacking.Character.GetClassStatInt() == 2)
                {
                    playerIamAttacking.Character.AddExtraSkill(playerIamAttacking.Status, "Класс: ", 2, true);
                }

                if (player.Character.GetClassStatInt() == 2)
                {
                    player.Character.AddExtraSkill(player.Status, "Класс: ",2, true);
                }

                //add skill
                if (player.Status.WhoToAttackThisTurn == playerIamAttacking.Status.PlayerId)
                    switch (player.Character.GetCurrentSkillTarget())
                    {
                        case "Интеллект":
                            if (playerIamAttacking.Character.GetClassStatInt() == 0)
                            {
                                player.Character.AddMainSkill(player.Status, "**умного**");
                                var known = player.Status.KnownPlayerClass.Find(x =>
                                    x.EnemyId == playerIamAttacking.Status.PlayerId);
                                if (known != null)
                                    player.Status.KnownPlayerClass.Remove(known);
                                player.Status.KnownPlayerClass.Add(new InGameStatus.KnownPlayerClassClass(playerIamAttacking.Status.PlayerId, "(**Умный** ?) "));
                            }
                            break;
                        case "Сила":
                            if (playerIamAttacking.Character.GetClassStatInt() == 1)
                            {
                                player.Character.AddMainSkill(player.Status, "**сильного**");
                                var known = player.Status.KnownPlayerClass.Find(x =>
                                    x.EnemyId == playerIamAttacking.Status.PlayerId);
                                if (known != null)
                                    player.Status.KnownPlayerClass.Remove(known);
                                player.Status.KnownPlayerClass.Add(new InGameStatus.KnownPlayerClassClass(playerIamAttacking.Status.PlayerId, "(**Сильный** ?) "));

                            }
                            break;
                        case "Скорость":
                            if (playerIamAttacking.Character.GetClassStatInt() == 2)
                            {
                                player.Character.AddMainSkill(player.Status, "**быстрого**");
                                var known = player.Status.KnownPlayerClass.Find(x =>
                                    x.EnemyId == playerIamAttacking.Status.PlayerId);
                                if (known != null)
                                    player.Status.KnownPlayerClass.Remove(known);
                                player.Status.KnownPlayerClass.Add(new InGameStatus.KnownPlayerClassClass(playerIamAttacking.Status.PlayerId, "(**Быстрый** ?) "));
                            }
                            break;
                    }




                //main formula:
                //main formula:
                var me = player.Character;
                var target = playerIamAttacking.Character;

                var scaleMe = (me.GetIntelligence() + me.GetStrength() + me.GetSpeed() + me.GetPsyche()) + me.GetSkill() / 60;
                var scaleTarget = (target.GetIntelligence() + target.GetStrength() + target.GetSpeed() + target.GetPsyche()) + target.GetSkill() / 60;

                var weighingMachine = scaleMe - scaleTarget;

                if (me.GetClassStatInt() == 0 && target.GetClassStatInt() == 2)
                {
                    weighingMachine += 2;
                    isContrLost -= 1;
                }

                if (me.GetClassStatInt() == 1 && target.GetClassStatInt() == 0)
                {
                    weighingMachine += 2;
                    isContrLost -= 1;
                }

                if (me.GetClassStatInt() == 2 && target.GetClassStatInt() == 1)
                {
                    weighingMachine += 2;
                    isContrLost -= 1;
                }


                if (target.GetClassStatInt() == 0 && me.GetClassStatInt() == 2)
                {
                    weighingMachine -= 2;
                    isContrLost += 1;
                }

                if (target.GetClassStatInt() == 1 && me.GetClassStatInt() == 0)
                {
                    weighingMachine -= 2;
                    isContrLost += 1;
                }

                if (target.GetClassStatInt() == 2 && me.GetClassStatInt() == 1)
                {
                    weighingMachine -= 2;
                    isContrLost += 1;
                }

                switch (WhoIsBetter(player, playerIamAttacking))
                {
                    case 1:
                        weighingMachine += 5;
                        break;
                    case 2:
                        weighingMachine -= 5;
                        break;
                }

                var psycheDifference = me.GetPsyche() - target.GetPsyche();
                switch (psycheDifference)
                {
                    case > 0 and <= 3:
                        weighingMachine += 1;
                        break;
                    case >= 4 and <= 5:
                        weighingMachine += 2;
                        break;
                    case >= 6:
                        weighingMachine += 4;
                        break;
                    case < 0 and >= -3:
                        weighingMachine -= 1;
                        break;
                    case >= -5 and <= -4:
                        weighingMachine -= 2;
                        break;
                    case <= -6:
                        weighingMachine -= 4;
                        break;
                }


                switch (weighingMachine)
                {
                    case >= 13:
                        isTooGoodPlayer = true;
                        randomForTooGood = 68;
                        isTooGoodLost = 1;
                        break;
                    case <= -13:
                        isTooGoodEnemy = true;
                        randomForTooGood = 32;
                        isTooGoodLost = -1;
                        break;
                }


                var wtf = scaleMe * (1 + (me.GetSkill() / 1000 - target.GetSkill() / 1000)) - scaleMe;
                weighingMachine += wtf;


                weighingMachine += (player.Character.Justice.GetJusticeNow() -
                                    playerIamAttacking.Character.Justice.GetJusticeNow());


                switch (weighingMachine)
                {
                    case > 0:
                        pointsWined++;
                        isContrLost -= 1;
                        break;
                    case < 0:
                        pointsWined--;
                        isContrLost += 1;
                        break;
                }
                //end round 1


                //round 2 (Justice)
                if (player.Character.Justice.GetJusticeNow() > playerIamAttacking.Character.Justice.GetJusticeNow())
                    pointsWined++;
                if (player.Character.Justice.GetJusticeNow() < playerIamAttacking.Character.Justice.GetJusticeNow())
                    pointsWined--;
                //end round 2

                //round 3 (Random)
                if (pointsWined == 0)
                {
                    var maxRandomNumber = 100;
                    if (player.Character.Justice.GetJusticeNow() > 1 || playerIamAttacking.Character.Justice.GetJusticeNow() > 1)
                        maxRandomNumber -= (player.Character.Justice.GetJusticeNow() - playerIamAttacking.Character.Justice.GetJusticeNow()) * 5;
                    var randomNumber = _rand.Random(1, maxRandomNumber);
                    if (randomNumber <= randomForTooGood) pointsWined++;
                    else pointsWined--;
                }
                //end round 3


                var moral = player.Status.PlaceAtLeaderBoard - playerIamAttacking.Status.PlaceAtLeaderBoard;



                //CheckIfWin to remove Justice
                if (pointsWined >= 1)
                {
                    //сильный
                    if (player.Character.GetClassStatInt() == 1)
                    {
                        player.Character.AddExtraSkill(player.Status, "Класс: ", 5, true);
                    }

                    isContrLost -= 1;
                    game.AddPreviousGameLogs($" ⟶ {player.DiscordUsername}");

                    //еврей
                    var point = _characterPassives.HandleJewPassive(player, game);

                    if (point.Result == 0) player.Status.AddInGamePersonalLogs("Евреи...\n");
                    //end еврей

                    //add regular points
                    player.Status.AddRegularPoints(point.Result, "Победа");


                    player.Status.WonTimes++;
                    player.Character.Justice.IsWonThisRound = true;


                    if (player.Status.PlaceAtLeaderBoard > playerIamAttacking.Status.PlaceAtLeaderBoard && game.RoundNo > 1)
                    {
                        player.Character.AddMoral(player.Status, moral, "Победа: ");
                        playerIamAttacking.Character.AddMoral(playerIamAttacking.Status, moral * -1, "Поражение: ");
                    }
                        

                    playerIamAttacking.Character.Justice.AddJusticeForNextRound();

                    player.Status.IsWonThisCalculation = playerIamAttacking.Status.PlayerId;
                    playerIamAttacking.Status.IsLostThisCalculation = player.Status.PlayerId;
                    playerIamAttacking.Status.WhoToLostEveryRound.Add(new InGameStatus.WhoToLostPreviousRoundClass(player.Status.PlayerId, game.RoundNo, isTooGoodPlayer));
                }
                else
                {

                    //сильный
                    if (playerIamAttacking.Character.GetClassStatInt() == 1)
                    {
                        playerIamAttacking.Character.AddExtraSkill(playerIamAttacking.Status, "Класс: ", 5, true);
                    }

                    if (isTooGoodLost == -1)
                    {
                        player.Status.AddInGamePersonalLogs($"{playerIamAttacking.DiscordUsername} is __TOO GOOD__ for you\n");
                    }

                    isContrLost += 1;
                    //octopus  // playerIamAttacking is octopus
                    var check = _characterPassives.HandleOctopus(playerIamAttacking, player, game);
                    //end octopus

                    if (check)
                    {
                        game.AddPreviousGameLogs($" ⟶ {playerIamAttacking.DiscordUsername}");

                        playerIamAttacking.Status.AddRegularPoints(1, "Победа");

                        player.Status.WonTimes++;
                        playerIamAttacking.Character.Justice.IsWonThisRound = true;

                        if (player.Status.PlaceAtLeaderBoard < playerIamAttacking.Status.PlaceAtLeaderBoard && game.RoundNo > 1)
                        {
                            player.Character.AddMoral(player.Status, moral, "Поражение: ");
                            playerIamAttacking.Character.AddMoral(playerIamAttacking.Status, moral * -1, "Победа: ");
                        }

                        if (playerIamAttacking.Character.Name == "Толя" && playerIamAttacking.Status.IsBlock)
                            playerIamAttacking.Character.Justice.IsWonThisRound = false;

                        player.Character.Justice.AddJusticeForNextRound();

                        playerIamAttacking.Status.IsWonThisCalculation = player.Status.PlayerId;
                        player.Status.IsLostThisCalculation = playerIamAttacking.Status.PlayerId;
                        player.Status.WhoToLostEveryRound.Add(new InGameStatus.WhoToLostPreviousRoundClass(playerIamAttacking.Status.PlayerId, game.RoundNo, isTooGoodEnemy));
                    }
                }


                //т.е. он получил урон, какие у него дебаффы на этот счет 
                await _characterPassives.HandleEveryAttackOnHimAfterCalculations(playerIamAttacking, player, game);

                //т.е. я его аттакую, какие у меня бонусы на это
                await _characterPassives.HandleEveryAttackFromMeAfterCalculations(player, playerIamAttacking, game);

                //TODO: merge top 2 methods and 2 below... they are the same... or no?


                switch (isContrLost)
                {
                    case 3:
                        player.Status.AddInGamePersonalLogs($"Поражение: {GetLostContrText(target, me)} {playerIamAttacking.DiscordUsername}\n");
                        break;
                    case -3:
                        playerIamAttacking.Status.AddInGamePersonalLogs($"Поражение: {GetLostContrText(me, target)} {player.DiscordUsername}\n");
                        break;
                }

                _characterPassives.HandleCharacterAfterCalculations(player, game);

                _characterPassives.HandleCharacterAfterCalculations(playerIamAttacking, game);
                await _characterPassives.HandleEventsAfterEveryBattle(game); //used only for shark...

                player.Status.IsWonThisCalculation = Guid.Empty;
                player.Status.IsLostThisCalculation = Guid.Empty;
                playerIamAttacking.Status.IsWonThisCalculation = Guid.Empty;
                playerIamAttacking.Status.IsLostThisCalculation = Guid.Empty;
                playerIamAttacking.Status.IsFighting = Guid.Empty;
                player.Status.IsFighting = Guid.Empty;
            }


            await _characterPassives.HandleEndOfRound(game);
            if (game.RoundNo % 2 == 0)
                game.TurnLengthInSecond += 10;
            else
                game.TurnLengthInSecond -= 10;

            if (game.RoundNo == 1) game.TurnLengthInSecond -= 75;

            foreach (var player in game.PlayersList)
            {
                player.Status.IsBlock = false;
                player.Status.IsAbleToWin = true;
                player.Status.IsSkip = false;
                player.Status.IsAbleToTurn = true;
                player.Status.IsReady = false;
                player.Status.WhoToAttackThisTurn = Guid.Empty;
                player.Status.CombineRoundScoreAndGameScore(game, _gameGlobal, game.Phrases);
                player.Status.ClearInGamePersonalLogs();
                player.Status.InGamePersonalLogsAll += "|||";

                player.Status.MoveListPage = 1;

                if (player.Character.Justice.IsWonThisRound) player.Character.Justice.SetJusticeNow(player.Status, 0, "Новый Раунд:", false);

                player.Character.Justice.IsWonThisRound = false;
                player.Character.Justice.AddJusticeNow(player.Character.Justice.GetJusticeForNextRound());
                player.Character.Justice.SetJusticeForNextRound(0);
            }

            game.SkipPlayersThisRound = 0;
            game.RoundNo++;
            game.GameStatus = 1;


            await _characterPassives.HandleNextRound(game);

            //Handle Moral
            foreach (var p in game.PlayersList)
            {
                p.Status.AddBonusPoints(p.Character.GetBonusPointsFromMoral(), "Мораль: ");
                p.Character.SetBonusPointsFromMoral(0);
            }
            //end Moral

            game.PlayersList = game.PlayersList.OrderByDescending(x => x.Status.GetScore()).ToList();


            //HardKitty unique
            if (game.PlayersList.Any(x => x.Character.Name == "HardKitty"))
            {
                var tempHard = game.PlayersList.Find(x => x.Character.Name == "HardKitty");
                var hardIndex = game.PlayersList.IndexOf(tempHard);

                for (var i = hardIndex; i < game.PlayersList.Count - 1; i++)
                    game.PlayersList[i] = game.PlayersList[i + 1];

                game.PlayersList[game.PlayersList.Count - 1] = tempHard;
            }
            //end //HardKitty unique


            //Tigr Unique
            if (game.PlayersList.Any(x => x.Character.Name == "Тигр"))
            {
                var tigrTemp = game.PlayersList.Find(x => x.Character.Name == "Тигр");

                var tigr = _gameGlobal.TigrTop.Find(x =>
                    x.GameId == game.GameId && x.PlayerId == tigrTemp.Status.PlayerId);

                if (tigr is {TimeCount: > 0})
                {
                    var tigrIndex = game.PlayersList.IndexOf(tigrTemp);

                    game.PlayersList[tigrIndex] = game.PlayersList[0];
                    game.PlayersList[0] = tigrTemp;
                    tigr.TimeCount--;
                    // await game.Phrases.TigrTop.SendLog(tigrTemp);
                }
            }
            //end Tigr Unique

            //sort
            for (var i = 0; i < game.PlayersList.Count; i++)
            {
                if (game.RoundNo == 3 || game.RoundNo == 5 || game.RoundNo == 7 || game.RoundNo == 9)
                    game.PlayersList[i].Status.MoveListPage = 3;
                game.PlayersList[i].Status.PlaceAtLeaderBoard = i + 1;
                game.PlayersList[i].Character.RollCurrentSkillTarget();
            }
            //end sorting

            SortGameLogs(game);
            _logs.Info("Start HandleNextRoundAfterSorting");
            _characterPassives.HandleNextRoundAfterSorting(game);
            _logs.Info("Finished HandleNextRoundAfterSorting");

            game.TimePassed.Reset();
            game.TimePassed.Start();
            _logs.Info(
                $"Finished calculating game #{game.GameId} (round# {game.RoundNo - 1}). || {watch.Elapsed.TotalSeconds}s");
           _logs.Critical("");
            watch.Stop();
            await Task.CompletedTask;
        }

        /*
I: 4 | St: 8 | Sp: 9 | Ps: 3
I: 1 | St: 9 | Sp: 9 | Ps: 1
1-1
         */

        public int WhoIsBetter(GamePlayerBridgeClass meClass, GamePlayerBridgeClass targetClass)
        {
            var me = meClass.Character;
            var target = targetClass.Character;

            int intel = 0, speed = 0, str = 0;

            if (me.GetIntelligence() - target.GetIntelligence() > 0)
                intel = 1;
            if (me.GetIntelligence() - target.GetIntelligence() < 0)
                intel = -1;

            if (me.GetSpeed() - target.GetSpeed() > 0)
                speed = 1;
            if (me.GetSpeed() - target.GetSpeed() < 0)
                speed = -1;

            if (me.GetStrength() - target.GetStrength() > 0)
                str = 1;
            if (me.GetStrength() - target.GetStrength() < 0)
                str = -1;


            if (intel + speed + str >= 2)
                return 1;
            if (intel + speed + str <= -2)
                return 2;

            if (intel == 1 && str != -1)
                return 1;
            if (str == 1 && speed != -1)
                return 1;
            if (speed == 1 && intel != -1)
                return 1;

            return 2;
        }

        public void SortGameLogs(GameClass game)
        {
            var sortedGameLogs = "";
            var extraGameLogs = "\n";
            var logsSplit = game.GetPreviousGameLogs().Split("\n").ToList();
            logsSplit.RemoveAll(x => x.Length <= 2);
            sortedGameLogs += $"{logsSplit[0]}\n";
            logsSplit.RemoveAt(0);

            for (var i = 0; i < logsSplit.Count; i++)
            {
                if (logsSplit[i].Contains(":war:")) continue;
                extraGameLogs += $"{logsSplit[i]}\n";
                logsSplit.RemoveAt(i);
                i--;
            }


            foreach (var player in game.PlayersList)
                for (var i = 0; i < logsSplit.Count; i++)
                    if (logsSplit[i].Contains($"{player.DiscordUsername}"))
                    {
                        var fightLine = logsSplit[i];

                        var fightLineSplit = fightLine.Split("⟶");

                        var fightLineSplitSplit = fightLineSplit[0].Split("<:war:561287719838547981>");

                        fightLine = fightLineSplitSplit[0].Contains($"{player.DiscordUsername}")
                            ? $"{fightLineSplitSplit[0]} <:war:561287719838547981> {fightLineSplitSplit[1]}"
                            : $"{fightLineSplitSplit[1]} <:war:561287719838547981> {fightLineSplitSplit[0]}";


                        fightLine += $" ⟶ {fightLineSplit[1]}";

                        sortedGameLogs += $"{fightLine}\n";
                        logsSplit.RemoveAt(i);
                        i--;
                    }

            sortedGameLogs += extraGameLogs;
            game.SetPreviousGameLogs(sortedGameLogs);
        }
    }
}
