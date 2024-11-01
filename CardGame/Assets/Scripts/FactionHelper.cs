using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace MyGame
{
    public static class FactionHelper
    {
        public static Faction CreatePlayerFaction(int[] faction)
        {
            if (faction.IsNullOrEmpty())
            {
                Debug.LogError("[FactionHelper] faction is empty");
                return null;
            }

            List<HeroModel> models = new List<HeroModel>();
            GameManager.Instance.GetService(out PlayerManager playerManager);
            for (int i = 0; i < faction.Length; i++)
            {
                models.Add(playerManager.GetHeroModel(faction[i]));
            }
            return new Faction(models, EFaction.Player);
        }

        public static Faction CreateEnemyFaction(int stage)
        {
            
            // TODO: 从配置里 读取 阵营英雄
            return new Faction();
        }
    }
}