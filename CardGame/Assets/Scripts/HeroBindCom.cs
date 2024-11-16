using UnityEngine;

namespace MyGame
{
    public class HeroBindCom : IHeroComponent
    {
        public HeroObj HeroObj { get; private set; }
        
        public void Initialize(HeroObj heroObj)
        {
            HeroObj = heroObj;
        }
        
        public void Clear()
        {
            HeroObj = null;
        }
        
        public BindPoint GetBindPointByKey(string key){
            BindPoint[] bindPoints = HeroObj.GetComponentsInChildren<BindPoint>();
            for (int i = 0; i < bindPoints.Length; i++){
                if (bindPoints[i].Key == key){
                    return bindPoints[i];
                }
            }
            return null;
        }
        
        public void AddBindGameObject(string bindPointKey, string go, string key, bool loop){
            BindPoint bp = GetBindPointByKey(bindPointKey);
            if (bp == null) return;
            bp.AddBindGameObject(go, key, loop);
        }
        
        public void RemoveBindGameObject(string bindPointKey, string key){
            BindPoint bp = GetBindPointByKey(bindPointKey);
            if (bp == null) return;
            bp.RemoveBindGameObject(key);
        }
    }
}