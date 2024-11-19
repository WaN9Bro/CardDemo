using UnityEngine;
using UnityEngine.UI;

namespace MyGame
{
    public class UIManager : MonoBehaviour,IPreGameService
    {
        public Text textRound;
        public void Init()
        {
            
        }

        public void Modify()
        {
            GameManager.Instance.GetService(out BattleManager battleManager);

            textRound.text = "回合数：" + battleManager.Round;
        }
    }
}