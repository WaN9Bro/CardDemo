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
    public class Faction
    {
        private class HeroWarp
        {
            public HeroModel Model;
            public HeroObj Obj;

            public HeroWarp( HeroModel model, HeroObj obj)
            {
                Model = model;
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
                    if(kv.Value.Model == null) continue;
                    
                    if (kv.Value.Obj.IsDead == false)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public Faction(List<HeroModel> heroModels ,EFaction factionType)
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
            Clear();
            GameManager.Instance.GetService(out BattleManager battleManager);
            foreach (KeyValuePair<EStanding, HeroWarp> kv in _heroObjs)
            {
                if (kv.Value.Model == null) continue;
                HeroObj heroObj = HeroObjCreator.CreateHeroObj(kv.Value.Model);
                kv.Value.Obj = heroObj;
                heroObj.Init(kv.Value.Model,kv.Key);
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

        public HeroObj FindActionTarget(EStanding standing)
        {
            // 如果是普通攻击
            if (_placeHeros.TryGetValue(standing, out HeroObj entity))
            {
                if (entity != null)
                {
                    return entity;
                }
            }

            foreach (HeroObj _entity in _placeHeros.Values)
            {
                if (_entity != null)
                    return _entity;
            }

            return null;
        }

        public async UniTask FactionAciton(Faction otherFaction)
        {
            foreach (KeyValuePair<EStanding, HeroObj> pair in _heroObjs)
            {
                if (!otherFaction.HasEntityAlive) return;
                if (pair.Value == null) continue;
                if(pair.Value.IsDead) continue;                                                         
                HeroBattleCom heroBattleCom = pair.Value.GetEntityComponent<HeroBattleCom>();
                heroBattleCom.Active(this,otherFaction);
            }
        }
    }
}