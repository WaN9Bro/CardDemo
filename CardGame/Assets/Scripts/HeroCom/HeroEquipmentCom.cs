using UnityEngine;

namespace MyGame
{
    public class HeroEquipmentCom : MonoBehaviour
    {
        public HeroObj HeroObj { get; private set; }
        public void Initialize(HeroObj heroObj)
        {
            HeroObj = heroObj;
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