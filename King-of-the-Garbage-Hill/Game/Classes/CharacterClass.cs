using System.Collections.Generic;

namespace King_of_the_Garbage_Hill.Game.Classes
{
    public class CharacterClass
    {

        public string Name { get; set; }
        private int Intelligence { get; set; }
        private int Psyche { get; set; }
        private int Speed { get; set; }
        private int Strength { get; set; }
        public JusticeClass Justice { get; set; }
        public string Avatar { get; set; }
        public List<Passive> Passive { get; set; }


        public CharacterClass(int intelligence, int strength, int speed, int psyche, string name)
        {
            Intelligence = intelligence;
            Strength = strength;
            Speed = speed;
            Psyche = psyche;
            Justice = new JusticeClass();
            Name = name;
        }

        public void AddIntelligence(int howMuchToAdd = 1)
        {
            if (Intelligence < 10)
                Intelligence += howMuchToAdd;
            if (Intelligence < 0)
                Intelligence = 0;
        }

        public int GetIntelligence()
        {
            return Intelligence;
        }

        public void SetIntelligence(int newIntelligence)
        {
            Intelligence = newIntelligence;
        }

        public void AddPsyche(int howMuchToAdd = 1)
        {
            if (Psyche < 10)
                Psyche += howMuchToAdd;

            if (Psyche < 0)
                Psyche = 0;
        }

        public int GetPsyche()
        {
            return Psyche;
        }

        public void SetPsyche(int newPsyche)
        {
            Psyche = newPsyche;
        }

        public void AddSpeed(int howMuchToAdd = 1)
        {
            if (Speed < 10)
                Speed += howMuchToAdd;
            if (Speed < 0)
                Speed = 0;
        }

        public int GetSpeed()
        {
            return Speed;
        }

        public void SetSpeed(int newSpeed)
        {
            Speed = newSpeed;
        }

        public void AddStrength(int howMuchToAdd = 1)
        {
            if (Strength < 10)
                Strength += howMuchToAdd;
            if (Strength < 0)
                Strength = 0;
        }

        public int GetStrength()
        {
            return Strength;
        }

        public void SetStrength(int newStrength)
        {
            Strength = newStrength;
        }

    }

    public class Passive
    {
        public string PassiveDescription;
        public string PassiveName;

        public Passive()
        {
            PassiveDescription = "";
            PassiveName = "";
        }

        public Passive(string passiveName, string passiveDescription)
        {
            PassiveDescription = passiveDescription;
            PassiveName = passiveName;
        }
    }

    public class JusticeClass
    {
        private int JusticeNow { get; set; }
        private int JusticeForNextRound { get; set; }
        public bool IsWonThisRound { get; set; }

        public JusticeClass()
        {
            JusticeNow = 0;
            JusticeForNextRound = 0;
            IsWonThisRound = false;
        }

        public void AddJusticeNow(int howMuchToAdd = 1)
        {
            if (JusticeNow < 5)
                JusticeNow += howMuchToAdd;
            if (JusticeNow < 0)
                JusticeNow = 0;
        }

        public int GetJusticeNow()
        {
            return JusticeNow;
        }

        public void SetJusticeNow(int newJusticeNow)
        {
            JusticeNow = newJusticeNow;
        }

       public void AddJusticeForNextRound(int howMuchToAdd = 1)
        {
            if (JusticeForNextRound < 10)
                JusticeForNextRound += howMuchToAdd;

            if (JusticeForNextRound < 0)
                JusticeForNextRound = 0;
        }

        public int GetJusticeForNextRound()
        {
            return JusticeForNextRound;
        }

        public void SetJusticeForNextRound(int newJusticeForNextRound)
        {
            JusticeForNextRound = newJusticeForNextRound;
        }

    }
}