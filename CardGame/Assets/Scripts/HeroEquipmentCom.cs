namespace MyGame
{
    public class HeroEquipmentCom : IHeroComponent
    {
        public HeroObj HeroObj { get; }
        public void Initialize(HeroObj heroObj)
        {
            throw new System.NotImplementedException();
        }
        
        public HeroProperty GetProperty()
        {
            return default;
        }

        public void Clear()
        {
            
        }
    }
}