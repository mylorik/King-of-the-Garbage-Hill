﻿using System;
using System.Collections.Generic;

namespace King_of_the_Garbage_Hill.Game.Characters;

public class DeepList
{
    public class Mockery
    {
        public ulong GameId;
        public Guid PlayerId;
        public List<MockerySub> WhoWonTimes;

        public Mockery(List<MockerySub> whoWonTimes, ulong gameId, Guid playerId)
        {
            WhoWonTimes = whoWonTimes;
            GameId = gameId;
            PlayerId = playerId;
        }
    }

    public class MockerySub
    {
        public Guid EnemyPlayerId;
        public int Times;
        public bool Triggered;

        public MockerySub(Guid enemyPlayerId, int times)
        {
            EnemyPlayerId = enemyPlayerId;
            Times = times;
            Triggered = false;
        }
    }

    public class SuperMindKnown
    {
        public ulong GameId;
        public List<Guid> KnownPlayers = new();
        public Guid PlayerId;

        public SuperMindKnown(Guid playerId, ulong gameId, Guid player2Id)
        {
            PlayerId = playerId;
            GameId = gameId;
            KnownPlayers.Add(player2Id);
        }
    }

    public class Madness
    {
        public ulong GameId;
        public List<MadnessSub> MadnessList = new();
        public Guid PlayerId;
        public int RoundItTriggered;

        public Madness(Guid playerId, ulong gameId, int roundItTriggered)
        {
            PlayerId = playerId;
            GameId = gameId;
            RoundItTriggered = roundItTriggered;
        }
    }

    public class MadnessSub
    {
        public int Index;
        public int Intel;
        public int Psyche;
        public int Speed;
        public int Str;

        public MadnessSub(int index, int intel, int str, int speed, int psyche)
        {
            Index = index;
            Intel = intel;
            Str = str;
            Speed = speed;
            Psyche = psyche;
        }
    }
}