using System;
using UnityEngine;


namespace MyGame
{
    public static class HeroHelper
    {
        public static HeroObj CreateHeroObj(HeroData data)
        {
            HeroObj gObj = Resources.Load<HeroObj>("Hero/HeroObj");
            HeroObj heroObj = GameObject.Instantiate(gObj);
            heroObj.name = $"[ID:{data.Id}/UID:{data.Uid}]";
            return heroObj;
        }

        public static GameObject CreateHeroSpine(HeroData data)
        {
            GameObject gObj = Resources.Load<GameObject>("Hero/" + data.Config().Prefab);
            GameObject heroObj = GameObject.Instantiate(gObj);
            return heroObj;
        }

        public static HeroData CreateHeroData(int id,int level = 1)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Hero table = tableManager.Tables.TbHero.Get(id);
            // 唯一UID
            long uid = UniqueIDGenerator.GenerateUniqueID();
            
            // 装备列表
            EquipmentData[] equipmentData = new EquipmentData[Enum.GetValues(typeof(EEquipmetType)).Length];
            
            //技能列表
            SkillData[] skillData = new SkillData[table.SkillGroupIds.Length];
            for (int i = 0; i < skillData.Length; i++)
            {
                skillData[i] = new SkillData(table.SkillGroupIds[i], 1);
            }
            
            //被动技能
            PassiveSkillData[] passiveSkillData = new PassiveSkillData[table.PassiveSkillGroupIds.Length];
            for (int i = 0; i < passiveSkillData.Length; i++)
            {
                passiveSkillData[i] = new PassiveSkillData(table.PassiveSkillGroupIds[i], 1);
            }
            
            HeroData heroData = new HeroData(id,uid,level,equipmentData,skillData,passiveSkillData);
            return heroData;
        }

        public static Hero Config(this HeroData self)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Hero table = tableManager.Tables.TbHero.Get(self.Id);
            return table;
        }
    }
}