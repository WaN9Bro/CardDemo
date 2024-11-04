using System.Collections.Generic;

namespace MyGame
{
    public class HeroBuffCom : IHeroComponent
    {
        public HeroObj HeroObj { get; }
        public void Initialize(HeroObj heroObj) {
            throw new System.NotImplementedException();
        }

        public List<BuffObj> Buffs = new List<BuffObj>();
        private IHeroComponent _heroComponentImplementation;

        public HeroProperty GetProperty()
        {
            
        }

        public void Clear()
        {
            Buffs.Clear();
        }
    }
}