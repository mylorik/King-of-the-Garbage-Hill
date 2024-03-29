﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using King_of_the_Garbage_Hill.Game.Classes;

//using Discord;

namespace King_of_the_Garbage_Hill.Game.MemoryStorage;

public class CharactersUniquePhrase
{
    //initialize variables 
    public PhraseClass AwdkaAfk;
    public PhraseClass AwdkaTeachToPlay;
    public PhraseClass AwdkaTrolling;
    public PhraseClass AwdkaTrying;

    public PhraseClass DarksciDysmoral;
    public PhraseClass DarksciFuckThisGame;
    public PhraseClass DarksciLucky;
    public PhraseClass DarksciNotLucky;
    public PhraseClass DeepListDoubtfulTacticFirstLostPhrase;

    public PhraseClass DeepListDoubtfulTacticPhrase;
    public PhraseClass DeepListMadnessPhrase;
    public PhraseClass DeepListPokePhrase;
    public PhraseClass DeepListSuperMindPhrase;


    public PhraseClass FirstСommandment;
    public PhraseClass FirstСommandmentLost;
    public PhraseClass FourthСommandment;
    public PhraseClass SecondСommandment;
    public PhraseClass SecondСommandmentBan;
    public PhraseClass ThirdСommandment;

    public PhraseClass GlebChallengerPhrase;
    public PhraseClass GlebChallengerSeparatePhrase;
    public PhraseClass GlebComeBackPhrase;
    public PhraseClass GlebSleepyPhrase;
    public PhraseClass GlebTeaPhrase;
    public PhraseClass GlebTeaReadyPhrase;


    public PhraseClass YongGlebIrelia;
    public PhraseClass YongGlebCommunication;
    public PhraseClass YongGlebCommunicationReady;
    public PhraseClass YongGlebMeta;
    public PhraseClass YongGlebTea;
    public PhraseClass YongGlebTeaReady;

    public PhraseClass HardKittyDoebatsyaAnswerPhrase;
    public PhraseClass HardKittyDoebatsyaPhrase;
    public PhraseClass HardKittyDoebatsyaLovePhrase;
    public PhraseClass HardKittyLonelyPhrase;
    public PhraseClass HardKittyMutedPhrase;

    public PhraseClass LeCrispAssassinsPhrase;
    public PhraseClass LeCrispBoolingPhrase;
    public PhraseClass LeCrispImpactPhrase;
    public PhraseClass LeCrispJewPhrase;

    public PhraseClass MitsukiCheekyBriki;
    public PhraseClass MitsukiGarbageSmell;
    public PhraseClass MitsukiSchoolboy;
    public PhraseClass MitsukiTooMuchFucking;
    public PhraseClass MitsukiTooMuchFuckingNoAttack;

    public PhraseClass MylorikBoolePhrase;
    public PhraseClass MylorikRevengeLostPhrase;
    public PhraseClass MylorikRevengeVictoryPhrase;
    public PhraseClass MylorikSpanishPhrase;


    public PhraseClass SirinoksDragonPhrase;
    public PhraseClass SirinoksFriendsPhrase;
    public PhraseClass SpartanDragonSlayer;

    public PhraseClass SpartanShameMylorik;
    public PhraseClass SpartanTheyWontLikeIt;
    public PhraseClass SpartanFirstBlood;

    public PhraseClass TigrSnipe;
    public PhraseClass TigrThreeZero;
    public PhraseClass TigrTop;
    public PhraseClass TigrTwoBetter;

    public PhraseClass TolyaCountPhrase;
    public PhraseClass TolyaCountReadyPhrase;
    public PhraseClass TolyaJewPhrase;
    public PhraseClass TolyaRammus2Phrase;
    public PhraseClass TolyaRammus3Phrase;
    public PhraseClass TolyaRammus4Phrase;
    public PhraseClass TolyaRammus5Phrase;
    public PhraseClass TolyaRammusPhrase;

    public PhraseClass VampyrHematophagy;
    public PhraseClass VampyrNoAttack;
    public PhraseClass VampyrSuckspenStake;
    public PhraseClass VampyrVampyr;
    public PhraseClass VampyrTheLoh;

    public PhraseClass CraboRackSidewaysBoolePhrase;

    public PhraseClass WeedwickRuthlessHunter;
    public PhraseClass WeedwickValuablePreyPoints1;
    public PhraseClass WeedwickValuablePreyPoints2;
    public PhraseClass WeedwickValuablePreyPoints3;
    public PhraseClass WeedwickValuablePreyPoints4;
    public PhraseClass WeedwickValuablePreyPoints5;
    public PhraseClass WeedwickValuablePreyPoints6;
    public PhraseClass WeedwickValuablePreyPoints7;
    public PhraseClass WeedwickValuablePreyDrop;
    public PhraseClass WeedwickWeedYes1;
    public PhraseClass WeedwickWeedYes2;
    public PhraseClass WeedwickWeedYes3;
    public PhraseClass WeedwickWeedYes4;
    public PhraseClass WeedwickWeedYes5;
    public PhraseClass WeedwickWeedYes6;
    public PhraseClass WeedwickWeedYes7;
    public PhraseClass WeedwickWeedYes8;
    public PhraseClass WeedwickWeedYes9;
    public PhraseClass WeedwickWeedYes10;
    public PhraseClass WeedwickWeedYes11;

    public PhraseClass WeedwickWeedNo;


    public PhraseClass KratosEventYes;
    public PhraseClass KratosEventNo;
    public PhraseClass KratosEventFailed;
    public PhraseClass KratosEventKill;
    public PhraseClass KratosTarget;


    //end
    public PhraseClass AutoMove;


