using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MyGame
{
    
    public class HeroSkillCom : IHeroComponent
    {
        public HeroObj HeroObj { get; }

        /// <summary>
        /// 拥有的技能列表
        /// </summary>
        public readonly Dictionary<SkillObj, SkillCastCondition> SkillObjs = new Dictionary<SkillObj, SkillCastCondition>();
        
        public void Initialize(HeroObj heroObj)
        {
            
        }

        public void CastSkill()
        {
            // 首先看有没有主动技能可以释放的，没有的话就放普攻，
            // 初始化技能的时候就进行初始化技能cd表
            
            
        }
        
        public HeroProperty GetProperty()
        {
            return default;
        }

        public void Clear()
        {
            SkillObjs.Clear();
        }
    }
}