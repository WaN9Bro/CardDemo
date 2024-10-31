using UnityEngine;

namespace MyGame
{
    public class PlayerManager : MonoBehaviour, IPostGameService
    {
        public PlayerData PlayerData { get; private set; }

        public void Init(GameManager gameManager)
        {
            PlayerData = PlayerData.Default;
        }

        public void AddHero(HeroModel hero)
        {
            PlayerData.Heroes.Add(hero);
        }
    }
}