    public CharactersUniquePhrase()
    {
        AutoMove = new PhraseClass("Авто Ход");

        //add values
        FirstСommandment = new PhraseClass("Первая заповедь");
        FirstСommandmentLost = new PhraseClass("Первая заповедь");
        SecondСommandment = new PhraseClass("Вторая заповедь");
        ThirdСommandment = new PhraseClass("Третья заповедь");
        SecondСommandmentBan = new PhraseClass("Третья заповедь");
        FourthСommandment = new PhraseClass("Четвертая заповедь");

        VampyrHematophagy = new PhraseClass("Гематофагия");
        VampyrSuckspenStake = new PhraseClass("СОсиновый кол");
        VampyrNoAttack = new PhraseClass("СОсиновый кол");
        VampyrVampyr = new PhraseClass("VampУr");
        VampyrTheLoh = new PhraseClass("Vampyr Позорный");


        SpartanShameMylorik = new PhraseClass("Искусство");
        SpartanDragonSlayer = new PhraseClass("2kxaoc");
        SpartanTheyWontLikeIt = new PhraseClass("Guard Break");
        SpartanFirstBlood = new PhraseClass("Первая кровь");


        TigrTwoBetter = new PhraseClass("Лучше с двумя, чем с адекватными");
        TigrThreeZero = new PhraseClass("3-0 обоссан");
        TigrTop = new PhraseClass("Тигр топ, а ты холоп");
        TigrSnipe = new PhraseClass("Стримснайпят и банят и банят и банят");

        DeepListMadnessPhrase = new PhraseClass("Безумие");

        DeepListPokePhrase = new PhraseClass("__Стёб__");
        DeepListDoubtfulTacticPhrase = new PhraseClass("Сомнительная тактика");
        DeepListDoubtfulTacticFirstLostPhrase = new PhraseClass("Сомнительная тактика");
        DeepListSuperMindPhrase = new PhraseClass("Сверхразум");

        MylorikRevengeLostPhrase = new PhraseClass("Месть");
        MylorikRevengeVictoryPhrase = new PhraseClass("Месть");
        MylorikBoolePhrase = new PhraseClass("Буль");
        MylorikSpanishPhrase = new PhraseClass("Испанец");

        GlebChallengerPhrase = new PhraseClass("Претендент русского сервера");
        GlebChallengerSeparatePhrase = new PhraseClass("Претендент русского сервера");
        GlebSleepyPhrase = new PhraseClass("Спящее хуйло");
        GlebComeBackPhrase = new PhraseClass("Я щас приду");
        GlebTeaPhrase = new PhraseClass("Я за чаем");
        GlebTeaReadyPhrase = new PhraseClass("__**Я за чаем**");

        YongGlebIrelia = new PhraseClass("Main Ирелия");
        YongGlebCommunication = new PhraseClass("Коммуникация");
        YongGlebCommunicationReady = new PhraseClass("Коммуникация");
        YongGlebMeta = new PhraseClass("Следит за игрой");
        YongGlebTea = new PhraseClass("Спокойствие");
        YongGlebTeaReady = new PhraseClass("__**Спокойствие**");

        LeCrispJewPhrase = new PhraseClass("Еврей");
        LeCrispBoolingPhrase = new PhraseClass("Булинг");
        LeCrispAssassinsPhrase = new PhraseClass("Гребанные ассассины");
        LeCrispImpactPhrase = new PhraseClass("Импакт");

        TolyaJewPhrase = new PhraseClass("Еврей");
        TolyaCountPhrase = new PhraseClass("Подсчет");
        TolyaCountReadyPhrase = new PhraseClass("__**Подсчет**");
        TolyaRammusPhrase = new PhraseClass("Раммус мейн");
        TolyaRammus2Phrase = new PhraseClass("Раммус мейн");
        TolyaRammus3Phrase = new PhraseClass("Раммус мейн");
        TolyaRammus4Phrase = new PhraseClass("Раммус мейн");
        TolyaRammus5Phrase = new PhraseClass("Раммус мейн");

        HardKittyLonelyPhrase = new PhraseClass("Одиночество");
        HardKittyDoebatsyaPhrase = new PhraseClass("Доебаться");
        HardKittyDoebatsyaLovePhrase = new PhraseClass("Доебаться");
        HardKittyMutedPhrase = new PhraseClass("Mute");
        HardKittyDoebatsyaAnswerPhrase = new PhraseClass("Доебаться");

        SirinoksFriendsPhrase = new PhraseClass("Заводить друзей");
        SirinoksDragonPhrase = new PhraseClass("Дракон");

        MitsukiCheekyBriki = new PhraseClass("Дерзкая школота");
        MitsukiTooMuchFucking = new PhraseClass("Много выебывается");
        MitsukiTooMuchFuckingNoAttack = new PhraseClass("Много выебывается");
        MitsukiGarbageSmell = new PhraseClass("Запах мусора");
        MitsukiSchoolboy = new PhraseClass("Школьник");

        AwdkaTeachToPlay = new PhraseClass("Научите играть");
        AwdkaTrolling = new PhraseClass("Произошел троллинг");
        AwdkaTrying = new PhraseClass("Я пытаюсь!");
        AwdkaAfk = new PhraseClass("АФКА");


        DarksciNotLucky = new PhraseClass("Не повезло");
        DarksciLucky = new PhraseClass("Повезло");
        DarksciFuckThisGame = new PhraseClass("Да всё нахуй эту игру");
        DarksciDysmoral = new PhraseClass("Дизмораль");

        CraboRackSidewaysBoolePhrase = new PhraseClass("Хождение боком");

        WeedwickRuthlessHunter = new PhraseClass("Безжалостный охотник");
        WeedwickValuablePreyPoints1 = new PhraseClass("Ценная добыча");
        WeedwickValuablePreyPoints2 = new PhraseClass("Ценная добыча");
        WeedwickValuablePreyPoints3 = new PhraseClass("Ценная добыча");
        WeedwickValuablePreyPoints4 = new PhraseClass("Ценная добыча");
        WeedwickValuablePreyPoints5 = new PhraseClass("Ценная добыча");
        WeedwickValuablePreyPoints6 = new PhraseClass("Ценная добыча");
        WeedwickValuablePreyPoints7 = new PhraseClass("Ценная добыча");
        WeedwickValuablePreyDrop = new PhraseClass("Ценная добыча");
        WeedwickWeedYes1 = new PhraseClass("Weed");
        WeedwickWeedYes2 = new PhraseClass("Weed");
        WeedwickWeedYes3 = new PhraseClass("Weed");
        WeedwickWeedYes4 = new PhraseClass("Weed");
        WeedwickWeedYes5 = new PhraseClass("Weed");
        WeedwickWeedYes6 = new PhraseClass("Weed");
        WeedwickWeedYes7 = new PhraseClass("Weed");
        WeedwickWeedYes8 = new PhraseClass("Weed");
        WeedwickWeedYes9 = new PhraseClass("Weed");
        WeedwickWeedYes10 = new PhraseClass("Weed");
        WeedwickWeedYes11 = new PhraseClass("Weed");
        WeedwickWeedNo = new PhraseClass("Weed");

        KratosEventYes = new PhraseClass("Возвращение из мертвых");
        KratosEventNo = new PhraseClass("Возвращение из мертвых");
        KratosEventFailed = new PhraseClass("Возвращение из мертвых");
        KratosEventKill = new PhraseClass("Возвращение из мертвых");
        KratosTarget = new PhraseClass("Охота на богов");
        //end

        //
        AutoMove.PassiveLogRus.Add("Ты что, бот?");
        AutoMove.PassiveLogRus.Add("А ну играй сам! Я для кого игру делал?");
        AutoMove.PassiveLogRus.Add("Слышь, не трогай эту кнопку, она для админов");
        AutoMove.PassiveLogRus.Add("Сложно самому походить что ли?");
        AutoMove.PassiveLogRus.Add("Серьезно, уже пора показывать скилл. САМОМУ.");
        AutoMove.PassiveLogRus.Add("Я щас заблокирую это кнопку, если еще раз нажмешь.");
        AutoMove.PassiveLogRus.Add("Нет, я щас лучше тебя заблокирую. Будешь как Тигр сидеть");
        AutoMove.PassiveLogRus.Add("Давай-давай, игра уже щас закончится... Ну же... Ну... НУ!!! ПОХОДИ");
        AutoMove.PassiveLogRus.Add("ДАВАЙ, ВСЕГО ОДИН ХОД");
        AutoMove.PassiveLogRus.Add("Пиздец. Всю игру просидел в автоходе... Вы там вообще живой? Может вы умер?... Глеб?");
        //

        //add  as many phrases as you wany
        FirstСommandment.PassiveLogRus.Add("Если фидишь то пропушь, если пушишь то нафидь");
        FirstСommandmentLost.PassiveLogRus.Add("Тебе нужна помощь бога");
        FirstСommandmentLost.PassiveLogRus.Add("Багословляю, сын мой");
        FirstСommandmentLost.PassiveLogRus.Add("Ты пришел к богу");
        FirstСommandmentLost.PassiveLogRus.Add("Просветление");
        FirstСommandmentLost.PassiveLogRus.Add("Только вера спасет");
        SecondСommandment.PassiveLogRus.Add(
            "И как боженька сказал - \"Бань удира, я зассал\" - и как доктор говорит - \"Не забанил - инвалид\"");
        SecondСommandmentBan.PassiveLogRus.Add("Изыди!");
        SecondСommandmentBan.PassiveLogRus.Add("Нечесть");
        SecondСommandmentBan.PassiveLogRus.Add("Бань удира.");
        ThirdСommandment.PassiveLogRus.Add("Не флейми в чате");

        FourthСommandment.PassiveLogRus.Add("Играй руками");

        VampyrVampyr.PassiveLogRus.Add("Вампурская ноо-ооочь!");
        VampyrVampyr.PassiveLogRus.Add("Таинственный Вампур");
        VampyrHematophagy.PassiveLogRus.Add("Я - Вампур из Лондона!");
        VampyrHematophagy.PassiveLogRus.Add("Какая питательная кровь");
        VampyrHematophagy.PassiveLogRus.Add("Вампур-шампур!");
        VampyrHematophagy.PassiveLogRus.Add("Клыки как бур - Вампур");
        VampyrHematophagy.PassiveLogRus.Add("ВОТ МОЯ ФИНАЛЬНАЯ ФОРМА, теперь я - Vampyr!");
        VampyrSuckspenStake.PassiveLogRus.Add("Ай");
        VampyrSuckspenStake.PassiveLogRus.Add("Мое Вампурское сердце не выдержит");
        VampyrSuckspenStake.PassiveLogRus.Add("Ниеееет!");
        VampyrSuckspenStake.PassiveLogRus.Add("Пфхаааш!");
        VampyrSuckspenStake.PassiveLogRus.Add("Прибавляется, отнимается, прибавляется, отнимается");

        VampyrNoAttack.PassiveLogRus.Add("Не хочу, не буду");
        VampyrNoAttack.PassiveLogRus.Add("Аа я баюс его");
        VampyrNoAttack.PassiveLogRus.Add("Это Хельсинг, начальнике");
        VampyrNoAttack.PassiveLogRus.Add("От него пахнет чесноком!");
        VampyrNoAttack.PassiveLogRus.Add("Он мне кол в жопу засунул");


        VampyrTheLoh.PassiveLogRus.Add("Никаких статов для тебя, поешь чеснока");
        VampyrTheLoh.PassiveLogRus.Add("Иди отсюда, Вампур позорный");
        VampyrTheLoh.PassiveLogRus.Add("А ну хватит кусаться!");
        VampyrTheLoh.PassiveLogRus.Add("Клыки наточил?");



        SpartanShameMylorik.PassiveLogRus.Add("ОН уважает военное искуство!");
        SpartanDragonSlayer.PassiveLogRus.Add("*Oторвался от остальных на 2000 голды*");
        SpartanTheyWontLikeIt.PassiveLogRus.Add("Твой щит - ничто для моего копья!");
        SpartanTheyWontLikeIt.PassiveLogRus.Add("Настакал blackcleaver!");
        SpartanTheyWontLikeIt.PassiveLogRus.Add("Мое копье создано что бы пронзить НЕБЕСА!");
        SpartanTheyWontLikeIt.PassiveLogRus.Add("Кину копье, напрыгну, отхуярю щитом");
        SpartanTheyWontLikeIt.PassiveLogRus.Add("Я твой фанат. Я пантеон.");
        SpartanFirstBlood.PassiveLogRus.Add("Кулю на эти деньги сапоги! +1 Скорость");


        TigrTwoBetter.PassiveLogRus.Add("Лучше с двумя, чем с адекватными");
        TigrTwoBetter.PassiveLogRus.Add("Добро пожаловать в мой клан!");

        TigrThreeZero.PassiveLogRus.Add("Го 1v1");
        TigrThreeZero.PassiveLogRus.Add("2:0");
        TigrThreeZero.PassiveLogRus.Add("Изи 3-0, обоссан");
        TigrTop.PassiveLogRus.Add("Тигр топ, а ты холоп!");
        TigrTop.PassiveLogRus.Add("Я - ТОП1 БЕРСЕРК НА СЕРВЕРЕ!!!");
        TigrSnipe.PassiveLogRus.Add("ЕБАНЫЕ БАНЫ НА 10 ЛЕТ");

        DarksciNotLucky.PassiveLogRus.Add("Сука, не везет с командой!");
        DarksciNotLucky.PassiveLogRus.Add("И вот так каждое промо...");
        DarksciNotLucky.PassiveLogRus.Add("Что они творят?");
        DarksciNotLucky.PassiveLogRus.Add("Я тилтед");
        DarksciLucky.PassiveLogRus.Add("Золотой");
        DarksciLucky.PassiveLogRus.Add("Наконец-то!");
        DarksciFuckThisGame.PassiveLogRus.Add("Нахуй эту игру");
        DarksciDysmoral.PassiveLogRus.Add("Всё, у меня горит!");

        DeepListMadnessPhrase.PassiveLogRus.Add("Хаха. Ха. АХАХАХАХАХАХАХ!");
        DeepListMadnessPhrase.PassiveLogRus.Add("Стоп, кто... я?");
        DeepListMadnessPhrase.PassiveLogRus.Add("Заткнитесь!");
        DeepListMadnessPhrase.PassiveLogRus.Add("Да бля бля Сука бля");
        DeepListMadnessPhrase.PassiveLogRus.Add("Какая полезная филактерия");
        DeepListMadnessPhrase.PassiveLogRus.Add("Невидимый волк. Он пришел за мной!!!");
        DeepListMadnessPhrase.PassiveLogRus.Add("Я самый хитрый таракан");
        DeepListMadnessPhrase.PassiveLogRus.Add("Покажите звук");
        DeepListMadnessPhrase.PassiveLogRus.Add("БАСЛОВУШКА");
        DeepListMadnessPhrase.PassiveLogRus.Add("Доктор, у меня проблема: Я слышу голоса, но сейчас их нет...");
        DeepListMadnessPhrase.PassiveLogRus.Add("Доктор Чо");
        DeepListMadnessPhrase.PassiveLogRus.Add("*Звуки губной гармошки*");
        DeepListMadnessPhrase.PassiveLogRus.Add("*Звуки расстроенной гитары*");
        

        

        DeepListPokePhrase.PassiveLogRus.Add("Хехе");
        DeepListPokePhrase.PassiveLogRus.Add("Чисто постЕбаться");
        DeepListPokePhrase.PassiveLogRus.Add("Я в деле!");
        DeepListPokePhrase.PassiveLogRus.Add("Два игнайта...");
        DeepListPokePhrase.PassiveLogRus.Add("*Хохочет*");
        DeepListPokePhrase.PassiveLogRus.Add("Ты кто такой?");
        DeepListPokePhrase.PassiveLogRus.Add("Скобочка");
        DeepListPokePhrase.PassiveLogRus.Add("It's a joke. You got it?");
        DeepListPokePhrase.PassiveLogRus.Add("Где импакт!?");
        DeepListPokePhrase.PassiveLogRus.Add("Вариан?");
        DeepListPokePhrase.PassiveLogRus.Add("Барон!");
        DeepListPokePhrase.PassiveLogRus.Add("Задень головой");
        DeepListPokePhrase.PassiveLogRus.Add("Хуле ты встал?");
        DeepListPokePhrase.PassiveLogRus.Add("Не руби сук");
        DeepListPokePhrase.PassiveLogRus.Add("Обмылки");
        DeepListPokePhrase.PassiveLogRus.Add("Ты жирный");
        DeepListPokePhrase.PassiveLogRus.Add("Ты тоже");
        DeepListPokePhrase.PassiveLogRus.Add("Как говорит мясник: Люди делятся");
        DeepListPokePhrase.PassiveLogRus.Add("#");
        DeepListPokePhrase.PassiveLogRus.Add("Прекрати пиздеть!");
        DeepListPokePhrase.PassiveLogRus.Add("Молоко в пакете");
        DeepListPokePhrase.PassiveLogRus.Add("Где рецензия на др. Стоуна?");
        DeepListPokePhrase.PassiveLogRus.Add("Ты змея очковая, и дело не в очках!");
        DeepListPokePhrase.PassiveLogRus.Add("Тыква. Выква. Мыква."); 
        DeepListPokePhrase.PassiveLogRus.Add("Крисп.");
        DeepListPokePhrase.PassiveLogRus.Add("сложно быть гением в дебилтернете"); 
        DeepListPokePhrase.PassiveLogRus.Add("Из Загары вышел хороший тиммейт");  
        DeepListPokePhrase.PassiveLogRus.Add("Я проиграл");  
        DeepListPokePhrase.PassiveLogRus.Add("Large wave of angry zergs");  
        DeepListPokePhrase.PassiveLogRus.Add("LeCringe");  
        DeepListPokePhrase.PassiveLogRus.Add("Кто **Там**?"); 
        DeepListPokePhrase.PassiveLogRus.Add("Дэусманс, придурки"); 
        DeepListPokePhrase.PassiveLogRus.Add("I hate Sirinokses!"); 
        DeepListPokePhrase.PassiveLogRus.Add("Надеваю кепку."); 
        DeepListPokePhrase.PassiveLogRus.Add("I DC'd again."); 
        DeepListPokePhrase.PassiveLogRus.Add("Mit*SUKI*"); 
        DeepListPokePhrase.PassiveLogRus.Add("Почему гиены смеются? Потому что смешно!"); 
        DeepListPokePhrase.PassiveLogRus.Add("**Лол** коронный, **Кек** похоронный"); 
        DeepListPokePhrase.PassiveLogRus.Add("Витамин C содержится в курице и __**БОБАХ**__"); 
        DeepListPokePhrase.PassiveLogRus.Add("Руки-щуки"); 

        

        DeepListDoubtfulTacticPhrase.PassiveLogRus.Add("Everything is going according to my plan");
        DeepListDoubtfulTacticPhrase.PassiveLogRus.Add("My superior tactic will win");
        DeepListDoubtfulTacticPhrase.PassiveLogRus.Add("Я всё рассчитал, это работает.");
        DeepListDoubtfulTacticPhrase.PassiveLogRus.Add("Napoleon Wonnafcuk");
        DeepListDoubtfulTacticPhrase.PassiveLogRus.Add("Техника скрытого Листа - Гамбит");
        

        DeepListDoubtfulTacticFirstLostPhrase.PassiveLogRus.Add("Скоро...");
        DeepListDoubtfulTacticFirstLostPhrase.PassiveLogRus.Add("Тише едешь, дальше будешь");
        DeepListDoubtfulTacticFirstLostPhrase.PassiveLogRus.Add("Я называю это \"Гамбит\"");
        DeepListDoubtfulTacticFirstLostPhrase.PassiveLogRus.Add("Привилегия даумондов");
        DeepListDoubtfulTacticFirstLostPhrase.PassiveLogRus.Add("Это лишь часть моего плана");
        DeepListDoubtfulTacticFirstLostPhrase.PassiveLogRus.Add("На нашей стороне ТЕОРИЯ НЕВЕРОЯТНОСТИ!");
        DeepListDoubtfulTacticFirstLostPhrase.PassiveLogRus.Add("Вижу хуету и я в деле!");
        


        DeepListSuperMindPhrase.PassiveLogRus.Add("Поделив энтропию на ноль, вы поняли, что ");
        DeepListSuperMindPhrase.PassiveLogRus.Add("Используя свою дедукцию, вы поняли, что ");
        DeepListSuperMindPhrase.PassiveLogRus.Add("Сложив 2+2, вы каким-то чудом догадались, что ");
        DeepListSuperMindPhrase.PassiveLogRus.Add("Всё ясно. Героин - Марихуана, а ");
        DeepListSuperMindPhrase.PassiveLogRus.Add("Враг спалился! Но я это понял еще давно. ");
        DeepListSuperMindPhrase.PassiveLogRus.Add("Пст... у меня есть информация, что ");
        DeepListSuperMindPhrase.PassiveLogRus.Add("Здравствуйте, я ваш заменитель мозга. И кстати, ");


        MylorikRevengeLostPhrase.PassiveLogRus.Add("Ах вы суки блять!");
        MylorikRevengeLostPhrase.PassiveLogRus.Add("МММХ!");
        MylorikRevengeLostPhrase.PassiveLogRus.Add("ПРРРРРРУ");
        MylorikRevengeLostPhrase.PassiveLogRus.Add("Понерфайте!");
        MylorikRevengeLostPhrase.PassiveLogRus.Add("РАААЗЪЫБУ!");
        MylorikRevengeLostPhrase.PassiveLogRus.Add("Понерфайте!");
        MylorikRevengeLostPhrase.PassiveLogRus.Add("Я принял решение");

        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("ЭТО СПАРТА");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("Показал как надо");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("ММ!");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("Слабый персонаж!");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("Лезу");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("Го вирт");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("Молоко в пакете");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("Алексич... я твой фанат... Я Пантеон.");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("Берём.");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("Вьетнамская тристана!");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("Вьетнамская тристана!!!");
        MylorikRevengeVictoryPhrase.PassiveLogRus.Add("ВЬЕТНАМСКАЯ ТРИСТАНА!!!");

        
        
        MylorikSpanishPhrase.PassiveLogRus.Add("Ааарива!");
        MylorikSpanishPhrase.PassiveLogRus.Add("Буэнос ночес!");
        MylorikSpanishPhrase.PassiveLogRus.Add("Ale handro!");
        MylorikSpanishPhrase.PassiveLogRus.Add("noob team");
        MylorikBoolePhrase.PassiveLogRus.Add("Буль");
        MylorikBoolePhrase.PassiveLogRus.Add("Там... **Кенч**");
        MylorikBoolePhrase.PassiveLogRus.Add("А вы заметили? Айсика научилась играть...");


        GlebChallengerPhrase.PassiveLogRus.Add("А? БАРОН?!");
        GlebChallengerPhrase.PassiveLogRus.Add("Ща я покажу как надо");
        GlebChallengerPhrase.PassiveLogRus.Add("Глебка залетает!");
        GlebChallengerPhrase.PassiveLogRus.Add("В Претендентмобиль!");
        GlebChallengerPhrase.PassiveLogRus.Add("ЛИИИИРОЙ ДЖЕНКИНС");
        GlebChallengerPhrase.PassiveLogRus.Add("FLASH IN THE NIGhT! Я как вспышка в ночи!");


        GlebChallengerSeparatePhrase.PassiveLogRus.Add("FULL POWER!!!");
        GlebChallengerSeparatePhrase.PassiveLogRus.Add("OVER 9000");
        GlebChallengerSeparatePhrase.PassiveLogRus.Add("JA - PRETENDENT!");
        GlebChallengerSeparatePhrase.PassiveLogRus.Add("IT'S NASHORS TIME!");
        GlebChallengerSeparatePhrase.PassiveLogRus.Add("NORMALNO. U MENJA EST HEAL!");
        GlebChallengerSeparatePhrase.PassiveLogRus.Add("IGRAEM IGRAEM!");
        GlebChallengerSeparatePhrase.PassiveLogRus.Add("DOMOJ GAMBIT!");
        GlebChallengerSeparatePhrase.PassiveLogRus.Add("POSTAVTE WARD!");
        

        GlebSleepyPhrase.PassiveLogRus.Add("Zzzz...");
        GlebSleepyPhrase.PassiveLogRus.Add("Я... Я тут...");
        GlebSleepyPhrase.PassiveLogRus.Add("Только не делайте ремейк *Зевнул*");
        GlebComeBackPhrase.PassiveLogRus.Add("5 минут");
        GlebComeBackPhrase.PassiveLogRus.Add("Я щас приду");
        GlebComeBackPhrase.PassiveLogRus.Add("Без меня не начина...");
        GlebTeaPhrase.PassiveLogRus.Add("Какао белого цвета");
        GlebTeaReadyPhrase.PassiveLogRus.Add("U menja est' lishnij chaj!__");
        GlebTeaReadyPhrase.PassiveLogRus.Add("Chaj zavarilsa!__");


        YongGlebIrelia.PassiveLogRus.Add("Blja opat' mou ireliu ponerfali. Nu ni4ego, glebka otbIgraetsa.");
        YongGlebIrelia.PassiveLogRus.Add("VsmbIsle? Skolko mozno nerfat' ee? Eto zhe celbIh");
        YongGlebIrelia.PassiveLogRus.Add("Rioti sovsem poputali y mojei malishki zabrali");
        YongGlebIrelia.PassiveLogRus.Add("Da kak tak 4alanger brat' kogday personazha");
        YongGlebCommunication.PassiveLogRus.Add("Otli4no, postavil ward. Glebka carry");
        YongGlebCommunicationReady.PassiveLogRus.Add("__Ja kupil communication__");
        YongGlebMeta.PassiveLogRus.Add("Delajem objactivs!");
        YongGlebMeta.PassiveLogRus.Add("Igraem ot objectov pasanbI");
        YongGlebMeta.PassiveLogRus.Add("Igraem igraem");
        YongGlebMeta.PassiveLogRus.Add("4alanger sam sebja ne apnet!!!!");
        YongGlebMeta.PassiveLogRus.Add("viigrivaetsa on objactivs");
        YongGlebMeta.PassiveLogRus.Add("vbI medlennie, nado srazy na objecti! po pervomy cally");
        YongGlebMeta.PassiveLogRus.Add("Ja kak Darien");
        YongGlebMeta.PassiveLogRus.Add("Ja kak __alexich__");
        YongGlebMeta.PassiveLogRus.Add("Ja kak **FLASH IN THE NIGHT**");
        YongGlebMeta.PassiveLogRus.Add("Gleb journey to Challanger");
        YongGlebTea.PassiveLogRus.Add("VbIpij 4au, yspokojsa");
        YongGlebTea.PassiveLogRus.Add("Derji 4aj ne ynbIvai XD XD");
        YongGlebTea.PassiveLogRus.Add("Poslushaj kak vodi4ka l'etsa");
        YongGlebTea.PassiveLogRus.Add("Eto moj lubimbIj 4aj is kitaja");
        YongGlebTea.PassiveLogRus.Add("4aj mne nepius privez");
        YongGlebTeaReady.PassiveLogRus.Add("Chaj zavarilsa!__");


        LeCrispJewPhrase.PassiveLogRus.Add("Я жру деньги!");
        LeCrispJewPhrase.PassiveLogRus.Add("Kill secure"); 
        LeCrispJewPhrase.PassiveLogRus.Add("ОЙ ВЕЙ!");
        LeCrispJewPhrase.PassiveLogRus.Add("Кошерно"); 
        LeCrispJewPhrase.PassiveLogRus.Add("ОЙ ВЕЙ!");     
        LeCrispJewPhrase.PassiveLogRus.Add("Хава Нагила"); 
        LeCrispJewPhrase.PassiveLogRus.Add("Призываю Иуду"); 
        LeCrispJewPhrase.PassiveLogRus.Add("Поднимаю кошелек на улице, а там нехватает"); 


        LeCrispBoolingPhrase.PassiveLogRus.Add("Why are you bulling me?");
        LeCrispBoolingPhrase.PassiveLogRus.Add("fuk u");
        LeCrispBoolingPhrase.PassiveLogRus.Add("У тебя учусь");
        LeCrispBoolingPhrase.PassiveLogRus.Add("DeepList - это заменитель мозга");
        LeCrispBoolingPhrase.PassiveLogRus.Add("Как Адепт");
        
        
        LeCrispAssassinsPhrase.PassiveLogRus.Add("Гребанные ассассины");
        LeCrispAssassinsPhrase.PassiveLogRus.Add("Эх, жизнь АДК");
        LeCrispAssassinsPhrase.PassiveLogRus.Add("Моя жопа не смогла съесть змею. (с)");
        LeCrispAssassinsPhrase.PassiveLogRus.Add("Женская собака приседает и писает");      
        LeCrispAssassinsPhrase.PassiveLogRus.Add("Аль тигий ли ба делет"); 
        LeCrispAssassinsPhrase.PassiveLogRus.Add("Малфит подкинул, Ясуо ультанул..."); 
        LeCrispAssassinsPhrase.PassiveLogRus.Add("Голова арбуза"); 
        LeCrispAssassinsPhrase.PassiveLogRus.Add("Я трахался на летних каникулах. Никому это не понравилось "); 

        
        LeCrispImpactPhrase.PassiveLogRus.Add("Импакт!");
        LeCrispImpactPhrase.PassiveLogRus.Add("Это импакт?");
        LeCrispImpactPhrase.PassiveLogRus.Add("шпещьмен");
        LeCrispImpactPhrase.PassiveLogRus.Add("Забрал кэмп!");
        LeCrispImpactPhrase.PassiveLogRus.Add("Я MVP, Я MVP!");    
        LeCrispImpactPhrase.PassiveLogRus.Add("Хорошее позицианирование");       
        LeCrispImpactPhrase.PassiveLogRus.Add("Это просто навык");                                          
        LeCrispImpactPhrase.PassiveLogRus.Add("Хумус");                                                    
        LeCrispImpactPhrase.PassiveLogRus.Add("Мазальтов!");                                                      
        LeCrispImpactPhrase.PassiveLogRus.Add("Нимфоманка!"); 
        LeCrispImpactPhrase.PassiveLogRus.Add("Я прям вижу как он зонит");                                   
        LeCrispImpactPhrase.PassiveLogRus.Add("Импакт!");
        LeCrispImpactPhrase.PassiveLogRus.Add("Тихо, тихо... Без импакта не стреляй...");
        LeCrispImpactPhrase.PassiveLogRus.Add("Стреляй только когда точно добьешь!");
        LeCrispImpactPhrase.PassiveLogRus.Add("Я хочу чтобы моя девушка была немножко cringe");
        LeCrispImpactPhrase.PassiveLogRus.Add("Как Надо!");
        LeCrispImpactPhrase.PassiveLogRus.Add("OnlyFans... Почему здесь всё стоит денег?");
        LeCrispImpactPhrase.PassiveLogRus.Add("Стреляю редко, но метко");

        

        TolyaJewPhrase.PassiveLogRus.Add("Easy money");
        TolyaJewPhrase.PassiveLogRus.Add("Worth");
        TolyaJewPhrase.PassiveLogRus.Add("Таки кошерно");
        TolyaJewPhrase.PassiveLogRus.Add("Я пока еще не втянулся в эти ваши еврейские схемы");
        TolyaJewPhrase.PassiveLogRus.Add("Хехе, спиздил");
        TolyaJewPhrase.PassiveLogRus.Add("Одна медная!");
        TolyaJewPhrase.PassiveLogRus.Add("Ыыыы");
        TolyaJewPhrase.PassiveLogRus.Add("Настоящий еврей знает свой толк");        

        TolyaCountReadyPhrase.PassiveLogRus.Add("Я готов это просчитать__");
        TolyaCountPhrase.PassiveLogRus.Add("Ха! Подстчет!");
        TolyaCountPhrase.PassiveLogRus.Add("Так и знал!");
        TolyaCountPhrase.PassiveLogRus.Add("Предиктед");
        TolyaCountPhrase.PassiveLogRus.Add("Справедливо"); 
        TolyaCountPhrase.PassiveLogRus.Add("Для победы мне нужны: листочек, еврейсто и ручка"); 
        
        TolyaRammusPhrase.PassiveLogRus.Add("Okay.");
        TolyaRammusPhrase.PassiveLogRus.Add("Hm.");
        TolyaRammusPhrase.PassiveLogRus.Add("Должны, да не обязанны");
        TolyaRammus2Phrase.PassiveLogRus.Add("Я живу и горю");
        TolyaRammus3Phrase.PassiveLogRus.Add("Изи разбайтил");
        TolyaRammus4Phrase.PassiveLogRus.Add("Посмотри сколько я поднял за раз!");
        TolyaRammus5Phrase.PassiveLogRus.Add("Оу, ну всё. ГГ.");


        HardKittyLonelyPhrase.PassiveLogRus.Add("Привет");
        HardKittyLonelyPhrase.PassiveLogRus.Add("Мне сегодня снилось, что...");
        HardKittyLonelyPhrase.PassiveLogRus.Add("Что делаешь?");
        HardKittyLonelyPhrase.PassiveLogRus.Add("Как дела?");
        HardKittyDoebatsyaPhrase.PassiveLogRus.Add("У вас продают молоко в пакетах?");
        HardKittyDoebatsyaPhrase.PassiveLogRus.Add("Какой у тебя Windows?");
        HardKittyDoebatsyaPhrase.PassiveLogRus.Add("Что лучше взять на MF?");
        HardKittyDoebatsyaPhrase.PassiveLogRus.Add("Эй, э-эй...");
        HardKittyDoebatsyaPhrase.PassiveLogRus.Add("Как работает твой бот?");
        HardKittyDoebatsyaLovePhrase.PassiveLogRus.Add("**ЭТО МОЕ ГОВНИЩЕ**, я доебывал его всю свою жизнь...");

        HardKittyDoebatsyaAnswerPhrase.PassiveLogRus.Add("Вам ответили на письмо!");
        HardKittyMutedPhrase.PassiveLogRus.Add("Muted");


        SirinoksFriendsPhrase.PassiveLogRus.Add("Го в пати");
        SirinoksFriendsPhrase.PassiveLogRus.Add("М/Ж?");
        SirinoksFriendsPhrase.PassiveLogRus.Add("Я проиграл");
        SirinoksFriendsPhrase.PassiveLogRus.Add("Пепега");
        SirinoksFriendsPhrase.PassiveLogRus.Add("Invite");


        SirinoksDragonPhrase.PassiveLogRus.Add("ROAR!");
        SirinoksDragonPhrase.PassiveLogRus.Add("А НУ, МОРДА. ROAR");
        SirinoksDragonPhrase.PassiveLogRus.Add("НИКАКОГО NSFW НА СТРИМЕ! ROAR");
        SirinoksDragonPhrase.PassiveLogRus.Add("ГДЕ БЛЯТЬ ГЛЕБ!?!?! ROAR");
        SirinoksDragonPhrase.PassiveLogRus.Add("ОПЯТЬ ЛИСТ ХУЙНЮ ГОВОРИТ! ROAR");
        SirinoksDragonPhrase.PassiveLogRus.Add("ЛОРИК НИЧЕГО НЕ ПОНИМАЕТ! ROAR");
        SirinoksDragonPhrase.PassiveLogRus.Add("БЛЯТЬ КРИИИИСП! ROAR");
        SirinoksDragonPhrase.PassiveLogRus.Add("You can fly in a push of a button!");
        
        

        MitsukiCheekyBriki.PassiveLogRus.Add("Ну чё, ёпта, поиграем?");
        MitsukiTooMuchFucking.PassiveLogRus.Add("Алмаз!");
        MitsukiTooMuchFucking.PassiveLogRus.Add("Ну так-то всё правильно");
        MitsukiTooMuchFucking.PassiveLogRus.Add("Сука ня");
        MitsukiTooMuchFucking.PassiveLogRus.Add("Чё пацаны, аниме?");
        MitsukiTooMuchFucking.PassiveLogRus.Add("Наслаждайтесь жижей!");
        MitsukiTooMuchFuckingNoAttack.PassiveLogRus.Add("Ну че, зассали, суки?");
        MitsukiTooMuchFuckingNoAttack.PassiveLogRus.Add("Кто на меня?");
        MitsukiTooMuchFuckingNoAttack.PassiveLogRus.Add("Пвп или зассал?");
        MitsukiGarbageSmell.PassiveLogRus.Add("Во что это я вляпался?");
        MitsukiGarbageSmell.PassiveLogRus.Add("Запахло мусором...");
        MitsukiSchoolboy.PassiveLogRus.Add("Да блять, своего компьютера-то нету");


        AwdkaTeachToPlay.PassiveLogRus.Add("Научите играть");
        AwdkaTeachToPlay.PassiveLogRus.Add("Я ничего не умею");
        AwdkaTeachToPlay.PassiveLogRus.Add("Я бесполезный");
        AwdkaTeachToPlay.PassiveLogRus.Add("Кстати, я хач.");
        AwdkaTeachToPlay.PassiveLogRus.Add("Как это работает?");
        AwdkaTeachToPlay.PassiveLogRus.Add("Киберспорт!");
        AwdkaTeachToPlay.PassiveLogRus.Add("Что это?");
        AwdkaTeachToPlay.PassiveLogRus.Add("Как там нажимать?");
        AwdkaTeachToPlay.PassiveLogRus.Add("Два игнайта на вуконга!");
        AwdkaTrolling.PassiveLogRus.Add("Произошел троллинг");
        AwdkaTrolling.PassiveLogRus.Add("Ого... Это как?");
        AwdkaTrolling.PassiveLogRus.Add("Что я сделаль?");
        AwdkaTrying.PassiveLogRus.Add("Сапаги сораходы! Дёшево!");
        AwdkaTrying.PassiveLogRus.Add("Флеш-ульт в клона лебланки");
        AwdkaTrying.PassiveLogRus.Add("Что начинает получаться");
        AwdkaTrying.PassiveLogRus.Add("Почти...");
        AwdkaTrying.PassiveLogRus.Add("В киберспорт!");
        AwdkaAfk.PassiveLogRus.Add("AFKA");


        CraboRackSidewaysBoolePhrase.PassiveLogRus.Add("У них одни ренжевики!");
        CraboRackSidewaysBoolePhrase.PassiveLogRus.Add("На Полому!");
        CraboRackSidewaysBoolePhrase.PassiveLogRus.Add("*Кручусь на месте*");

        WeedwickRuthlessHunter.PassiveLogRus.Add("От меня не спрячешься");
        WeedwickRuthlessHunter.PassiveLogRus.Add("ВУФ!");
        WeedwickRuthlessHunter.PassiveLogRus.Add("Псевдособаааакааа");
        WeedwickRuthlessHunter.PassiveLogRus.Add("Что за запах..?");

        WeedwickValuablePreyPoints1.PassiveLogRus.Add("SHOTDDDDDDOWN");
        WeedwickValuablePreyPoints2.PassiveLogRus.Add("ГОВОРЯЩАЯ СОБАКА");
        WeedwickValuablePreyPoints3.PassiveLogRus.Add("ГАФ");
        WeedwickValuablePreyPoints4.PassiveLogRus.Add("ОМЕГА-ГАФ");
        WeedwickValuablePreyPoints5.PassiveLogRus.Add("АльфоВолк!");
        WeedwickValuablePreyPoints6.PassiveLogRus.Add("СААААМАЯ БОГАТАЯ ПСЕВДОСОБАКА");
        WeedwickValuablePreyPoints7.PassiveLogRus.Add("Не употребляйте наркотики");

        WeedwickValuablePreyDrop.PassiveLogRus.Add("БЛЯТЬ, СУКА, СОБАКА! АУУУУУУУУУФ! СОБАКАСОБАКАСОБАКА!!!");
        WeedwickValuablePreyDrop.PassiveLogRus.Add("Я - Шиза воплоти!");
        WeedwickValuablePreyDrop.PassiveLogRus.Add("На нахуй!");
        WeedwickValuablePreyDrop.PassiveLogRus.Add("Спускайся вниз!");
        WeedwickValuablePreyDrop.PassiveLogRus.Add("ГАФГАФГАФ РРРРРРРР");
        WeedwickValuablePreyDrop.PassiveLogRus.Add("Слезай с дерева!!!");
        WeedwickValuablePreyDrop.PassiveLogRus.Add("Биологи, почему собаки смеются? Потому что смешно!");
        WeedwickValuablePreyDrop.PassiveLogRus.Add("Я смеюсь над тобой!");
        WeedwickValuablePreyDrop.PassiveLogRus.Add("Покусаю, покусаю! Напрыгну, укушу!");



        WeedwickWeedYes1.PassiveLogRus.Add("Weed");
        WeedwickWeedYes2.PassiveLogRus.Add("Вуф");
        WeedwickWeedYes3.PassiveLogRus.Add("Авуууу!!!");
        WeedwickWeedYes4.PassiveLogRus.Add("СКУБИ-ДУБИ-ДУ-УУ!");
        WeedwickWeedYes5.PassiveLogRus.Add("WEED!");
        WeedwickWeedYes6.PassiveLogRus.Add("WEEEEEED!!!");
        WeedwickWeedYes7.PassiveLogRus.Add("SWA$$GG");
        WeedwickWeedYes8.PassiveLogRus.Add("D_O_double.G");
        WeedwickWeedYes9.PassiveLogRus.Add("420");
        WeedwickWeedYes10.PassiveLogRus.Add("81@23 !7");
        WeedwickWeedYes11.PassiveLogRus.Add("Я выбиваю из них ДУРЬ");

        WeedwickWeedNo.PassiveLogRus.Add("ГДЕ МОЙ WEED СУКИ?");
        WeedwickWeedNo.PassiveLogRus.Add("ГДЕ ШО");
        WeedwickWeedNo.PassiveLogRus.Add("228 папиросим, НО ГДЕ???");

        KratosEventYes.PassiveLogRus.Add("If all of Olympus will deny me my vengeance, then all of Olympus **will die**!");
        KratosEventNo.PassiveLogRus.Add("My vengeance ends now!");
        KratosEventFailed.PassiveLogRus.Add("AAAAAAAAAAAAAAAAAAAAAAAAAAA");

        KratosEventKill.PassiveLogRus.Add("Я вернулся из мертвых, чтобы подняться на олимп!");
        KratosEventKill.PassiveLogRus.Add("Зевс, твой сын идет за тобой!");
        KratosEventKill.PassiveLogRus.Add("Я не остановлюсь. Скоро атеисты окажутся правы!");
        KratosEventKill.PassiveLogRus.Add("Еще один....");
        KratosEventKill.PassiveLogRus.Add("Теперь останется лишь один бог! Вот и всё.");

        KratosTarget.PassiveLogRus.Add("О, ебать, лутбокс Пандоры!");
        KratosTarget.PassiveLogRus.Add("Гораздо круче Ареса");
        KratosTarget.PassiveLogRus.Add("Ненавижу богов!");
        KratosTarget.PassiveLogRus.Add("Сука, еще один бог");
        KratosTarget.PassiveLogRus.Add("Куда подевали мою семью? МММ????");
        KratosTarget.PassiveLogRus.Add("Я буду мстить");
        KratosTarget.PassiveLogRus.Add("Ща как покажу!");
        KratosTarget.PassiveLogRus.Add("The power to creater the power to creater the power to destroy!");
        KratosTarget.PassiveLogRus.Add("Призрак Спарты в заброшенном доме... Буууу!");

        //end
    }


