﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using King_of_the_Garbage_Hill.DiscordFramework;
using King_of_the_Garbage_Hill.Game.Classes;

namespace King_of_the_Garbage_Hill.Game.GameLogic;

public class DoomsdayMachine : IServiceSingleton
{
    private readonly CharacterPassives _characterPassives;
    private readonly LoginFromConsole _logs;
    private readonly CalculateRounds _calculateRounds;

    public DoomsdayMachine(CharacterPassives characterPassives, LoginFromConsole logs, CalculateRounds calculateRounds)
    {
        _characterPassives = characterPassives;
        _logs = logs;
        _calculateRounds = calculateRounds;
    }

    public async Task InitializeAsync()
    {
        await Task.CompletedTask;
    }

    /*
Intelligence => Speed
Strength => Intelligence
Speed => Strength
*/
    public string GetLostContrText(GamePlayerBridgeClass me, GamePlayerBridgeClass target)
    {
        if (me.GameCharacter.GetSkillClass() == "Интеллект" && target.GameCharacter.GetSkillClass() == "Скорость")
        {
            target.Status.KnownPlayerClass.Add(new InGameStatus.KnownPlayerClassClass(me.GetPlayerId(), "(**Умный** ?) "));

            return "вас обманул";
        }

        if (me.GameCharacter.GetSkillClass() == "Сила" && target.GameCharacter.GetSkillClass() == "Интеллект")
        {
            target.Status.KnownPlayerClass.Add(new InGameStatus.KnownPlayerClassClass(me.GetPlayerId(), "(**Сильный** ?) "));
            return "вас пресанул";
        }

        if (me.GameCharacter.GetSkillClass() == "Скорость" && target.GameCharacter.GetSkillClass() == "Сила")
        {
            target.Status.KnownPlayerClass.Add(new InGameStatus.KnownPlayerClassClass(me.GetPlayerId(), "(**Быстрый** ?) "));
            return "вас обогнал";
        }

        return "буль?";
    }


    public void ResetFight(GameClass game, GamePlayerBridgeClass me, GamePlayerBridgeClass target = null)
    {
        var players = new List<GamePlayerBridgeClass> { me, target };
        foreach (var player in players.Where(p => p != null))
        {

            if (player.Status.IsWonThisCalculation != Guid.Empty)
            {
                player.FightCharacter.AddWinStreak();
                player.Passives.WeedwickWeed++;
            }

            if (player.Status.IsLostThisCalculation != Guid.Empty)
            {
                player.FightCharacter.SetWinStreak();
            }

            if (player.Status.IsLostThisCalculation != Guid.Empty && player.Passives.IsExploitable)
            {
               game.TotalExploit++;
            }


            //OneFight Mechanics, use GameCharacter only
            if (player.Status.IsIntelligenceForOneFight)
            {
                player.Status.IsIntelligenceForOneFight = false;
                player.GameCharacter.ResetIntelligenceForOneFight();
            }

            if (player.Status.IsStrengthForOneFight)
            {
                player.Status.IsStrengthForOneFight = false;
                player.GameCharacter.ResetStrengthForOneFight();
            }

            if (player.Status.IsSpeedForOneFight)
            {
                player.Status.IsSpeedForOneFight = false;
                player.GameCharacter.ResetSpeedForOneFight();
            }

            if (player.Status.IsPsycheForOneFight)
            {
                player.Status.IsPsycheForOneFight = false;
                player.GameCharacter.ResetPsycheForOneFight();
            }

            if (player.Status.IsJusticeForOneFight )
            {
                player.Status.IsJusticeForOneFight = false;
                player.GameCharacter.Justice.ResetJusticeForOneFight();
            }

            if (player.Status.IsSkillForOneFight)
            {
                player.Status.IsSkillForOneFight = false;
                player.GameCharacter.ResetSkillForOneFight();
            }
            //end OneFight Mechanics

            player.Status.IsWonThisCalculation = Guid.Empty;
            player.Status.IsLostThisCalculation = Guid.Empty;
            player.Status.IsFighting = Guid.Empty;
            player.Status.IsTargetSkipped = Guid.Empty;
            player.Status.IsTargetBlocked = Guid.Empty;
            player.Status.IsAbleToWin = true;
        }
    }

