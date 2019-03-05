﻿using System;
using System.Linq;
using System.Threading.Tasks;
using King_of_the_Garbage_Hill.BotFramework;
using King_of_the_Garbage_Hill.Game.Classes;
using King_of_the_Garbage_Hill.Helpers;

namespace King_of_the_Garbage_Hill.Game.Characters
{
    public class CharacterPassives : IServiceSingleton
    {
        //helpers
        private readonly HelperFunctions _help;
        private readonly SecureRandom _rand;
        private readonly InGameGlobal _gameGlobal;
        private readonly LoginFromConsole _log;
        private readonly CharactersUniquePhrase _phrase;
        //end helpers

        //chars
        private readonly Awdka _awdka;
        private readonly DeepList _deepList;
        private readonly DeepList2 _deepList2;
        private readonly Gleb _gleb;
        private readonly HardKitty _hardKitty;
        private readonly LeCrisp _leCrisp;
        private readonly Mitsuki _mitsuki;
        private readonly Mylorik _mylorik;
        private readonly Octopus _octopus;
        private readonly Shark _shark;
        private readonly Sirinoks _sirinoks;
        private readonly Tigr _tigr;
        private readonly Tolya _tolya;
        private readonly Darksci _darksci;
        //end chars
        
        public CharacterPassives(SecureRandom rand, HelperFunctions help, Awdka awdka, DeepList deepList,
            DeepList2 deepList2, Gleb gleb, HardKitty hardKitty, Mitsuki mitsuki, LeCrisp leCrisp, Mylorik mylorik,
            Octopus octopus, Shark shark, Sirinoks sirinoks, Tigr tigr, Tolya tolya, InGameGlobal gameGlobal, Darksci darksci, CharactersUniquePhrase phrase, LoginFromConsole log)
        {
            _rand = rand;
            _help = help;
            _awdka = awdka;
            _deepList = deepList;
            _deepList2 = deepList2;
            _gleb = gleb;
            _hardKitty = hardKitty;
            _mitsuki = mitsuki;
            _leCrisp = leCrisp;
            _mylorik = mylorik;
            _octopus = octopus;
            _shark = shark;
            _sirinoks = sirinoks;
            _tigr = tigr;
            _tolya = tolya;
            _gameGlobal = gameGlobal;
            _darksci = darksci;
            _phrase = phrase;
            _log = log;
        }

        public Task InitializeAsync() => Task.CompletedTask;

        //общее говно
        public async Task HandleEveryAttackOnHim(GameBridgeClass playerGotAttack, GameBridgeClass playerAttackFrom, GameClass game)
        {
            var characterName = playerGotAttack.Character.Name;

            switch (characterName)
            {
                case "Глеб":
                    var rand = _rand.Random(1, 8);
                    if (rand == 1)
                    {
                        var acc = _gameGlobal.GlebChallengerTriggeredWhen.Find(x =>
                            x.DiscordId == playerGotAttack.DiscordAccount.DiscordId && playerGotAttack.DiscordAccount.GameId == x.GameId);

                        if (acc != null)
                        {
                            if (acc.WhenToTrigger.Contains(game.RoundNo))
                            {
                                return;
                            }
                        }

                        if (!playerGotAttack.Status.IsSkip)
                        {
                            playerGotAttack.Status.IsSkip = true;
                            _gameGlobal.GlebSkipList.Add(new Gleb.GlebSkipClass(playerGotAttack.DiscordAccount.DiscordId, game.GameId));

                        }
                        
                    }
                    break;
                case "LeCrisp":
                    //гребанные ассасисны
                    if (playerAttackFrom.Character.Strength - playerGotAttack.Character.Strength >= 2)
                    {
                        playerGotAttack.Status.IsAbleToWin = false;
                        var customMessage =
                            _phrase.LeCrispAssassinsPhrase[_rand.Random(0, _phrase.LeCrispAssassinsPhrase.Count - 1)];
                     var mess  =   await playerGotAttack.Status.SocketMessageFromBot.Channel.SendMessageAsync(customMessage);
                     _help.DeleteMessOverTime(mess, 10);
                    }
                    //end гребанные ассасисны

                    //Импакт: 
                    var lePuska = _gameGlobal.LeCrispImpact.Find(x =>
                        x.DiscordId == playerGotAttack.DiscordAccount.DiscordId && x.GameId == game.GameId);

                    if (lePuska == null)
                    {
                        _gameGlobal.LeCrispImpact.Add(new LeCrisp.LeCrispImpactClass(playerGotAttack.DiscordAccount.DiscordId, game.GameId, game.RoundNo));
                    }
                    // end Импакт: 
                    break;

            }
            await Task.CompletedTask;
        }