    //class needed to send unique logs.
    public class PhraseClass
    {
        public List<string> PassiveLogEng = new();
        public List<string> PassiveLogRus = new();
        public string PassiveNameEng;
        public string PassiveNameRus;

        private int Random(int minValue, int maxValue)
        {
            maxValue += 1;
            if (minValue == maxValue) return minValue;
            if (minValue > maxValue)
                throw new ArgumentOutOfRangeException($"{nameof(minValue)} must be lower than {nameof(maxValue)}");

            var diff = (long)maxValue - minValue;
            var upperBound = uint.MaxValue / diff * diff;

            uint ui;
            do
            {
                var randomBytes = RandomNumberGenerator.GetBytes(555);
                ui = BitConverter.ToUInt32(randomBytes, 0);
            } while (ui >= upperBound);

            var result = (int)(minValue + ui % diff);
            return result;
        }

        public PhraseClass(string passiveNameRus, string passiveNameEng = "")
        {
            PassiveNameRus = passiveNameRus;
            PassiveNameEng = passiveNameEng;
        }

        public void SendLog(GamePlayerBridgeClass player, bool delete, string prefix = "", bool isRandomOrder = true, string suffix = "")
        {
            var description = PassiveLogRus[Random(0, PassiveLogRus.Count-1)];
            if (!isRandomOrder)
            {
                description = PassiveLogRus.First();
            }

            if (delete)
            {
                if (PassiveLogRus.Count > 1)
                    PassiveLogRus.Remove(description);
            }
            else
            {
                var personalLogs = player.Status.GetInGamePersonalLogs();
                var i = 0;
                while (i < 20)
                {
                    i++;
                    if (!personalLogs.Contains(description))
                        break;
                    description = PassiveLogRus[Random(0, PassiveLogRus.Count-1)];

                    if (!isRandomOrder)
                    {
                        description = PassiveLogRus.First();
                    }
                }
            }

            player.Status.AddInGamePersonalLogs($"|>Phrase<|{PassiveNameRus}: {prefix}{description}{suffix}\n");
        }