    public void DeepCopyFightCharactersToGameCharacter(GameClass game)
    {
        foreach (var player in game.PlayersList)
        {
            player.GameCharacter = player.FightCharacter.DeepCopy();
            player.Status.GameCharacter = player.GameCharacter;
        }
    }

    public void DeepCopyGameCharacterToFightCharacter(GameClass game)
    {
        foreach (var player in game.PlayersList)
        {
            player.FightCharacter = player.GameCharacter.DeepCopy();
        }
    }

    public void HandleEventsBeforeCalculation(GameClass game)
    {
        foreach (var player in game.PlayersList)
        {
            foreach (var passive in player.GameCharacter.Passive)
            {
                switch (passive.PassiveName)
                {
                        case "PointFunnel":
                            if (player.Status.WhoToAttackThisTurn.Count > 0)
                            {
                                foreach (var targetId in player.Status.WhoToAttackThisTurn)
                                {
                                    var target = game.PlayersList.Find(x => x.GetPlayerId() == targetId);
                                    target.Passives.PointFunneledTo = player.GetPlayerId();
                                }
                            }
                            break;
            } 
            }
        }

    }




    //пристрій судного дня
    public async Task CalculateAllFights(GameClass game)    
    {
        var watch = new Stopwatch();
        watch.Start();

        game.TimePassed.Stop();
        var roundNumber = game.RoundNo + 1;
        if (roundNumber > 10) roundNumber = 10;

        //Возвращение из мертвых
        if (game.IsKratosEvent)
            roundNumber = game.RoundNo + 1;
        //end Возвращение из мертвых


        //Handle Moral
        foreach (var p in game.PlayersList)
        {
            p.Status.AddBonusPoints(p.GameCharacter.GetBonusPointsFromMoral(), "Мораль");
            p.GameCharacter.SetBonusPointsFromMoral(0);
        }
        //end Moral

        HandleEventsBeforeCalculation(game);

        /*
        1-4 х1
        5-9 х2
        10  х4
         */

        game.SetGlobalLogs($"\n__**Раунд #{roundNumber}**__:\n\n");

        //FIGHT, user only GameCharacter to READ and FightCharacter to WRITE
        DeepCopyGameCharacterToFightCharacter(game);

        











        foreach (var player in game.PlayersList)
        {

            player.Status.AddFightingData($"\n\n**Logs for round #{game.RoundNo}:**");
            //if block => no one gets points, and no redundant playerAttacked variable
            if (player.Status.IsBlock || player.Status.IsSkip)
            {
                player.Status.AddFightingData($"IsBlock: {player.Status.IsBlock}");
                player.Status.AddFightingData($"IsSkip: {player.Status.IsSkip}");
                //fight Reset
                await _characterPassives.HandleCharacterAfterFight(player, game, true, false);
                ResetFight(game, player);
                continue;
            }

            foreach (var playerIamAttacking in player.Status.WhoToAttackThisTurn.Select(t => game.PlayersList.Find(x => x.GetPlayerId() == t)))
            {
                player.Status.AddFightingData("\n");
                playerIamAttacking.Status.AddFightingData("\n");
                player.Status.AddFightingData($"**you VS {playerIamAttacking.GameCharacter.Name} ({playerIamAttacking.DiscordUsername})**");
                playerIamAttacking.Status.AddFightingData($"**{player.GameCharacter.Name} ({player.DiscordUsername}) VS you**");

                
                playerIamAttacking.Status.IsFighting = player.GetPlayerId();
                player.Status.IsFighting = playerIamAttacking.GetPlayerId();


                _characterPassives.HandleDefenseBeforeFight(playerIamAttacking, player, game);
                _characterPassives.HandleAttackBeforeFight(player, playerIamAttacking, game);


                //умный
                if (player.GameCharacter.GetSkillClass() == "Интеллект" && playerIamAttacking.GameCharacter.Justice.GetRealJusticeNow() == 0)
                {
                    player.FightCharacter.AddExtraSkill(6 * player.GameCharacter.GetClassSkillMultiplier(), "Класс");
                }




                game.AddGlobalLogs($"{player.DiscordUsername} <:war:561287719838547981> {playerIamAttacking.DiscordUsername}", "");

                //add skill
                if (player.GameCharacter.GetCurrentSkillClassTarget() == playerIamAttacking.GameCharacter.GetSkillClass())
                {
                    string text1;
                    string text2;

                    if (playerIamAttacking.GameCharacter.GetSkillClass() == "Интеллект")
                    {
                        text1 = "**умного**";
                        text2 = "(**Умный** ?) ";
                    }
                    else if (playerIamAttacking.GameCharacter.GetSkillClass() == "Сила")
                    {
                        text1 = "**сильного**";
                        text2 = "(**Сильный** ?) ";
                    }
                    else if (playerIamAttacking.GameCharacter.GetSkillClass() == "Скорость")
                    {
                        text1 = "**быстрого**";
                        text2 = "(**Быстрый** ?) ";
                    }
                    else
                    {
                        text1 = "**буля**";
                        text2 = "(**БУЛЬ** ?!) ";
                    }

                    player.FightCharacter.AddMainSkill(text1);

                    var known = player.Status.KnownPlayerClass.Find(x => x.EnemyId == playerIamAttacking.GetPlayerId());
                    if (known != null)
                        player.Status.KnownPlayerClass.Remove(known);
                    player.Status.KnownPlayerClass.Add(new InGameStatus.KnownPlayerClassClass(playerIamAttacking.GetPlayerId(), text2));
                }


                //check skill text
                switch (player.GameCharacter.GetCurrentSkillClassTarget())
                {
                    case "Интеллект":
                        if (playerIamAttacking.GameCharacter.GetSkillClass() != "Интеллект")
                        {
                            var knownEnemy =
                                player.Status.KnownPlayerClass.Find(
                                    x => x.EnemyId == playerIamAttacking.GetPlayerId());
                            if (knownEnemy != null)
                                if (knownEnemy.Text.Contains("Умный"))
                                    player.Status.KnownPlayerClass.Remove(knownEnemy);
                        }

                        break;
                    case "Сила":
                        if (playerIamAttacking.GameCharacter.GetSkillClass() != "Сила")
                        {
                            var knownEnemy =
                                player.Status.KnownPlayerClass.Find(
                                    x => x.EnemyId == playerIamAttacking.GetPlayerId());
                            if (knownEnemy != null)
                                if (knownEnemy.Text.Contains("Сильный"))
                                    player.Status.KnownPlayerClass.Remove(knownEnemy);
                        }

                        break;
                    case "Скорость":
                        if (playerIamAttacking.GameCharacter.GetSkillClass() != "Скорость")
                        {
                            var knownEnemy =
                                player.Status.KnownPlayerClass.Find(
                                    x => x.EnemyId == playerIamAttacking.GetPlayerId());
                            if (knownEnemy != null)
                                if (knownEnemy.Text.Contains("Быстрый"))
                                    player.Status.KnownPlayerClass.Remove(knownEnemy);
                        }

                        break;
                    case "Буль":
                        if (playerIamAttacking.GameCharacter.GetSkillClass() != "Буль")
                        {
                            var knownEnemy =
                                player.Status.KnownPlayerClass.Find(
                                    x => x.EnemyId == playerIamAttacking.GetPlayerId());
                            if (knownEnemy != null)
                                if (knownEnemy.Text.Contains("БУЛЬ"))
                                    player.Status.KnownPlayerClass.Remove(knownEnemy);
                        }

                        break;
                }


                player.Status.AddFightingData($"IsArmorBreak: {player.Status.IsArmorBreak}");
                player.Status.AddFightingData($"IsBlockEnemy: {playerIamAttacking.Status.IsBlock}");
                playerIamAttacking.Status.AddFightingData($"IsBlock: {playerIamAttacking.Status.IsBlock}");
                playerIamAttacking.Status.AddFightingData($"IsArmorBreakEnemy: {player.Status.IsArmorBreak}");

                //if block => no one gets points
                if (playerIamAttacking.Status.IsBlock && !player.Status.IsArmorBreak)
                {
                    player.Status.IsTargetBlocked = playerIamAttacking.GetPlayerId();
                    // var logMess =  await _characterPassives.HandleBlock(player, playerIamAttacking, game);

                    var logMess = " ⟶ *Бой не состоялся...*";
                    if (game.PlayersList.Any(x => x.PlayerType == 1))
                        logMess = " ⟶ *Бой не состоялся (Блок)...*";
                    game.AddGlobalLogs(logMess);


                    player.Status.AddBonusPoints(-1, "Блок");

                    playerIamAttacking.GameCharacter.Justice.AddJusticeForNextRoundFromFight();

                    //fight Reset
                    await _characterPassives.HandleCharacterAfterFight(player, game, true, false);
                    await _characterPassives.HandleCharacterAfterFight(playerIamAttacking, game, false, true);
                    _characterPassives.HandleDefenseAfterBlockOrFight(playerIamAttacking, player, game);
                    _characterPassives.HandleDefenseAfterBlockOrFightOrSkip(playerIamAttacking, player, game);

                    ResetFight(game, player, playerIamAttacking);

                    continue;
                }


                player.Status.AddFightingData($"IsSkipBreak: {player.Status.IsSkipBreak}");
                player.Status.AddFightingData($"IsSkipEnemy: {playerIamAttacking.Status.IsSkip}");
                playerIamAttacking.Status.AddFightingData($"IsSkip: {playerIamAttacking.Status.IsSkip}");
                playerIamAttacking.Status.AddFightingData($"IsSkipBreakEnemy: {player.Status.IsSkipBreak}");

                // if skip => something
                if (playerIamAttacking.Status.IsSkip && !player.Status.IsSkipBreak)
                {
                    player.Status.IsTargetSkipped = playerIamAttacking.GetPlayerId();
                    game.SkipPlayersThisRound++;

                    var logMess = " ⟶ *Бой не состоялся...*";
                    if (game.PlayersList.Any(x => x.PlayerType == 1))
                        logMess = " ⟶ *Бой не состоялся (Скип)...*";
                    game.AddGlobalLogs(logMess);

                    //fight Reset
                    await _characterPassives.HandleCharacterAfterFight(player, game, true, false);
                    await _characterPassives.HandleCharacterAfterFight(playerIamAttacking, game, false, true);
                    _characterPassives.HandleDefenseAfterBlockOrFightOrSkip(playerIamAttacking, player, game);

                    ResetFight(game, player, playerIamAttacking);

                    continue;
                }

                //round 1 (contr)


                //быстрый
                if (playerIamAttacking.GameCharacter.GetSkillClass() == "Скорость")
                    playerIamAttacking.FightCharacter.AddExtraSkill(2 * playerIamAttacking.GameCharacter.GetClassSkillMultiplier(), "Класс");

                if (player.GameCharacter.GetSkillClass() == "Скорость")
                    player.FightCharacter.AddExtraSkill(2 * player.GameCharacter.GetClassSkillMultiplier(), "Класс");


                //main formula:

                //round 1 (Stats)
                var (isTooGoodMe, isTooGoodEnemy, isTooStronkMe, isTooStronkEnemy, isStatsBetterMe, isStatsBettterEnemy, pointsWined, isContrLost, randomForPoint, weighingMachine, contrMultiplier, skillMultiplierMe, skillMultiplierTarget) = _calculateRounds.CalculateStep1(player, playerIamAttacking, true);
                //end round 1


                if (!player.Status.IsAbleToWin)
                {
                    pointsWined += -50;
                }

                if (!playerIamAttacking.Status.IsAbleToWin)
                {
                    pointsWined += 50;

                }

                player.Status.AddFightingData($"IsAbleToWin: {player.Status.IsAbleToWin}");
                player.Status.AddFightingData($"IsAbleToWinEnemy: {playerIamAttacking.Status.IsAbleToWin}");
                playerIamAttacking.Status.AddFightingData($"IsAbleToWin: {playerIamAttacking.Status.IsAbleToWin}");
                playerIamAttacking.Status.AddFightingData($"IsAbleToWinEnemy: {player.Status.IsAbleToWin}");

                
                //round 2 (Justice)
                pointsWined += _calculateRounds.CalculateStep2(player, playerIamAttacking, true);
                //end round 2


                //round 3 (Random)
                if (pointsWined == 0)
                {
                    pointsWined += _calculateRounds.CalculateStep3(player, playerIamAttacking, randomForPoint, contrMultiplier, true);
                }
                //end round 3


                var moral = player.Status.GetPlaceAtLeaderBoard() - playerIamAttacking.Status.GetPlaceAtLeaderBoard();


                //octopus  // playerIamAttacking is octopus
                if (pointsWined <= 0) 
                    pointsWined = await _characterPassives.HandleOctopus(playerIamAttacking, player, game);
                //end octopus


                //team
                var teamMate = false;
                if (game.Teams.Count > 0)
                {
                    var playerTeam = game.Teams.Find(x => x.TeamPlayers.Contains(player.Status.PlayerId)).TeamId;
                    var playerIamAttackingTeam = game.Teams.Find(x => x.TeamPlayers.Contains(playerIamAttacking.Status.PlayerId)).TeamId;
                    if (playerTeam == playerIamAttackingTeam)
                    {
                        teamMate = true;
                    }
                }

                //CheckIfWin to remove Justice
                if (pointsWined >= 1)
                {
                    var point = 1;
                    //сильный
                    if (player.GameCharacter.GetSkillClass() == "Сила")
                        player.FightCharacter.AddExtraSkill(4 * player.GameCharacter.GetClassSkillMultiplier(), "Класс");

                    isContrLost -= 1;
                    game.AddGlobalLogs($" ⟶ {player.DiscordUsername}");

                    //еврей
                    if (!teamMate)
                        point = await _characterPassives.HandleJews(player, playerIamAttacking, game);
                    if (point == 0) player.Status.AddInGamePersonalLogs("Евреи...\n");
                    //end еврей


                    //add regular points
                    if (!teamMate)
                        if (player.GameCharacter.Passive.Any(x => x.PassiveName == "Никому не нужен"))
                        {
                            player.Status.AddWinPoints(game, player, point * -1, "Победа");
                        }
                        else
                        {
                            player.Status.AddWinPoints(game, player, point, "Победа");
                        }


                    if (!teamMate)
                        player.GameCharacter.Justice.IsWonThisRound = true;

                    // -5 = 1 - 6
                    if (player.Status.GetPlaceAtLeaderBoard() > playerIamAttacking.Status.GetPlaceAtLeaderBoard() && game.RoundNo > 1)
                    {
                        if (!teamMate)
                        {
                            player.FightCharacter.AddMoral(moral, "Победа", isFightMoral:true);
                            playerIamAttacking.FightCharacter.AddMoral(moral * -1, "Поражение", isFightMoral: true);

                            player.Status.AddFightingData($"moral: {moral} ({player.Status.GetPlaceAtLeaderBoard()} - {playerIamAttacking.Status.GetPlaceAtLeaderBoard()})");
                            playerIamAttacking.Status.AddFightingData($"moral: {moral * -1} ({player.Status.GetPlaceAtLeaderBoard()} - {playerIamAttacking.Status.GetPlaceAtLeaderBoard()})");
                        }
                    }

                    if (!teamMate)
                        playerIamAttacking.GameCharacter.Justice.AddJusticeForNextRoundFromFight();

                    player.Status.IsWonThisCalculation = playerIamAttacking.GetPlayerId();
                    playerIamAttacking.Status.IsLostThisCalculation = player.GetPlayerId();
                    playerIamAttacking.Status.WhoToLostEveryRound.Add(new InGameStatus.WhoToLostPreviousRoundClass(player.GetPlayerId(), game.RoundNo, isTooGoodMe, isStatsBetterMe, isTooGoodEnemy, isStatsBettterEnemy, player.GetPlayerId()));

                    //Quality
                    var range = player.GameCharacter.GetSpeedQualityResistInt();
                    range -= playerIamAttacking.GameCharacter.GetSpeedQualityKiteBonus();

                    var placeDiff = player.Status.GetPlaceAtLeaderBoard() - playerIamAttacking.Status.GetPlaceAtLeaderBoard();
                    if (placeDiff < 0)
                        placeDiff *= -1;


                    if (placeDiff <= range)
                    {
                        playerIamAttacking.FightCharacter.LowerQualityResist(playerIamAttacking, game, player);
                    }

                    //end Quality
                }
                else
                {
                    //сильный
                    if (playerIamAttacking.GameCharacter.GetSkillClass() == "Сила")
                        playerIamAttacking.FightCharacter.AddExtraSkill(4 * player.GameCharacter.GetClassSkillMultiplier(), "Класс");

                    if (isTooGoodEnemy && !isTooStronkEnemy)
                        player.Status.AddInGamePersonalLogs($"{playerIamAttacking.DiscordUsername} is __TOO GOOD__ for you\n");
                    if (isTooStronkEnemy)
                        player.Status.AddInGamePersonalLogs($"{playerIamAttacking.DiscordUsername} is __TOO STONK__ for you\n");

                    isContrLost += 1;


                    game.AddGlobalLogs($" ⟶ {playerIamAttacking.DiscordUsername}");

                    if (!teamMate)
                        playerIamAttacking.Status.AddWinPoints(game, playerIamAttacking, 1, "Победа");



                    if (!teamMate)
                        playerIamAttacking.GameCharacter.Justice.IsWonThisRound = true;

                    if (player.Status.GetPlaceAtLeaderBoard() < playerIamAttacking.Status.GetPlaceAtLeaderBoard() && game.RoundNo > 1)
                    {
                        if (!teamMate)
                        {
                            player.FightCharacter.AddMoral(moral, "Поражение", isFightMoral: true);
                            playerIamAttacking.FightCharacter.AddMoral(moral * -1, "Победа", isFightMoral: true);

                            player.Status.AddFightingData($"moral: {moral} ({player.Status.GetPlaceAtLeaderBoard()} - {playerIamAttacking.Status.GetPlaceAtLeaderBoard()})");
                            playerIamAttacking.Status.AddFightingData($"moral: {moral * -1} ({player.Status.GetPlaceAtLeaderBoard()} - {playerIamAttacking.Status.GetPlaceAtLeaderBoard()})");
                        }
                    }

                    if (playerIamAttacking.GameCharacter.Passive.Any(x => x.PassiveName == "Раммус мейн") && playerIamAttacking.Status.IsBlock)
                        if (!teamMate)
                            playerIamAttacking.GameCharacter.Justice.IsWonThisRound = false;

                    if (!teamMate)
                        player.GameCharacter.Justice.AddJusticeForNextRoundFromFight();

                    playerIamAttacking.Status.IsWonThisCalculation = player.GetPlayerId();
                    player.Status.IsLostThisCalculation = playerIamAttacking.GetPlayerId();
                    player.Status.WhoToLostEveryRound.Add(new InGameStatus.WhoToLostPreviousRoundClass(playerIamAttacking.GetPlayerId(), game.RoundNo, isTooGoodEnemy, isStatsBettterEnemy, isTooGoodMe, isStatsBetterMe, player.GetPlayerId()));
                }

                
                switch (isContrLost)
                {
                    case 3:
                        player.Status.AddInGamePersonalLogs($"Поражение: {playerIamAttacking.DiscordUsername} {GetLostContrText(playerIamAttacking, player)}\n");
                        break;
                    case -3:
                        playerIamAttacking.Status.AddInGamePersonalLogs($"Поражение: {player.DiscordUsername} {GetLostContrText(player, playerIamAttacking)}\n");
                        break;
                }

                //т.е. он получил урон, какие у него дебаффы на этот счет 
                _characterPassives.HandleDefenseAfterFight(playerIamAttacking, player, game);
                _characterPassives.HandleDefenseAfterBlockOrFight(playerIamAttacking, player, game);
                _characterPassives.HandleDefenseAfterBlockOrFightOrSkip(playerIamAttacking, player, game);

                //т.е. я его аттакую, какие у меня бонусы на это
                _characterPassives.HandleAttackAfterFight(player, playerIamAttacking, game);

                //fight Reset
                await _characterPassives.HandleCharacterAfterFight(player, game, true, false);
                await _characterPassives.HandleCharacterAfterFight(playerIamAttacking, game, false, true);
                
                _characterPassives.HandleShark(game); //used only for shark...

                ResetFight(game, player, playerIamAttacking);
            }
        }














        //AFTER Fight, use only GameCharacter.
        DeepCopyFightCharactersToGameCharacter(game);

        await _characterPassives.HandleEndOfRound(game);

        foreach (var player in game.PlayersList)
        {
            player.Status.TimesUpdated = 0;
            player.Status.IsAutoMove = false;
            player.Status.IsBlock = false;
            player.Status.IsSkipBreak = false;
            player.Status.IsArmorBreak = false;
            player.Status.IsAbleToWin = true;
            player.Status.IsSkip = false;
            player.Status.IsReady = false;
            player.Status.WhoToAttackThisTurn = new List<Guid>();
            player.Status.MoveListPage = 1;
            player.Status.IsAbleToChangeMind = true;
            player.Status.RoundNumber = game.RoundNo+1;

            player.GameCharacter.SetSpeedResist();
            player.GameCharacter.NormalizeMoral();
            player.GameCharacter.Justice.HandleEndOfRoundJustice();

            player.Status.CombineRoundScoreAndGameScore(game);
            player.Status.ClearInGamePersonalLogs();
            player.Status.InGamePersonalLogsAll += "|||";

            player.Passives.PointFunneledTo = Guid.Empty;
        }

        //Возвращение из мертвых
        //game.PlayersList = game.PlayersList.Where(x => !x.Passives.KratosIsDead).ToList();

        if (game.PlayersList.Count(x => x.Passives.KratosIsDead && x.GameCharacter.Name != "Кратос") == 5)
        {
            game.IsKratosEvent = false;
            game.AddGlobalLogs("Все боги были убиты, открылась коробка Пандоры, стихийные бедствия уничтожили всё живое...");
            game.PlayersList[0].Status.AddInGamePersonalLogs("By the gods, what have I become?\n");
        }
        //end Возвращение из мертвых

        game.SkipPlayersThisRound = 0;
        game.RoundNo++;

        if (game.GameMode == "aram" && game.RoundNo == 2)
        {
            game.TurnLengthInSecond = 300;
        }


        await _characterPassives.HandleNextRound(game);



        game.PlayersList = game.PlayersList.OrderByDescending(x => x.Status.GetScore()).ToList();


        //Тигр топ, а ты холоп
        foreach (var player in game.PlayersList.Where(x => x.GameCharacter.Passive.Any(y => y.PassiveName == "Тигр топ, а ты холоп")).ToList())
        {
            var tigr = player.Passives.TigrTop;

            if (tigr is { TimeCount: > 0 })
            {
                var tigrIndex = game.PlayersList.IndexOf(player);

                game.PlayersList[tigrIndex] = game.PlayersList.First();
                game.PlayersList[0] = player;
                tigr.TimeCount--;
                // game.Phrases.TigrTop.SendLog(tigrTemp);
            }
        }
        //end Тигр топ, а ты холоп


        //Никому не нужен
        foreach (var player in game.PlayersList.Where(x => x.GameCharacter.Passive.Any(y => y.PassiveName == "Никому не нужен")).ToList())
        {
            var hardIndex = game.PlayersList.IndexOf(player);

            for (var k = hardIndex; k < game.PlayersList.Count - 1; k++)
                game.PlayersList[k] = game.PlayersList[k + 1];

            game.PlayersList[^1] = player;
        }
        //end Никому не нужен


        //sort
        for (var i = 0; i < game.PlayersList.Count; i++)
        {
            if (game.RoundNo is 3 or 5 or 7 or 9)
            {
                game.PlayersList[i].Status.LvlUpPoints++;
                game.PlayersList[i].Status.MoveListPage = 3;
            }
            game.PlayersList[i].Status.SetPlaceAtLeaderBoard(i + 1);
            game.PlayersList[i].GameCharacter.RollSkillTargetForNextRound();
            game.PlayersList[i].Status.PlaceAtLeaderBoardHistory.Add(new InGameStatus.PlaceAtLeaderBoardHistoryClass(game.RoundNo, game.PlayersList[i].Status.GetPlaceAtLeaderBoard()));
        }
        //end sorting

        //Quality Drop
        var droppedPlayers = game.PlayersList.Where(x => x.GameCharacter.GetStrengthQualityDropTimes() != 0 && x.Status.GetPlaceAtLeaderBoard() != 6).OrderByDescending(x => x.Status.GetPlaceAtLeaderBoard()).ToList();
        
        foreach (var player in droppedPlayers)
        {
            for (var i = 0; i < player.GameCharacter.GetStrengthQualityDropTimes(); i++)
            {
                var oldIndex = game.PlayersList.IndexOf(player);
                var newIndex = oldIndex + 1;

                if (newIndex == 5 && game.PlayersList[newIndex].GameCharacter.Passive.Any(x => x.PassiveName == "Никому не нужен"))
                    continue;
                if(newIndex >= 6)
                    continue;
                    
                game.PlayersList[oldIndex] = game.PlayersList[newIndex];
                game.PlayersList[newIndex] = player;
            }
        }

        if (droppedPlayers.Count > 0)
        {
            for (var i = 0; i < game.PlayersList.Count; i++)
            {
                game.PlayersList[i].Status.SetPlaceAtLeaderBoard(i + 1);
            }
        }
        //end //Quality Drop

        SortGameLogs(game);
        _characterPassives.HandleNextRoundAfterSorting(game);
        _characterPassives.HandleBotPredict(game);
        game.RollExploit();
        game.TimePassed.Reset();
        game.TimePassed.Start();

        if(game.GameMode is "Normal" or "Aram")
            _logs.Info($"Finished calculating game #{game.GameId} (round# {game.RoundNo - 1}). || {watch.Elapsed.TotalSeconds}s");

        watch.Stop();
    }



