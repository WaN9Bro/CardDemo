using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace MyGame
{
    public static class FactionHelper
    {
        public static Faction CreatePlayerFaction(int[,] faction)
        {
            if (faction.IsNullOrEmpty())
            {
                Debug.LogError("[FactionHelper] faction is empty");
                return null;
            }

            HeroData[,] models = new HeroData[3,2];
            GameManager.Instance.GetService(out PlayerManager playerManager);

            for (int i = 0; i < faction.GetLength(0); i++)
            {
                for (int j = 0; j < faction.GetLength(0); j++)
                {
                    if (faction[i,j] == 0) continue;
                    HeroData heroData = playerManager.GetHeroModel(faction[i,j]);
                    if (heroData == null)
                    {
                        Debug.LogErrorFormat("[FactionHelper] hero [{0}] is invalid", faction[i,j]);
                        continue;
                    }

                    models[i, j] = playerManager.GetHeroModel(faction[i, j]);
                }
            }
            

            Faction fa = ReferencePool.Acquire<Faction>();
            fa.Init(models, EFaction.Player);
            return fa;
        }

        public static Faction CreateEnemyFaction(int stage)
        {
            HeroData data = HeroHelper.Create(1001);
            HeroData[,] models = new HeroData[3,2];
            models[0,0] = data;
            Faction fa = ReferencePool.Acquire<Faction>();
            fa.Init(models, EFaction.Enemy);
            return fa;
        }
    }
}