        public void SendLog(GamePlayerBridgeClass player, GamePlayerBridgeClass player2, bool delete, bool isRandomOrder = true)
        {
            var description = PassiveLogRus[Random(0, PassiveLogRus.Count-1)];
            if (!isRandomOrder)
            {
                description = PassiveLogRus.First();
            }
            if (delete)
            {
                if (PassiveLogRus.Count > 1)
                    PassiveLogRus.Remove(description);
            }
            else
            {
                var personalLogs = player.Status.GetInGamePersonalLogs();
                var i = 0;
                while (i < 20)
                {
                    i++;
                    if (!personalLogs.Contains(description))
                        break;
                    description = PassiveLogRus[Random(0, PassiveLogRus.Count-1)];
                    if (!isRandomOrder)
                    {
                        description = PassiveLogRus.First();
                    }
                }
            }

            description += $"{player2.DiscordUsername} - {player2.GameCharacter.Name}";


            player.Status.AddInGamePersonalLogs($"|>Phrase<|{PassiveNameRus}: {description}\n");
        }

        public async Task SendLogSeparate(GamePlayerBridgeClass player, bool delete, int delayMs, bool isRandomOrder = true)
        {
            if (player.IsBot()) return;

            var description = PassiveLogRus[Random(0, PassiveLogRus.Count-1)];
            if (!isRandomOrder)
            {
                description = PassiveLogRus.First();
            }
            if (delete)
            {
                if (PassiveLogRus.Count > 1)
                    PassiveLogRus.Remove(description);
            }
            else
            {
                var personalLogs = player.Status.GetInGamePersonalLogs();
                var i = 0;
                while (i < 20)
                {
                    i++;
                    if (!personalLogs.Contains(description))
                        break;
                    description = PassiveLogRus[Random(0, PassiveLogRus.Count-1)];
                    if (!isRandomOrder)
                    {
                        description = PassiveLogRus.First();
                    }
                }
            }


            if (PassiveLogRus.Count > 1)
                PassiveLogRus.Remove(description);
            try
            {
                var mess2 = await player.DiscordStatus.SocketGameMessage.Channel.SendMessageAsync(description);
                player.DeleteMessages.Add(new GamePlayerBridgeClass.DeleteMessagesClass(mess2.Id, delayMs));
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                Console.Write(exception.StackTrace);
            }
        }

