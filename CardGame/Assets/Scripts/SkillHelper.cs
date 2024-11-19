using System;

namespace MyGame
{
    public static class SkillHelper
    {
        public static Skill Config(this SkillModel self)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Skill table = tableManager.Tables.TbSkill.Get(self.Id);
            return table;
        }
        
        public static Skill Config(this SkillData self)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Skill table = tableManager.Tables.TbSkill.Get(self.SkillId());
            return table;
        }

        public static int SkillId(this SkillData self)
        {
            string level = self.Id.ToString() + self.Level;
            return Convert.ToInt32(level);
        }
        
        public static SkillModel GetSkillModel(this SkillData self)
        {
            Skill config = self.Config();
            return new SkillModel(config.Id, config.Condition, config.Cost, config.EffectKey,config.EffectValue);
        }

        public static PassiveSkill Config(this PassiveSkillData self)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            PassiveSkill table = tableManager.Tables.TbPassiveSkill.Get(self.SkillId());
            return table;
        }

        public static int SkillId(this PassiveSkillData self)
        {
            return Convert.ToInt32(self.Id.ToString() + self.Level);
        }
        
        public static bool IsAttackSKill(this SkillModel self)
        {
            return IsAttackSKill(self.Id);
        }
        
        public static bool IsAttackSKill(this SkillData self)
        {
            return IsAttackSKill(self.Id);
        }

        private static bool IsAttackSKill(int id)
        {
            long number = id;
            while (number >= 10) // 逐步减少数字
            {
                number /= 10; // 每次除以10
            }

            return (int)number == 9;
        }

        public static HeroHealth ConvertToHeroHealth(this CostResource self)
        {
            return new HeroHealth(self.HP, self.Shield);
        }
    }
}