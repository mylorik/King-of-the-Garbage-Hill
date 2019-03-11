﻿using Discord;

namespace King_of_the_Garbage_Hill.Game.Classes
{
    public class InGameStatus
    {
        public InGameStatus()
        {
            MoveListPage = 1;
            SocketMessageFromBot = null;
            Score = 0;
            IsBlock = false;
            IsAbleToTurn = true;
            PlaceAtLeaderBoard = 0;
            WhoToAttackThisTurn = 0;

            IsReady = false;
            IsAbleToWin = true;
            WonTimes = 0;
            IsWonLastTime = 0;
            IsLostLastTime = 0;
            IsSkip = false;
            ScoresToGiveAtEndOfRound = 0;
        }

        public int MoveListPage { get; set; }
        /*
         * 1 = main page ( your character + leaderboard)
         * 2 = Log   
         * 3 = lvlUp     (what stat to update)
         */

        public IUserMessage SocketMessageFromBot { get; set; }

        private int Score { get; set; }
        public bool IsBlock { get; set; }
        public bool IsSkip { get; set; }
        public bool IsAbleToTurn { get; set; }
        public bool IsAbleToWin { get; set; }
        public int PlaceAtLeaderBoard { get; set; }

        public ulong WhoToAttackThisTurn { get; set; }


        public bool IsReady { get; set; }
        public int WonTimes { get; set; }
        public ulong IsWonLastTime { get; set; }
        public ulong IsLostLastTime { get; set; }
        private double ScoresToGiveAtEndOfRound { get; set; }

        public void SetScoresToGiveAtEndOfRound(int score)
        {
            ScoresToGiveAtEndOfRound = score;
        }

        public void AddRegularPoints(double regularPoints)
        {
            ScoresToGiveAtEndOfRound += regularPoints;
        }

        public void AddRegularPoints(int regularPoints)
        {
            ScoresToGiveAtEndOfRound += regularPoints;
        }

        public void AddBonusPoints(int bonusPoints)
        {
            ScoresToGiveAtEndOfRound += bonusPoints;
        }

        public double GetScoresToGiveAtEndOfRound()
        {
            return ScoresToGiveAtEndOfRound;
        }

        public void CombineRoundScoreAndGameScore(int roundNumber)
        {
            AddScore(GetScoresToGiveAtEndOfRound(), roundNumber);
            SetScoresToGiveAtEndOfRound(0);
        }

        private void AddScore(double score, int roundNumber)
        {
            /*
            1-4 х1
            5-9 х2
            10 х4
            */
            if (roundNumber <= 4)
                score = score * 1; // Why????????????????????????
            else if (roundNumber <= 9)
                score = score * 2;
            else if (roundNumber == 10) 
                score = score * 4;

            Score += (int)score;
        }

        public void SetScoreToThisNumber(int score)
        {
            Score = score;
        }

        public int GetScore()
        {
            return Score;
        }
    }
}