        public async Task HandleEveryAttackFromMe(GameBridgeClass player1, GameBridgeClass player2, GameClass game)
        {
            var characterName = player1.Character.Name;

            switch (characterName)
            {
                case "Глеб":
                    // Я за чаем:
                    var rand = _rand.Random(1, 10);
                    if (rand == 1)
                    {
                        _gameGlobal.AllSkipTriggeredWhen.Add(new WhenToTriggerClass(player1.Status.WhoToAttackThisTurn, game.GameId,
                            game.RoundNo + 1));
                        player1.Status.AddRegularPoints(1);
                        
                    }
                    //end  Я за чаем:



                    break;
                case "LeCrisp":
                    //Еврей
                    foreach (var player in game.PlayersList)
                    {
                        if (player.Status.WhoToAttackThisTurn == player2.DiscordAccount.DiscordId &&
                            player.DiscordAccount.DiscordId != player1.DiscordAccount.DiscordId)
                        {
                          
                            if (player.Character.Name != "DeepList")
                            {
                                //TODO, он забирает не все, а чуть чуть....
                                player1.Status.AddRegularPoints(1);

                                var customMessage =
                                    _phrase.LeCrispJewPhrase[_rand.Random(0, _phrase.LeCrispJewPhrase.Count - 1)];
                                await player1.Status.SocketMessageFromBot.Channel.SendMessageAsync(customMessage);
                            }

                        } 
                    }
                    //end Еврей


                    break;
            }

            await Task.CompletedTask;
        }

        public async Task HandleCharacterBeforeCalculations(GameBridgeClass player, GameClass game)
        {
            var characterName = player.Character.Name;
            switch (characterName)
            {
                case "DeepList":
                    _deepList.HandleDeepList(player);
                    break;
                case "mylorik":
                    await _mylorik.HandleMylorik(player, game);
                    break;
                case "Глеб":
                    _gleb.HandleGleb(player);
                    break;
                case "LeCrisp":
                    _leCrisp.HandleLeCrisp(player);
                    break;
                case "Толя":
                    _tolya.HandleTolya(player);
                    break;
                case "HardKitty":
                    _hardKitty.HandleHardKitty(player);
                    break;
                case "Sirinoks":
                    _sirinoks.HandleSirinoks(player);
                    break;
                case "Mitsuki":
                    _mitsuki.HandleMitsuki(player);
                    break;
                case "AWDKA":
                    _awdka.HandleAWDKA(player);
                    break;
                case "Осьминожка":
                    _octopus.HandleOctopus(player);
                    break;
                case "Darksci":
                    _darksci.HandleDarksci(player);
                    break;
                case "Тигр":
                    _tigr.HandleTigr(player);
                    break;
                case "Братишка":
                    _shark.HandleShark(player);
                    break;
                case "????":
                    _deepList2.HandleDeepList2(player);
                    break;
            }

            await Task.CompletedTask;
        }

        public async Task HandleCharacterAfterCalculations(GameBridgeClass player, GameClass game)
        {
            var characterName = player.Character.Name;
            switch (characterName)
            {
                case "DeepList":
                    _deepList.HandleDeepListAfter(player, game);
                    break;
                case "mylorik":
                    _mylorik.HandleMylorikAfter(player);
                    break;
                case "Глеб":
                    _gleb.HandleGlebAfter(player);
                    break;
                case "LeCrisp":
                    _leCrisp.HandleLeCrispAfter(player);
                    break;
                case "Толя":
                    _tolya.HandleTolyaAfter(player);
                    break;
                case "HardKitty":
                    _hardKitty.HandleHardKittyAfter(player);
                    break;
                case "Sirinoks":
                    _sirinoks.HandleSirinoksAfter(player);
                    break;
                case "Mitsuki":
                    _mitsuki.HandleMitsukiAfter(player);
                    break;
                case "AWDKA":
                    _awdka.HandleAWDKAAfter(player);
                    break;
                case "Осьминожка":
                    _octopus.HandleOctopusAfter(player);
                    break;
                case "Darksci":
                    _darksci.HandleDarksiAfter(player);
                    break;
                case "Тигр":
                    _tigr.HandleTigrAfter(player);
                    break;
                case "Братишка":
                    _shark.HandleSharkAfter(player);
                    break;
                case "????":
                    _deepList2.HandleDeepList2After(player);
                    break;
            }

            await Task.CompletedTask;
        }

