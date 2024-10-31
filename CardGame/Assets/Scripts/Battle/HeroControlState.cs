namespace MyGame
{
    public struct HeroControlState
    {
        public bool CanAttack { get; private set; }

        public bool CanAddAngry{ get; private set; }
        
        public bool CanUseSkill{ get; private set; }
        
        public static HeroControlState Default = new HeroControlState(true, true, true);

        public HeroControlState(bool canAttack, bool canAddAngry, bool canUseSkill)
        {
            CanAttack = canAttack;
            CanAddAngry = canAddAngry;
            CanUseSkill = canUseSkill;
        }
    }
}