    public void SortGameLogs(GameClass game)
    {
        var sortedGameLogs = "";
        var extraGameLogs = "\n";
        var logsSplit = game.GetGlobalLogs().Split("\n").ToList();
        logsSplit.RemoveAll(x => x.Length <= 2);
        sortedGameLogs += $"{logsSplit.First()}\n";
        logsSplit.RemoveAt(0);

        for (var i = 0; i < logsSplit.Count; i++)
        {
            if (logsSplit[i].Contains(":war:")) continue;
            extraGameLogs += $"{logsSplit[i]}\n";
            logsSplit.RemoveAt(i);
            i--;
        }

        sortedGameLogs = logsSplit.Aggregate(sortedGameLogs, (current, log) => $"{current}{log}\n");
        /*
        foreach (var player in game.PlayersList)
            for (var i = 0; i < logsSplit.Count; i++)
                if (logsSplit[i].Contains($"{player.DiscordUsername}"))
                {
                    var fightLine = logsSplit[i];

                    var fightLineSplit = fightLine.Split("⟶");

                    var fightLineSplitSplit = fightLineSplit.First().Split("<:war:561287719838547981>");

                    fightLine = fightLineSplitSplit.First().Contains($"{player.DiscordUsername}")
                        ? $"{fightLineSplitSplit.First()} <:war:561287719838547981> {fightLineSplitSplit[1]}"
                        : $"{fightLineSplitSplit[1]} <:war:561287719838547981> {fightLineSplitSplit.First()}";


                    fightLine += $" ⟶ {fightLineSplit[1]}";

                    sortedGameLogs += $"{fightLine}\n";
                    logsSplit.RemoveAt(i);
                    i--;
                }
        */
        sortedGameLogs += extraGameLogs;
        game.SetGlobalLogs(sortedGameLogs);
    }
}