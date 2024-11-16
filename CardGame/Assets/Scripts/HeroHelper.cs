using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyGame
{
    public static class HeroHelper
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
            return new TimelineNode(warp.Elapsed,warp.EventWarp.Event, MObjectHelper.ConvertMObjectToObject(warp.EventWarp.Params));
        }
    }
}