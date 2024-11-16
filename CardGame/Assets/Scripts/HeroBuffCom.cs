using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace MyGame
{
    public class HeroBuffCom : IHeroComponent
    {
        public HeroObj HeroObj { get; private set; }

        public void Initialize(HeroObj heroObj)
        {
            HeroObj = heroObj;
        }

        public readonly List<BuffObj> Buffs = new List<BuffObj>();

        public HeroProperty GetProperty()
        {
            // 统计能直接带来攻击提升的buff
            // 判断当前buff有没有直接学了就能增加攻击力或者提升攻击百分比的
            HeroProperty property = HeroProperty.Empty;

            foreach (BuffObj buffObj in Buffs)
            {
                property += buffObj.Model.PropMod;
            }
            
            return default;
        }

        public void ExecuteBuff(EBuffEventType eventType,params object[] args)
        {
            foreach (BuffObj buffObj in Buffs)
            {
                buffObj.ExecuteBuff(eventType,buffObj,args);
            }
        }

        public void FixedUpdate(float fixedDeltaTime)
        {
            //对身上的buff进行管理
            List<BuffObj> removeBuffObjs = ListPool<BuffObj>.Get();
            for (int i = 0; i < Buffs.Count; i++)
            {
                if (Buffs[i].Permanent == false)
                {
                    Buffs[i].Duration -= fixedDeltaTime;
                }
                
                Buffs[i].TimeElapsed += fixedDeltaTime;

                if (Buffs[i].Model.TickTime > 0)
                {
                    //float取模不精准，所以用x1000后的整数来
                    if (Mathf.RoundToInt(Buffs[i].TimeElapsed * 1000) % Mathf.RoundToInt(Buffs[i].Model.TickTime * 1000) == 0)
                    {
                        Buffs[i].ExecuteBuff(EBuffEventType.OnTick,Buffs[i]);
                        Buffs[i].Ticked += 1;
                    }
                }

                //只要duration <= 0，不管是否是permanent都移除掉
                if (Buffs[i].Duration <= 0 || Buffs[i].Stack <= 0)
                {
                    Buffs[i].ExecuteBuff(EBuffEventType.OnRemoved,Buffs[i]);
                    removeBuffObjs.Add(Buffs[i]);
                }
            }

            if (removeBuffObjs.Count > 0)
            {
                for (int i = 0; i < removeBuffObjs.Count; i++)
                {
                    Buffs.Remove(removeBuffObjs[i]);
                }

                HeroObj.RecheckProperty();
            }
            
            ListPool<BuffObj>.Release(removeBuffObjs);
        }

        public void AddBuff(AddBuffInfo addBuffInfo)
        {
            bool hasBuff = HasBuff(addBuffInfo.Model.Id, out BuffObj existBuff);
            int modStack = Mathf.Min(addBuffInfo.AddStack, addBuffInfo.Model.MaxStack);
            bool needRemove = false;
            BuffObj addBuffObj = null;

            // 已经存在了这个buff，看是否能叠加
            if (hasBuff)
            {
                // 更新参数
                existBuff.BuffParams = new Dictionary<string, object>();
                if (addBuffInfo.BuffParams != null)
                {
                    foreach (KeyValuePair<string, object> kv in addBuffInfo.BuffParams)
                    {
                        existBuff.BuffParams[kv.Key] = kv.Value;
                    }
                }

                // 根据TimeModify来确认是直接设置持续时间还是增加持续时间
                existBuff.Duration = addBuffInfo.TimeModify
                    ? addBuffInfo.Duration
                    : addBuffInfo.Duration + existBuff.Duration;

                // 更新buff的可叠加层数
                int afterAdd = existBuff.Stack + modStack;
                modStack = afterAdd >= existBuff.Model.MaxStack
                    ? existBuff.Model.MaxStack - existBuff.Stack
                    : afterAdd <= 0
                        ? 0 - existBuff.Stack
                        : modStack;

                existBuff.Stack += modStack;
                existBuff.Permanent = addBuffInfo.Permanent;
                addBuffObj = existBuff;
                needRemove = existBuff.Stack <= 0;
            }
            else
            {
                addBuffObj = new BuffObj(addBuffInfo.Model, addBuffInfo.Duration, addBuffInfo.Permanent,
                    addBuffInfo.AddStack, addBuffInfo.Caster, addBuffInfo.Target, addBuffInfo.BuffParams);
                Buffs.Add(addBuffObj);
                Buffs.Sort((a, b) => a.Model.Priority.CompareTo(b.Model.Priority));
            }

            if (!needRemove)
            {
                addBuffObj.ExecuteBuff(EBuffEventType.OnOccur,addBuffObj, modStack);
            }

            HeroObj.RecheckProperty();
        }

        public void RemoveBuff(string buffKey)
        {
            if (HasBuff(buffKey, out BuffObj buffObj))
            {
                Buffs.Remove(buffObj);
                HeroObj.RecheckProperty();
            }
        }
        public bool HasBuff(string key, out BuffObj buffObj)
        {
            buffObj = Buffs.FirstOrDefault(buff => buff.Model.Id == key);
            if (buffObj != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 随机删除增益buff
        /// </summary>
        /// <param name="count"></param>
        public void RemoveBuffByRandom(int count)
        {
            RemoveBuff(count, true);
        }

        /// <summary>
        /// 随机删除负面buff
        /// </summary>
        /// <param name="count"></param>
        public void RemoveDebuffByRandom(int count)
        {
            RemoveBuff(count, false);
        }

        private void RemoveBuff(int count, bool isGood)
        {
            string tag = isGood ? "Good" : "Bad";
            List<BuffObj> buffObjs = ListPool<BuffObj>.Get();
            // 过滤出增益buff

            foreach (BuffObj buffObj in Buffs)
            {
                if (buffObj.Model.Tags.Contains(tag))
                {
                    buffObjs.Add(buffObj);
                }
            }

            if (buffObjs.IsNullOrEmpty())
            {
                return;
            }

            // 0表示删除全部
            if (count == 0)
            {
                count = buffObjs.Count;
            }
            else if (count > buffObjs.Count)
            {
                count = buffObjs.Count;
            }

            for (int i = 0; i < count; i++)
            {
                int random = Random.Range(0,buffObjs.Count);
                BuffObj removeBuffObj = buffObjs[random];
                buffObjs.Remove(removeBuffObj);
                Buffs.Remove(removeBuffObj);
                removeBuffObj.ExecuteBuff(EBuffEventType.OnRemoved, removeBuffObj);
            }

            HeroObj.RecheckProperty();
            ListPool<BuffObj>.Release(buffObjs);
        }

        public void Clear()
        {
            Buffs.Clear();
        }
    }
}