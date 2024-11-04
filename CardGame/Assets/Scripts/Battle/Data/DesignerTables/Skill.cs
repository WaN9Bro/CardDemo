using System.Collections.Generic;

namespace MyGame.Data.DesignerTables
{
    public class Skill
    {
        public static Dictionary<string, SkillModel> Data = new Dictionary<string, SkillModel>
        {
            {"Attack",new SkillModel("Attack",SkillCastCondition.Default,HeroResource.Normal,"Attack",
                new AddBuffInfo[] { }
                
            )}
        };
    }
}