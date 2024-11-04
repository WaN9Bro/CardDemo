﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyGame
{
    [Serializable]
    public class HeroModel : IReference
    {
        public int Id;

        public long Uid;
        
        public int Level;

        public EquipmentData[] Equipment;
        
        public SkillData[] Skill;
        
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

                return new HeroProperty(hp, attack, pyDef, magicDef);
            }
        }
        
        public HeroModel(int id, long uid, int level,EquipmentData[] equipmentData,SkillData[] skillData)
        {
            Id = id;
            Uid = uid;
            Level = level;
            Equipment = equipmentData;
            Skill = skillData;
        }

        public static HeroModel Create(int id,int level)
        {
            if (GameManager.Instance.GetService(out TableManager tableManager))
            {
                Hero table = tableManager.Tables.TbHero.Get(id);
                // 唯一UID
                long uid = UniqueIDGenerator.GenerateUniqueID();
                // 装备列表
                EquipmentData[] equipmentData = new EquipmentData[Enum.GetValues(typeof(EEquipmetType)).Length];
                //技能列表
                SkillData[] skillData = new SkillData[table.SkillIds.Length];
                for (int i = 0; i < skillData.Length; i++)
                {
                    skillData[i] = new SkillData(table.SkillIds[i], 1);
                }
                HeroModel heroModel = new HeroModel(id,uid,level,equipmentData,skillData);
                return heroModel;
            }

            throw new Exception("[HeroModel] invalid tableManager.");
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
}