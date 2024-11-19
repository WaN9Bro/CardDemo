using UnityEngine;

namespace MyGame
{
    public class HeroUICom : MonoBehaviour
    {
        public HeroObj HeroObj { get; private set; }

        [SerializeField] private Transform hpBar;
        [SerializeField] private Transform shieldBar;
        
        public void Initialize(HeroObj heroObj)
        {
            HeroObj = heroObj;
        }

        public void Modify()
        {
            float hp = 0;
            if (HeroObj.Health.HP == 0 || HeroObj.Property.TotalHp == 0)
            {
                hp = 0;
            }
            else
            {
                hp = Mathf.Clamp((float)HeroObj.Health.HP / HeroObj.Property.TotalHp,0,1);
            }
            
            float shield = 0;
            
            if (HeroObj.Health.Shield == 0 || HeroObj.Property.Shield == 0)
            {
                shield = 0;
            }
            else
            {
                shield = Mathf.Clamp((float)HeroObj.Health.Shield / HeroObj.Property.Shield,0,1);
            }
            
            hpBar.localScale = new Vector3(hp, 1,1);
            shieldBar.localScale = new Vector3(shield, 1,1);
        }
    }
}