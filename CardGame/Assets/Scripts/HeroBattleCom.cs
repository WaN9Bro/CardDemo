namespace MyGame
{
    public class HeroBattleCom : IHeroComponent
    {
        public HeroObj HeroObj { get; private set; }
        public bool IsActive { get; private set; }
        
        private GameManager _gameManager;

        private Faction _selfFaction;
        private Faction _otherFaction;
        
        public void Initialize(HeroObj heroObj, GameManager gameManager)
        {
            HeroObj = heroObj;
            _gameManager = gameManager;
        }

        public void CleanUp()
        {
            
        }

        public void Active(Faction selfFaction, Faction otherFaction = null)
        {
            if (IsActive) return;
            IsActive = true;
            _selfFaction = selfFaction;
            _otherFaction = otherFaction;
            
            // 根据玩家的情况
            // 突（入场时有高额怒气）、攻（普攻回复怒气）、防（收到普攻后回复怒气）、辅（友方普攻后回复怒气）
            // 首先判断自己的怒气
            // 如果是突，
            // 当前血量、当前护盾、当前怒气
            
            
            HeroObj.SkillComponent.
            
            
            
            
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
    }
}