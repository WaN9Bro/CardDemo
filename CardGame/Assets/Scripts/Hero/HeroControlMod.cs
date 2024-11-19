namespace MyGame
{
    public struct HeroControlMod
    {
        public bool CanAttack;
        public bool CanUseSkill;
        public bool CanBeHurt;

        public bool CanHeal;

        public static HeroControlMod Default = new HeroControlMod(true, true,true,true);
        
        public HeroControlMod(bool canAttack, bool canUseSkill, bool canBeHurt,bool canHeal)
        {
            CanAttack = canAttack;
            CanUseSkill = canUseSkill;
            CanBeHurt = canBeHurt;
            CanHeal = canHeal;
        }
        
        public static HeroControlMod operator +(HeroControlMod a, HeroControlMod b){
            return new HeroControlMod(
                a.CanAttack & b.CanAttack,
                a.CanUseSkill & b.CanUseSkill,
                a.CanBeHurt & b.CanBeHurt,
                a.CanHeal & b.CanHeal
            );
        }
    }
}