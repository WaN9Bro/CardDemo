using System.Collections.Generic;

namespace MyGame
{
    public class HeroBuffCom : IHeroComponent
    {
        public HeroObj HeroObj { get; }
        public void Initialize(HeroObj heroObj, GameManager gameManager) { throw new System.NotImplementedException();
        }

        public void CleanUp()
        {
            Buffs.Clear();
        }

        public List<BuffObj> Buffs = new List<BuffObj>();

        public HeroProperty GetProperty()
        {
            
        }
    }
}