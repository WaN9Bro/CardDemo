using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

namespace MyGame
{
    public class HeroSkillCom : MonoBehaviour
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
        
        public HeroObj HeroObj { get; private set; }

        /// <summary>
        /// 拥有的技能列表  0：普攻、1：主动技能1、2：被主动技能2
        /// </summary>
        private readonly List<SkillWarp> _skillObjs = new List<SkillWarp>();
        
        private readonly List<PassiveSkill> _passiveSkillObjs = new List<PassiveSkill>();
        
        public void Initialize(HeroObj heroObj)
        {
            HeroObj = heroObj;
            if (!heroObj.Data.Skill.IsNullOrEmpty())
            {
                foreach (SkillData skillData in heroObj.Data.Skill)
                {
                    SkillModel model = skillData.GetSkillModel();
                    Skill config = model.Config();
                
                    SkillObj skillObj = ReferencePool.Acquire<SkillObj>();
                    skillObj.Init(model);

                    SkillWarp skillWarp = ReferencePool.Acquire<SkillWarp>();
                    skillWarp.Init(skillObj,config.Rule.Cast,config.Rule.CD,0);
                
                    _skillObjs.Add(skillWarp);
                }
            }

            // 给玩家添加被动技能的buff
            if (!heroObj.Data.PassiveSkill.IsNullOrEmpty())
            {
                foreach (PassiveSkillData passiveSkillData in heroObj.Data.PassiveSkill)
                {
                    PassiveSkillObj passiveSkillObj = ReferencePool.Acquire<PassiveSkillObj>();
                    passiveSkillObj.Init(passiveSkillData.Id,passiveSkillData.Config().AddBuff.ConvertWarpToAddBuffInfos());
                    
                    if (!passiveSkillObj.AddBuffInfos.IsNullOrEmpty())
                    {
                        for (var i = 0; i < passiveSkillObj.AddBuffInfos.Count; i++)
                        {
                            AddBuffInfo addBuffInfo = passiveSkillObj.AddBuffInfos[i];
                            addBuffInfo.Caster = heroObj;
                            addBuffInfo.Target = heroObj;
                            HeroObj.BuffCom.AddBuff(addBuffInfo);
                        }
                    }
                }
            }
        }

        public void RefreshSkillCDRound()
        {
            foreach (SkillWarp warp in _skillObjs)
            {
                if (warp.CdRound > 0)
                {
                    warp.CdRound -= 1;
                }
            }
        }

        public async UniTask CastSkill()
        {
            // 新的回合需要刷新技能cd再开始放技能
            RefreshSkillCDRound();
            
            // 再看有没有主动技能可以释放的，没有的话就放普攻，
            GameManager.Instance.GetService(out BattleManager battleManager);
            int curRound = battleManager.Round;
            int castSkillIndex = 0; //默认是普攻
            if (HeroObj.ControlMod.CanUseSkill == true)
            {
                if (IsUsefulSkillByIndex(1, curRound) == true)
                {
                    castSkillIndex = 1;
                }
                else if (IsUsefulSkillByIndex(2, curRound) == true)
                {
                    castSkillIndex = 2;
                }
            }
            
            // 创建技能TimelineObj
            GameManager.Instance.GetService(out TimelineManager timelineManager);
            TimelineObj timelineObj = new TimelineObj();
            timelineObj.Init(_skillObjs[castSkillIndex].SkillObj.Model.Effect, HeroObj);
            HeroObj.BuffCom.ExecuteBuff(EBuffEventType.OnCast,timelineObj);
            HeroObj.ModifyHealth(_skillObjs[castSkillIndex].SkillObj.Model.Cost.ConvertToHeroHealth());
            _skillObjs[castSkillIndex].CdRound = _skillObjs[castSkillIndex].FixedCdRound + 1;
            if (timelineManager.AddTimeline(timelineObj))
            {
                await timelineObj.AwaitRelease();
            }
        }

        private bool IsUsefulSkillByIndex(int index,int curRound)
        {
            if (index >= _skillObjs.Count)
            {
                return false;
            }
            
            if(!HeroObj.Health.Enough(_skillObjs[index].SkillObj.Model.Condition))
            {
                return false;
            }
            
            if(!HeroObj.Health.Enough(_skillObjs[index].SkillObj.Model.Cost))
            {
                return false;
            }
            
            if (curRound == _skillObjs[index].CastRound)
            {
                return true;
            }
            
            return _skillObjs[index].CdRound <= 0;
        }

        public void Clear()
        {
            for (var i = 0; i < _skillObjs.Count; i++)
            {
                SkillWarp skillWarp = _skillObjs[i];
                ReferencePool.Release(skillWarp);
            }

            _skillObjs.Clear();
        }
    }
}