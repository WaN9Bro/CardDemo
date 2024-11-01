using System.Linq;
using UnityEngine;

namespace MyGame
{
    public class PlayerManager : MonoBehaviour, IPostGameService
    {
        public PlayerData PlayerData { get; private set; }

        public void Init()
        {
            PlayerData = PlayerData.Default;
        }

        public void AddHero(HeroModel hero)
        {
            PlayerData.Heroes.Add(hero);
        }

        public HeroModel GetHeroModel(int uid)
        {
            return PlayerData.Heroes.FirstOrDefault(model => model.Uid == uid);
        }
    }
}