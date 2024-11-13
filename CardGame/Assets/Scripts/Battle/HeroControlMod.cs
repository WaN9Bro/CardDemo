namespace MyGame
{
    public struct HeroControlMod
    {
        public bool CanAttack { get; private set; }
        public bool CanUseSkill{ get; private set; }

        public bool CanDead { get; private set; }
        
        public bool CanBeHurt { get; private set; }

        public static HeroControlMod Default = new HeroControlMod(true, true, true,true);
        
        public HeroControlMod(bool canAttack, bool canUseSkill, bool canDead, bool canBeHurt)
        {
            CanAttack = canAttack;
            CanUseSkill = canUseSkill;
            CanDead = canDead;
            CanBeHurt = canBeHurt;
        }
    }
}