        public void CalculatePassiveChances(GameClass game)
        {
            foreach (var player in game.PlayersList)
            {
                var characterName = player.Character.Name;
                WhenToTriggerClass when;
                switch (characterName)
                {
                    case "DeepList":

                        when = _gameGlobal.GetWhenToTrigger(player, true, 7, 1);
                        _gameGlobal.DeepListSupermindTriggeredWhen.Add(when);
                       when = _gameGlobal.GetWhenToTrigger(player, false, 10, 2);
                        _gameGlobal.DeepListMadnessTriggeredWhen.Add(when);

                        break;
                    case "mylorik":
                        when = _gameGlobal.GetWhenToTrigger(player, false, 10, 2);
                        _gameGlobal.MylorikBooleTriggeredWhen.Add(when);
                        break;

                    case "Глеб":
                        //Спящее хуйло chance
                        when = _gameGlobal.GetWhenToTrigger(player, true, 4, 3);
                        _gameGlobal.GlebSleepingTriggeredWhen.Add(when);

                        //challenger when
                        int[] temp = {-1, -1, -1};
                        if (when.WhenToTrigger.Count >= 1)
                            temp[0] = when.WhenToTrigger[0];
                        if (when.WhenToTrigger.Count >= 2)
                            temp[1] = when.WhenToTrigger[1];
                        if (when.WhenToTrigger.Count >= 3)
                            temp[2] = when.WhenToTrigger[2];
                        do
                        {
                            when = _gameGlobal.GetWhenToTrigger(player, true, 12, 3, true);
                        } while (when.WhenToTrigger.All(x => x == temp[0] || x == temp[1] || x == temp[2]));

                        _gameGlobal.GlebChallengerTriggeredWhen.Add(when);

                        break;
                }
            }
        }


