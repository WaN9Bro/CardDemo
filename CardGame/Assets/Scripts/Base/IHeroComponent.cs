namespace MyGame
{
    /// <summary>
    /// 逻辑组件
    /// </summary>
    public interface IHeroComponent
    {
        HeroObj HeroObj { get; }
        
        void Initialize(HeroObj heroObj);

        void CleanUp();
    }
}