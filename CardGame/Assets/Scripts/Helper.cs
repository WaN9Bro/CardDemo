using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame
{
    public static class Helper
    {
        public static HeroObj CreateHeroObj(HeroData data)
        {
            HeroObj gObj = Resources.Load<HeroObj>(data.Config().Prefab);
            HeroObj heroObj = GameObject.Instantiate(gObj);
            return heroObj;
        }

        public static HeroData Create(int id,int level = 1)
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
            HeroData heroData = new HeroData(id,uid,level,equipmentData,skillData);
            return heroData;
        }

        public static Hero Config(this HeroData self)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Hero table = tableManager.Tables.TbHero.Get(self.Id);
            return table;
        }

        public static Skill Config(this SkillModel self)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Skill table = tableManager.Tables.TbSkill.Get(self.Id);
            return table;
        }
        
        public static Skill Config(this SkillData self)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Skill table = tableManager.Tables.TbSkill.Get(self.Id);
            return table;
        }

        public static SkillModel GetSkillModel(this SkillData self)
        {
            Skill config = self.Config();
            return new SkillModel(self.Id, config.Condition, config.Cost, config.EffectKey,config.EffectValue);
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
        
        public static TimelineModel GetTimelineModel(string key,EventWarp[] effectValue)
        {
            GameManager.Instance.GetService(out TableManager tableManager);
            Timeline timeline = tableManager.Tables.TbTimeline.Get(key);
            TimelineNode[] nodes = new TimelineNode[timeline.Node.Length];
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i] = ConvertTimelineNodeWarpToTimelineNode(timeline.Node[i]);

                if (effectValue != null)
                {
                    foreach (EventWarp warp in effectValue)
                    {
                        if (timeline.Node[i].EventWarp.Event == warp.Event)
                        {
                            nodes[i].EventParameters.AddRange(warp.Params);
                        }
                    }
                }
            }
            
            TimelineModel skillModel = new TimelineModel(timeline.Key,nodes,timeline.Duration);
            return skillModel;
        }

        private static TimelineNode ConvertTimelineNodeWarpToTimelineNode(TimelineNodeWarp warp)
        {
            return new TimelineNode(warp.Elapsed,warp.EventWarp.Event, ConvertMObjectToObject(warp.EventWarp.Params));
        }

        private static List<object> ConvertMObjectToObject(List<mObject> self)
        {
            List<object> objects = new List<object>(self.Count);
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i] = ConvertMObjectToObject(self[i]);
            }

            return objects;
        }

        private static object ConvertMObjectToObject(mObject mObj)
        {
            if (mObj is mInt mInt)
            {
                return mInt.Data;
            }
            
            if (mObj is mFloat mFloat)
            {
                return mFloat.Data;
            }
            
            if (mObj is mString mString)
            {
                return mString.Data;
            }
            
            if (mObj is mCreateDamageWarp mCreateDamageWarp)
            {
                return mCreateDamageWarp.Data;
            }
            
            if (mObj is mAddBuffWarp mAddBuffWarp)
            {
                return mAddBuffWarp.Data;
            }

            return null;
        }
    }
}