        public async Task HandleNextRound(GameClass game)
        {
            foreach (var player in game.PlayersList)
            {
                var characterName = player.Character.Name;
                switch (characterName)
                {
                    case "mylorik":
                        var acc = _gameGlobal.MylorikBooleTriggeredWhen.Find(x =>
                            x.DiscordId == player.DiscordAccount.DiscordId && player.DiscordAccount.GameId == x.GameId);

                        if (acc != null)
                            if (acc.WhenToTrigger.Contains(game.RoundNo))
                            {
                                player.Status.IsSkip = true;
                                player.Status.IsBlock = false;
                                player.Status.IsAbleToTurn = false;
                                player.Status.IsReady = true;
                                player.Status.WhoToAttackThisTurn = 0;
                                var mess = await player.Status.SocketMessageFromBot.Channel
                                    .SendMessageAsync("Ты буль.");
                                 _help.DeleteMessOverTime(mess, 15);
                            }

                        break;
                    case "Глеб":
                        //Спящее хуйло:
                        acc = _gameGlobal.GlebSleepingTriggeredWhen.Find(x =>
                            x.DiscordId == player.DiscordAccount.DiscordId && player.DiscordAccount.GameId == x.GameId);

                        if (acc != null)
                            if (acc.WhenToTrigger.Contains(game.RoundNo))
                            {
                                player.Status.IsSkip = true;
                                player.Status.IsBlock = false;
                                player.Status.IsAbleToTurn = false;
                                player.Status.IsReady = true;
                                player.Status.WhoToAttackThisTurn = 0;
                                var mess =
                                    await player.Status.SocketMessageFromBot.Channel.SendMessageAsync("Ты уснул.");
                                 _help.DeleteMessOverTime(mess, 15);
                            }

                        //Претендент русского сервера: 
                        acc = _gameGlobal.GlebChallengerTriggeredWhen.Find(x =>
                            x.DiscordId == player.DiscordAccount.DiscordId && player.DiscordAccount.GameId == x.GameId);

                        if (acc != null)
                            if (acc.WhenToTrigger.Contains(game.RoundNo))
                            {

                                var curr = _gameGlobal.GlebChallengerList.Find(x =>
                                    x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId);
                                //just check
                                if (curr != null)
                                {
                                    _gameGlobal.GlebChallengerList.Remove(curr);
                                }

                                _gameGlobal.GlebChallengerList.Add(new DeepList.Madness(player.DiscordAccount.DiscordId, game.GameId, game.RoundNo));
                                curr = _gameGlobal.GlebChallengerList.Find(x => x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId);
                                curr.MadnessList.Add(new DeepList.MadnessSub(1, player.Character.Intelligence, player.Character.Strength, player.Character.Speed, player.Character.Psyche));

                                //  var randomNumber =  _rand.Random(1, 100);
                               
                                var intel = player.Character.Intelligence >= 10 ? 10 : 9;
                                var str = player.Character.Strength >= 10 ? 10 : 9;
                                var speed = player.Character.Speed >= 10 ? 10 : 9;
                                var pshy = player.Character.Psyche >= 10 ? 10 : 9;


                                player.Character.Intelligence = intel;
                                player.Character.Strength = str;
                                player.Character.Speed = speed;
                                player.Character.Psyche = pshy;


                                curr.MadnessList.Add(new DeepList.MadnessSub(2, intel, str, speed, pshy));

                                var customMess =
                                    _phrase.GlebChallengerPhrase[
                                        _rand.Random(0, _phrase.GlebChallengerPhrase.Count - 1)];
                                var mess = await player.Status.SocketMessageFromBot.Channel.SendMessageAsync(
                                    customMess);

                                 _help.DeleteMessOverTime(mess, 15);
                            }

                        break;
                    case "DeepList":

                        //Сверхразум
                        var currentDeepList = _gameGlobal.DeepListSupermindTriggeredWhen.Find(x =>
                            x.DiscordId == player.DiscordAccount.DiscordId && game.GameId == x.GameId);

                        if (currentDeepList != null)
                        {
                            if (currentDeepList.WhenToTrigger.Any(x => x == game.RoundNo))
                            {
                                GameBridgeClass randPlayer;


                                do
                                {
                                    randPlayer = game.PlayersList[_rand.Random(0, game.PlayersList.Count - 1)];
                                    var check1 = _gameGlobal.DeepListSupermindKnown.Find(x =>
                                        x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId);
                                    if (check1 != null)
                                    {
                                        if (check1.KnownPlayers.Contains(randPlayer.DiscordAccount.DiscordId))
                                        {
                                            randPlayer = player;
                                        }
                                    }
                                    
                                } while (randPlayer.DiscordAccount.DiscordId != player.DiscordAccount.DiscordId);

                                var check = _gameGlobal.DeepListSupermindKnown.Find(x =>
                                    x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId);
                                if (check == null)
                                {
                                    _gameGlobal.DeepListSupermindKnown.Add(new DeepList.SuperMindKnown(player.DiscordAccount.DiscordId, game.GameId,randPlayer.DiscordAccount.DiscordId));
                                }
                                else
                                {
                                    check.KnownPlayers.Add(randPlayer.DiscordAccount.DiscordId);
                                }

                                var customMess =
                                    _phrase.DeepListSuperMindPhrase[
                                        _rand.Random(0, _phrase.DeepListSuperMindPhrase.Capacity - 1)];

                                var mess = await player.Status.SocketMessageFromBot.Channel.SendMessageAsync(
                                    $"{customMess} {randPlayer.DiscordAccount.DiscordUserName} - {randPlayer.Character.Name}!\n" +
                                    $"Его статы: Cила {randPlayer.Character.Strength}, Скорость {randPlayer.Character.Speed}" +
                                    $", Ум {randPlayer.Character.Intelligence}");

                                 _help.DeleteMessOverTime(mess, 45);
                            }
                        }
                        else
                        {
                            Console.WriteLine("DEEP LIST SUPERMIND PASSIVE ERRORR!!!!!!!!!!!!!!!!!");
                        }
                        //end Сверхразум

                        //Madness

                        var madd = _gameGlobal.DeepListMadnessTriggeredWhen.Find(x =>
                            x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId);

                        if (madd != null)
                        {
                            if (madd.WhenToTrigger.Contains(game.RoundNo))
                            {
                                //trigger maddness

                             var curr = _gameGlobal.DeepListMadnessList.Find(x =>
                                    x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId);
                             //just check
                             if (curr != null)
                             {
                                 _gameGlobal.DeepListMadnessList.Remove(curr);
                             }

                             _gameGlobal.DeepListMadnessList.Add(new DeepList.Madness(player.DiscordAccount.DiscordId, game.GameId, game.RoundNo));
                             curr = _gameGlobal.DeepListMadnessList.Find(x => x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId);
                             curr.MadnessList.Add(new DeepList.MadnessSub(1, player.Character.Intelligence, player.Character.Strength, player.Character.Speed, player.Character.Psyche));


                          //  var randomNumber =  _rand.Random(1, 100);
                             var intel = 0;
                             var str = 0;
                             var speed = 0;
                             var pshy = 0;





                             curr.MadnessList.Add(new DeepList.MadnessSub(2, intel, str, speed, pshy));
                            }
                        }
                        //end madness

                        break;
                }

                var isSkip = _gameGlobal.AllSkipTriggeredWhen.Find(x =>
                    x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId &&
                    x.WhenToTrigger.Contains(game.RoundNo));

                if (isSkip != null)
                {
                    player.Status.IsSkip = true;
                    player.Status.IsBlock = false;
                    player.Status.IsAbleToTurn = false;
                    player.Status.IsReady = true;
                    player.Status.WhoToAttackThisTurn = 0;
                    var mess = await player.Status.SocketMessageFromBot.Channel.SendMessageAsync(
                        "Тебя заставили пропустить этот ход");
                     _help.DeleteMessOverTime(mess, 15);
                }
            }

     
        }

