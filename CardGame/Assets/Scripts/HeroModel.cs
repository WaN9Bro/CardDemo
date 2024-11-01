using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyGame
{
    [Serializable]
    public class HeroModel : IReference
    {
        public int Id;

        public int Uid;
        
        public int Level;

        public string Prefab;
        
        public List<EquipmentData> Equipment;
        
        public List<SkillData> Skill;

        [JsonIgnore]
        public HeroProperty BaseProperty
        {
            get
            {
                //TODO:读配置表内容进行生成
                return HeroProperty.Default;
            }
        }

        public static void Create(int id,int level)
        {
            if (GameManager.Instance.GetService(out ConfigManager configManager))
            {
                //TODO：创建英雄表、技能表
            }
        }

        public void Clear()
        {
            
        }
    }

    [Serializable]
    public class EquipmentData
    {
        public int Id;
        
        public int Level;
    }

    public class SkillData
    {
        public int Id;
        
        public int Level;
    }
}