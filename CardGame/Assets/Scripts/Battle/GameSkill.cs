namespace MyGame
{
    public class SkillObj
    {
        public SkillModel Model { get; private set; }
        
        public int Level { get; private set; }

        public SkillObj(SkillModel model, int level = 1)
        {
            Model = model;
            Level = level;
        }
    }

    public struct SkillModel
    {
        public string Id { get; private set; }
        
        public SkillCastCondition Condition { get; private set; }
        
        public HeroResource Cost { get; private set; }
        
        public TimelineModel Effect { get; private set; }
        
        public AddBuffInfo[] AddBuffs { get; private set; }

        public SkillModel(string id, SkillCastCondition condition, HeroResource cost, string effect, AddBuffInfo[] addBuffs)
        {
            Id = id;
            Condition = condition;
            Cost = cost;
            Effect = string.IsNullOrEmpty(effect) ? TimelineModel.Default : Data.DesignerTables.Timeline.Data[effect];
            AddBuffs = addBuffs;
        }
    }
}