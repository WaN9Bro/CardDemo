using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
// ReSharper disable All

namespace MyGame
{
    public class Faction
    {
        private List<HeroObj> _entities;
        private Dictionary<EStanding,HeroObj> _factionEntities;
        private GameManager _gameManager;

        public bool HasEntityAlive
        {
            get
            {
                foreach (HeroObj entity in _entities)
                {
                    if (!entity.IsDead)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void Init(GameManager gameManager, List<HeroObj> entities)
        {
            _gameManager = gameManager;
            _entities = entities;
            foreach (var v in Enum.GetValues(typeof(EStanding)))
            {
                EStanding standing = (EStanding)v;
                _factionEntities.Add(standing,_entities.FirstOrDefault(entity => entity.Standing == standing));
            }
        }

        // 战前准备
        public void PreBattle()
        {
            
        }

        public HeroObj FindActionTarget(EStanding standing)
        {
            // 如果是普通攻击
            if (_factionEntities.TryGetValue(standing, out HeroObj entity))
            {
                if (entity != null)
                {
                    return entity;
                }
            }

            foreach (HeroObj _entity in _factionEntities.Values)
            {
                if (_entity != null)
                    return _entity;
            }

            return null;
        }

        public async UniTask FactionAciton(Faction otherFaction)
        {
            foreach (KeyValuePair<EStanding, HeroObj> pair in _factionEntities)
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