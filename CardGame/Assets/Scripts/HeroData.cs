using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyGame
{
    [Serializable]
    public class HeroData
    {
        public int Id;

        public long Uid;
        
        public int Level;

        public EquipmentData[] Equipment;
        
        public SkillData[] Skill;
        
        public PassiveSkillData[] PassiveSkill;
        
        [JsonIgnore]
        public HeroProperty BaseProperty
        {
            get
            {
                //TODO:读配置表内容进行生成
                GameManager.Instance.GetService(out TableManager tableManager);
                Hero table = tableManager.Tables.TbHero.Get(Id);
                
                int hp = table.Hp;
                int attack = table.Attack;
                int pyDef = table.PhysicalDefense;
                int magicDef = table.MagicDefense;
                
                if (Level > 1)
                {
                    hp += (Level - 1) * table.HpDelta;
                    attack += (Level - 1) * table.AttackDelta;
                    pyDef += (Level - 1) * table.PhysicalDefense;
                    magicDef += (Level - 1) * table.MagicDefenseDelta;
                }

                return new HeroProperty(hp, attack, pyDef, magicDef,0,0);
            }
        }
        
        public HeroData(int id, long uid, int level,EquipmentData[] equipmentData,SkillData[] skillData,PassiveSkillData[] passiveSkill)
        {
            Id = id;
            Uid = uid;
            Level = level;
            Equipment = equipmentData;
            Skill = skillData;
            PassiveSkill = passiveSkill;
        }
    }

    [Serializable]
    public class EquipmentData
    {
        public int Id;
        
        public int Level;

        public EquipmentData(int id, int level)
        {
            Id = id;
            Level = level;
        }
    }

    [Serializable]
    public class SkillData
    {
        public int Id;
        
        public int Level;

        public SkillData(int id, int level)
        {
            Id = id;
            Level = level;
        }
    }
    
    [Serializable]
    public class PassiveSkillData
    {
        public int Id;
        
        public int Level;

        public PassiveSkillData(int id, int level)
        {
            Id = id;
            Level = level;
        }
    }
}