        public async Task HandleEndOfRound(GameClass game)
        {
            foreach (var player in game.PlayersList)
            {
                if (player == null)
                {
                    _log.Critical("HandleEndOfRound - player == null");
                }
                var characterName = player.Character.Name;
                switch (characterName)
                {
                    case "DeepList":
                        
                        //madness
                        var madd = _gameGlobal.DeepListMadnessList.Find(x =>
                            x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId && x.RoundItTriggered == game.RoundNo);

                        if (madd != null)
                        {
                            var regularStats = madd.MadnessList.Find(x => x.Index == 1);
                            var madStats = madd.MadnessList.Find(x => x.Index == 2);

                                   
                            var intel =  player.Character.Intelligence - madStats.Intel;
                            var str = player.Character.Strength -  madStats.Str ;
                            var speed = player.Character.Speed -  madStats.Speed ;
                            var psy =  player.Character.Psyche - madStats.Psyche ;

                                                                 
                            player.Character.Intelligence = regularStats.Intel + intel;
                            player.Character.Strength = regularStats.Str + str;
                            player.Character.Speed = regularStats.Speed + speed;
                            player.Character.Psyche = regularStats.Psyche + psy;
                            _gameGlobal.DeepListMadnessList.Remove(madd);
                        }
                        // end madness 
                        break;
                    case "Глеб":
                        //challenger
                         madd = _gameGlobal.GlebChallengerList.Find(x =>
                            x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId && x.RoundItTriggered == game.RoundNo);

                        if (madd != null)
                        {
                            var regularStats = madd.MadnessList.Find(x => x.Index == 1);
                            var madStats = madd.MadnessList.Find(x => x.Index == 2);


                            var intel = player.Character.Intelligence - madStats.Intel;
                            var str = player.Character.Strength - madStats.Str;
                            var speed = player.Character.Speed - madStats.Speed;
                            var psy = player.Character.Psyche - madStats.Psyche;


                            player.Character.Intelligence = regularStats.Intel + intel;
                            player.Character.Strength = regularStats.Str + str;
                            player.Character.Speed = regularStats.Speed + speed;
                            player.Character.Psyche = regularStats.Psyche + psy;
                            _gameGlobal.GlebChallengerList.Remove(madd);
                        }
                        break;
                    case "LeCrisp":
                        //impact

                        var leImpact = _gameGlobal.LeCrispImpact.Find(x =>
                            x.DiscordId == player.DiscordAccount.DiscordId && x.GameId == game.GameId);
                        if (leImpact != null)
                        {
                            _gameGlobal.LeCrispImpact.Remove(leImpact);
                        }
                        else
                        {
                            player.Status.AddBonusPoints(1);
                        }
                        
                        //end impact

                        break;
                }
            }

            await Task.CompletedTask;
        }

        //end общие говно
    }
}