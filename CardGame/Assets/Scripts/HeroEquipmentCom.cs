namespace MyGame
{
    public class HeroEquipmentCom : IHeroComponent
    {
        public HeroObj HeroObj { get; }
        public void Initialize(HeroObj heroObj, GameManager gameManager)
        {
            throw new System.NotImplementedException();
        }

        public void CleanUp()
        {
            throw new System.NotImplementedException();
        }
        
        public HeroProperty GetProperty()
        {
            
        }
    }
}