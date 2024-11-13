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

            List<HeroData> models = new List<HeroData>();
            GameManager.Instance.GetService(out PlayerManager playerManager);
            for (int i = 0; i < faction.Length; i++)
            {
                if (faction[i] == 0) continue;
                HeroData heroData = playerManager.GetHeroModel(faction[i]);
                if (heroData == null)
                {
                    Debug.LogErrorFormat("[FactionHelper] hero [{0}] is invalid", faction[i]);
                    continue;
                }
                models.Add(playerManager.GetHeroModel(faction[i]));
            }

            Faction fa = ReferencePool.Acquire<Faction>();
            fa.Init(models, EFaction.Player);
            return fa;
        }

        public static Faction CreateEnemyFaction(int stage)
        {
            HeroData data = Helper.Create(1001);
            List<HeroData> models = new List<HeroData> { data };
            Faction fa = ReferencePool.Acquire<Faction>();
            fa.Init(models, EFaction.Enemy);
            return fa;
        }
    }
}