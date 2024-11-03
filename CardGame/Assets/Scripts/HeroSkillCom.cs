using UnityEngine;

namespace MyGame
{
    public class HeroSkillCom : MonoBehaviour,IHeroComponent
    {
        public HeroObj HeroObj { get; }
        public void Initialize(HeroObj heroObj, GameManager gameManager)
        {
            
        }

        public void CleanUp()
        {
            
        }
        
        public HeroProperty GetProperty()
        {
            
        }
        
    }
}