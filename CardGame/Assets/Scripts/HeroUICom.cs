using UnityEngine;

namespace MyGame
{
    public class HeroUICom : IHeroComponent
    {
        public HeroObj HeroObj { get; }
        public void Initialize(HeroObj heroObj)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(HeroObj heroObj, GameManager gameManager)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            
        }
    }
}