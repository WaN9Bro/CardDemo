using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;


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
        
        private readonly HeroWarp[,] _heroObjs = new HeroWarp[3,2];
        private EFaction _factionType;

        public bool HasEntityAlive
        {
            get
            {
                foreach (var heroWarp in _heroObjs)
                {
                    if(heroWarp.Data == null) continue;
                    
                    if (heroWarp.Obj.IsDead == false)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public void Init(HeroData[,] heroModels ,EFaction factionType)
        {
            for (int i = 0; i < heroModels.GetLength(0); i++)
            {
                for (int j = 0; j < heroModels.GetLength(1); j++)
                {
                    _heroObjs[i,j] = new HeroWarp(heroModels[i,j], null);
                }
            }
            
            _factionType = factionType;
        }

        public void Clear()
        {
            for (int i = 0; i < _heroObjs.GetLength(0); i++)
            {
                for (int j = 0; j < _heroObjs.GetLength(1); j++)
                {
                    _heroObjs[i,j].Obj = null;
                }
            }
        }

        // 战前准备
        public void PrepareBattle()
        {
            GameManager.Instance.GetService(out BattleManager battleManager);
            for (int i = 0; i < _heroObjs.GetLength(0); i++)
            {
                for (int j = 0; j < _heroObjs.GetLength(1); j++)
                {
                    HeroWarp heroWarp = _heroObjs[i,j];
                    if (heroWarp.Data == null) continue;
                
                    // 创建HeroObj对象到游戏中
                    HeroObj heroObj = HeroHelper.CreateHeroObj(heroWarp.Data);
                    heroWarp.Obj = heroObj; ;
                    Grid grid = new Grid(i, j);
                    
                    // 设置位置
                    heroObj.transform.SetParent(battleManager.GetPlace(_factionType,grid));
                    heroObj.transform.localPosition = Vector3.zero;
                    heroObj.transform.localRotation = Quaternion.identity;
                    heroObj.transform.localScale = Vector3.one;
                    
                    // 创建Hero的Spine
                    GameObject spineObj = HeroHelper.CreateHeroSpine(heroWarp.Data);
                    spineObj.transform.SetParent(heroObj.SpineContainer);
                    spineObj.transform.localPosition = Vector3.zero;
                    spineObj.transform.localRotation = Quaternion.identity;
                    spineObj.transform.localScale = Vector3.one;

                    // 初始化
                    heroObj.Init(heroWarp.Data, grid, _factionType);
                }
            }
        }
        
        public async UniTask StartBattle(Faction otherFaction)
        {
            foreach (HeroWarp heroWarp in _heroObjs)
            {
                if (!otherFaction.HasEntityAlive) return;
                if (heroWarp.Data == null) continue;
                if(heroWarp.Obj.IsDead) continue;                                                         
                await heroWarp.Obj.StartBattle();
            } 
        }

        public List<HeroObj> GetNoHasBuffHeroObjs(string key)
        {
            List<HeroObj> aliveHeroObjs = GetAliveHeroObjs();
            if (!aliveHeroObjs.IsNullOrEmpty())
            {
                List<HeroObj> result = ListPool<HeroObj>.Get();
                foreach (HeroObj heroObj in aliveHeroObjs)
                {
                    if (!heroObj.BuffCom.HasBuff(key, out var _))
                    {
                        result.Add(heroObj);
                    }
                }

                ListPool<HeroObj>.Release(aliveHeroObjs);
                return result;
            }

            return null;
        }

        public List<HeroObj> GetAliveHeroObjs()
        {
            List<HeroObj> aliveHeroObjs = new List<HeroObj>();
            foreach (HeroWarp heroWarp in _heroObjs)
            {
                if(heroWarp.Data == null) continue;
                    
                if (heroWarp.Obj.IsDead == false)
                {
                    aliveHeroObjs.Add(heroWarp.Obj);
                }
            }

            return aliveHeroObjs;
        }

        public List<HeroObj> GetFilterHeroObjs(TargetWarp warp,HeroObj caster)
        {
            List<HeroObj> aliveHeroObjs = GetAliveHeroObjs();
            
            switch (warp.TargetFliter)
            {
                case ETargetFliter.Normal :
                    return GetFilterHeroObjsByNormal(caster);
                case ETargetFliter.Random:
                    return GetFilterHeroObjsByRandom(aliveHeroObjs, warp.Count);
                case ETargetFliter.LargestRow:
                    return GetFilterHeroObjsByLargestRow(aliveHeroObjs);
                case ETargetFliter.HighestAttack:
                    return GetFilterHeroObjsByHighestAttack(aliveHeroObjs);
                case ETargetFliter.FrontRow:
                    return GetFilterHeroObjsByFrontRow(aliveHeroObjs);
                case ETargetFliter.RearRow:
                    return GetFilterHeroObjsByRearRow(aliveHeroObjs);
                case ETargetFliter.LowestHealth:
                    return GetFilterHeroObjsByLowestHealth(aliveHeroObjs);
                case ETargetFliter.MyDeadOrLowestHealth:
                    return GetFilterHeroObjsByMyDeadOrLowestHealth(aliveHeroObjs);
            }

            return null;
        }
        
        private List<HeroObj> GetFilterHeroObjsByNormal(HeroObj caster)
        {
            List<HeroObj> result = new List<HeroObj>(1);
            Grid grid = caster.Grid;
            HeroWarp[,] temp = null;
            // 先找对位的那一行
            for (int i = 0; i < 2; i++)
            {
                if (_heroObjs[grid.X, i].Data != null && !_heroObjs[grid.X, i].Obj.IsDead)
                {
                    result.Add(_heroObjs[grid.X,i].Obj);
                    return result;
                }
            }
            
            temp = new HeroWarp[2, 2];
            for (int i = 0; i < _heroObjs.GetLength(0); i++)
            {
                for (int j = 0; j < _heroObjs.GetLength(1); j++)
                {
                    if (i == grid.X) continue;
                        
                    temp[i,j] = _heroObjs[i,j];
                }
            }

            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    if (temp[i,j].Data != null && !temp[i,j].Obj.IsDead)
                    {
                        result.Add(temp[i,j].Obj);
                        return result;
                    }
                }
            }

            return null;
        }

        private List<HeroObj> GetFilterHeroObjsByRandom(List<HeroObj> aliveHeroObjs,int targetCount)
        {
            List<HeroObj> temp = new List<HeroObj>();
            temp.AddRange(aliveHeroObjs);
            
            if (aliveHeroObjs.Count < targetCount)
            {
                targetCount = aliveHeroObjs.Count;
            }

            List<HeroObj> result = new List<HeroObj>(targetCount);

            for (int i = 0; i < targetCount; i++)
            {
                int random = Random.Range(0, temp.Count);
                result.Add(temp[random]);
                temp.RemoveAt(random);
            }

            return result;
        }

        private List<HeroObj> GetFilterHeroObjsByLargestRow(List<HeroObj> aliveHeroObjs)
        {
            List<HeroObj> frontRow = new List<HeroObj>();
            List<HeroObj> rearRow = new List<HeroObj>();
            foreach (HeroObj heroObj in aliveHeroObjs)
            {
                if (heroObj.IsFrontRowHero)
                {
                    frontRow.Add(heroObj);
                }
                else
                {
                    rearRow.Add(heroObj);
                }
            }

            return frontRow.Count >= frontRow.Count ? frontRow : rearRow;
        }

        private List<HeroObj> GetFilterHeroObjsByHighestAttack(List<HeroObj> aliveHeroObjs)
        {
            List<HeroObj> result = new List<HeroObj>(1);
            HeroObj highestAttack = aliveHeroObjs[0];
            result.Add(highestAttack);
            if (aliveHeroObjs.Count > 1)
            {
                for (var i = 1; i < aliveHeroObjs.Count; i++)
                {
                    if (highestAttack.Property.Attack < aliveHeroObjs[i].Property.Attack)
                    {
                        highestAttack = aliveHeroObjs[i];
                    }
                }
            }

            return result;
        }

        private List<HeroObj> GetFilterHeroObjsByFrontRow(List<HeroObj> aliveHeroObjs)
        {
            List<HeroObj> frontRow = new List<HeroObj>();
            foreach (HeroObj heroObj in aliveHeroObjs)
            {
                if (heroObj.IsFrontRowHero)
                {
                    frontRow.Add(heroObj);
                }
            }

            if (frontRow.Count <= 0)
            {
                return GetFilterHeroObjsByRearRow(aliveHeroObjs);
            }

            return frontRow;
        }
        
        private List<HeroObj> GetFilterHeroObjsByRearRow(List<HeroObj> aliveHeroObjs)
        {
            List<HeroObj> rearRow = new List<HeroObj>();
            foreach (HeroObj heroObj in aliveHeroObjs)
            {
                if (heroObj.IsRearRowHero)
                {
                    rearRow.Add(heroObj);
                }
            }

            if (rearRow.Count <= 0)
            {
                return GetFilterHeroObjsByFrontRow(aliveHeroObjs);
            }

            return rearRow;
        }
        
        private List<HeroObj> GetFilterHeroObjsByLowestHealth(List<HeroObj> aliveHeroObjs)
        {
            List<HeroObj> result = new List<HeroObj>(1);
            HeroObj lowestAttack = aliveHeroObjs[0];
            result.Add(lowestAttack);
            if (aliveHeroObjs.Count > 1)
            {
                for (var i = 1; i < aliveHeroObjs.Count; i++)
                {
                    if (lowestAttack.Property.Attack > aliveHeroObjs[i].Property.Attack)
                    {
                        lowestAttack = aliveHeroObjs[i];
                    }
                }
            }

            return result;
        }

        private List<HeroObj> GetFilterHeroObjsByMyDeadOrLowestHealth(List<HeroObj> aliveHeroObjs)
        {
            for (int i = 0; i < _heroObjs.GetLength(0); i++)
            {
                for (int j = 0; j < _heroObjs.GetLength(1); j++)
                {
                    if (_heroObjs[i,j].Data != null && _heroObjs[i,j].Obj.IsDead)
                    {
                        List<HeroObj> result = new List<HeroObj>(1);
                        result.Add(_heroObjs[i,j].Obj);
                        return result;
                    }
                }
            }

            return GetFilterHeroObjsByLowestHealth(aliveHeroObjs);
        }
    }
}