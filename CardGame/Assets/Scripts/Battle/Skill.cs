namespace MyGame
{
    public class SkillObj : IReference
    {
        public SkillModel Model { get; private set; }

        private int _isAttackSkill = -1;
        public bool IsAttackSkill
        {
            get
            {
                if (_isAttackSkill == -1)
                {
                    _isAttackSkill = Model.IsAttackSKill() ? 1 : 0;
                }

                return _isAttackSkill == 1;
            }
        }
        
        public void Init(SkillModel model)
        {
            Model = model;
        }

        public void Clear()
        {
            Model = null;
        }
    }

    public class SkillModel
    {
        public int Id { get; private set; }
        
        public CastCondition Condition { get; private set; }
        
        public CostResource Cost { get; private set; }
        
        public TimelineModel Effect { get; private set; }
        

        public SkillModel(int id, CastCondition condition, CostResource cost, string effect,EventWarp[] effectValue)
        {
            Id = id;
            Condition = condition;
            Cost = cost;
            Effect = string.IsNullOrEmpty(effect) ? default : TimelineHelper.GetTimelineModel(effect,effectValue);
        }
    }
}