        public async Task SendLogSeparateWithFile(GamePlayerBridgeClass player, bool delete, string filePath, bool clearNextRound, int delayMs, bool isRandomOrder = true)
        {
            if (player.IsBot()) return;

            var description = PassiveLogRus[Random(0, PassiveLogRus.Count - 1)];
            if (!isRandomOrder)
            {
                description = PassiveLogRus.First();
            }
            if (delete)
            {
                if (PassiveLogRus.Count > 1)
                    PassiveLogRus.Remove(description);
            }
            else
            {
                var personalLogs = player.Status.GetInGamePersonalLogs();
                var i = 0;
                while (i < 20)
                {
                    i++;
                    if (!personalLogs.Contains(description))
                        break;
                    description = PassiveLogRus[Random(0, PassiveLogRus.Count - 1)];
                    if (!isRandomOrder)
                    {
                        description = PassiveLogRus.First();
                    }
                }
            }


            if (PassiveLogRus.Count > 1)
                PassiveLogRus.Remove(description);
            try
            {
                //.SendFileAsync($"DataBase/sound/Kratos_PLAY_ME.mp3", "123");
                var mess2 = await player.DiscordStatus.SocketGameMessage.Channel.SendFileAsync(filePath, description);
                if (clearNextRound)
                    player.DeleteMessages.Add(new GamePlayerBridgeClass.DeleteMessagesClass(mess2.Id, delayMs));
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
                Console.Write(exception.StackTrace);
            }
        }
    }
}
