using Cysharp.Threading.Tasks;

namespace MyGame
{
    public class HeroBattleCom : IHeroComponent
    {
        public HeroObj HeroObj { get; private set; }
        
        private Faction _selfFaction;
        private Faction _otherFaction;
        private IReference _referenceImplementation;

        public void Initialize(HeroObj heroObj)
        {
            HeroObj = heroObj;
        }
        public async UniTask StartBattle(Faction selfFaction, Faction otherFaction = null)
        {
            _selfFaction = selfFaction;
            _otherFaction = otherFaction;
            // 要么是普攻、 要么是放技能
            // 首先是技能释放判断。没有可以释放的技能，就进入普攻完结
            HeroObj.SkillCom.CastSkill();
        }

        public void Inactive()
        {
            
        }

        public void BeHit(HeroObj attacker)
        {
            
        }

        public void Attack(HeroObj target)
        {
            
        }

        public void OnDeath(HeroObj attacker)
        {
            
        }

        public void Idle()
        {
            
        }

        public void Clear()
        {
            
        }
    }
}