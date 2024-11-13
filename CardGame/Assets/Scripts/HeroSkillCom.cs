using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace MyGame
{
    public class HeroSkillCom : IHeroComponent
    {
        private class SkillWarp : IReference
        {
            public SkillObj SkillObj;
            public int CastRound;
            public int FixedCdRound;
            public int CdRound;

            public void Init(SkillObj skillObj,int castRound,int fixedCdRound, int cdRound)
            {
                SkillObj = skillObj;
                CastRound = castRound;
                FixedCdRound = fixedCdRound;
                CdRound = cdRound;
            }

            public void Clear()
            {
                ReferencePool.Release(SkillObj);
                SkillObj = null;
            }
        }
        
        public HeroObj HeroObj { get; }

        /// <summary>
        /// 拥有的技能列表  0：普攻、1：主动技能1、2：被动技能1、3：主动技能2、4：被动技能2
        /// </summary>
        private readonly List<SkillWarp> SkillObjs = new List<SkillWarp>();
        
        public void Initialize(HeroObj heroObj)
        {
            foreach (SkillData skillData in heroObj.Data.Skill)
            {
                SkillModel model = skillData.GetSkillModel();
                Skill config = model.Config();
                
                SkillObj skillObj = ReferencePool.Acquire<SkillObj>();
                skillObj.Init(model);

                SkillWarp skillWarp = ReferencePool.Acquire<SkillWarp>();
                skillWarp.Init(skillObj,config.Rule.Cast,config.Rule.CD,0);
                
                SkillObjs.Add(skillWarp);
            }
        }

        public async UniTask CastSkill()
        {
            // 首先看有没有主动技能可以释放的，没有的话就放普攻，
            GameManager.Instance.GetService(out BattleManager battleManager);
            int curRound = battleManager.Round;
            int castSkillIndex = 0; //默认是普攻
            if (HeroObj.ControlMod.CanUseSkill == true)
            {
                if (IsUsefulSkillByIndex(1, curRound) == true)
                {
                    castSkillIndex = 1;
                }
                else if (IsUsefulSkillByIndex(3, curRound) == true)
                {
                    castSkillIndex = 3;
                }
            }
            
            // 创建技能TimelineObj
            GameManager.Instance.GetService(out TimelineManager timelineManager);
            TimelineObj timelineObj = new TimelineObj(SkillObjs[castSkillIndex].SkillObj.Model.Effect, HeroObj, SkillObjs[castSkillIndex].SkillObj);
            
            timelineManager.AddTimeline(timelineObj);
        }

        private bool IsUsefulSkillByIndex(int index,int curRound)
        {
            if (index >= SkillObjs.Count)
            {
                return false;
            }
            
            if (curRound != SkillObjs[index].CastRound)
            {
                return false;
            }
            
            if(SkillObjs[index].CdRound > 0)
            {
                return false;
            }
            
            if(HeroObj.Resource.Enough(SkillObjs[index].SkillObj.Model.Condition))
            {
                return false;
            }
            
            if(HeroObj.Resource.Enough(SkillObjs[index].SkillObj.Model.Cost))
            {
                return false;
            }

            return false;
        }
        
        public HeroProperty GetProperty()
        {
            return default;
        }

        public void Clear()
        {
            for (var i = 0; i < SkillObjs.Count; i++)
            {
                SkillWarp skillWarp = SkillObjs[i];
                ReferencePool.Release(skillWarp);
            }

            SkillObjs.Clear();
        }
    }
}