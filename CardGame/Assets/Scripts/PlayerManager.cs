using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace MyGame
{
    public class PlayerManager : IPostGameService
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

        public void SetPlace(long uid, Grid grid)
        {
            PlayerData.Faction[grid.X, grid.Y] = uid;
        }

        public HeroData GetHeroModel(long uid)
        {
            return PlayerData.Heroes.FirstOrDefault(model => model.Uid == uid);
        }
    }
}