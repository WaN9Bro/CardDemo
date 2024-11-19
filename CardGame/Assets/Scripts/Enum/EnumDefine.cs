using System;

namespace MyGame
{
    public enum EAttackType
    {
        Near,
        Far,
    }
    
    public enum EBattleState
    {
        None = 0,
        Running = 1,
        Paused = 2,
        Ended = 3,
    }

    [Serializable]
    public enum EFaction
    {
        Player = 0,
        Enemy = 1,
    }
}