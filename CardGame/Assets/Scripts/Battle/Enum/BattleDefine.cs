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
    public enum EProfession
    {
        Warrior, // 战士
        Assassin, // 刺客
        Tank,   // 坦克
        Auxiliary,  // 辅助
    }
    
    [Serializable]
    public enum EStanding
    {
        L1,
        L2,
        L3,
        R1,
        R2,
        R3
    }

    [Serializable]
    public enum EFaction
    {
        Player = 0,
        Enemy = 1,
    }
}