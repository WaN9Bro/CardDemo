using System;
using System.Linq;
using Unity.VisualScripting;
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

        public void AddHero(HeroData hero)
        {
            if (PlayerData.Heroes.Exists(v => v.Uid == hero.Uid))
            {
                throw new Exception($"[PlayerManager]Hero Uid'{hero.Uid}' Already Exists");
            }
            PlayerData.Heroes.Add(hero);
        }

        public HeroData GetHeroModel(int uid)
        {
            return PlayerData.Heroes.FirstOrDefault(model => model.Uid == uid);
        }
    }
}