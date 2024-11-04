using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MyGame
{
    public class HeroSkillCom : IHeroComponent
    {
        public HeroObj HeroObj { get; }
        public List<SkillObj> SkillObjs;
        
        public void Initialize(HeroObj heroObj)
        {
            SkillObjs = ListPool<SkillObj>.Get();
        }
        
        public HeroProperty GetProperty()
        
        {
            
        }

        public void Clear()
        {
            ListPool<SkillObj>.Release(SkillObjs);
        }
    }
}