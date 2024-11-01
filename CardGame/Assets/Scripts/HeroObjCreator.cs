using UnityEngine;

namespace MyGame
{
    public static class HeroObjCreator
    {
        public static HeroObj CreateHeroObj(HeroModel model)
        {
            HeroObj gObj = Resources.Load<HeroObj>(model.Prefab);
            HeroObj heroObj = GameObject.Instantiate(gObj);
            return heroObj;
        }
    }
}