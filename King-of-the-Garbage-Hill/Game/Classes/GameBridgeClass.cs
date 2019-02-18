﻿

using King_of_the_Garbage_Hill.LocalPersistentData.UsersAccounts;

namespace King_of_the_Garbage_Hill.Game.Classes
{
  public  class GameBridgeClass
  {
      public AccountSettings Account { get; set; }
      public CharacterClass Character { get; set; }

      public InGameStatus Status { get; set; }
  }
}
