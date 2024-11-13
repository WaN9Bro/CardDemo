using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

// ReSharper disable All

namespace MyGame
{
    public class Faction : IReference
    {
        private class HeroWarp
        {
            public HeroData Data;
            public HeroObj Obj;

            public HeroWarp( HeroData data, HeroObj obj)
            {
                Data = data;
                Obj = obj;
            }
        }
        
        private Dictionary<EStanding, HeroWarp> _heroObjs = new Dictionary<EStanding, HeroWarp>();
        private EFaction _factionType;

        public bool HasEntityAlive
        {
            get
            {
                foreach (var kv in _heroObjs)
                {
                    if(kv.Value.Data == null) continue;
                    
                    if (kv.Value.Obj.IsDead == false)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void Init(List<HeroData> heroModels ,EFaction factionType)
        {
            for (var i = 0; i < heroModels.Count; i++)
            {
                _heroObjs.Add((EStanding)i, new HeroWarp(heroModels[i],null));
            }
            
            _factionType = factionType;
        }

        public void Clear()
        {
            _heroObjs.Clear();
        }

        // 战前准备
        public void PrepareBattle()
        {
            GameManager.Instance.GetService(out BattleManager battleManager);
            foreach (KeyValuePair<EStanding, HeroWarp> kv in _heroObjs)
            {
                if (kv.Value.Data == null) continue;
                HeroObj heroObj = Helper.CreateHeroObj(kv.Value.Data);
                kv.Value.Obj = heroObj;
                heroObj.Init(kv.Value.Data,kv.Key);
            }
        }

        public void SetHeroTransfrom(List<BattlePlace> places)
        {
            int i = 0;
            foreach (KeyValuePair<EStanding, HeroWarp> kv in _heroObjs)
            {
                if (kv.Value.Obj == null) continue;
                // 初始化位置
                kv.Value.Obj.transform.SetParent(places[i].Trans);
                kv.Value.Obj.transform.SetLocalPositionAndRotation(Vector3.zero,Quaternion.identity);
                i++;
            }
        }
        
        public async UniTask StartBattle(Faction otherFaction)
        {
            foreach (var kv in _heroObjs)
            {
                if (!otherFaction.HasEntityAlive) return;
                if (kv.Value == null) continue;
                if(kv.Value.Obj.IsDead) continue;                                                         
                await kv.Value.Obj.StartBattle(this,otherFaction);
            } 
        }
    }
}