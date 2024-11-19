using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = Unity.Mathematics.Random;

namespace MyGame
{
    //  回调点的函数具体实现定义
    public class TimelineFunction
    {
        #region EventFunction

        public static readonly Dictionary<string, TimelineEvent> Functions = new Dictionary<string, TimelineEvent>
        {
            {"TargetInit", TargetInit},
            { "PlayAnimOnCaster", PlayAnimOnCaster },
            { "PlayEffectOnCaster", PlayEffectOnCaster },
            { "PlayEffectOnTarget", PlayEffectOnTarget },
            { "CreateDamage", CreateDamage },
            { "TransferBuff", TransferBuff },
            { "RemoveBuff", RemoveBuff },
            { "AddBuff", AddBuff },
        };

        #endregion

        #region FunctionLogic
        
        
        private static void TargetInit(TimelineObj obj, params object[] args)
        {
            TargetWarp warp = (TargetWarp)args[0];
            GameManager.Instance.GetService(out BattleManager battleManager);
            List<HeroObj> targets = battleManager.GetFilterTargets(warp, obj.Caster);
            if (targets.Count > 0)
            {
                obj.LogicParams.Add("Targets", new List<HeroObj>(targets));
            }
        }
        
        /// <summary>
        /// 播放施法者指定动画
        /// </summary>
        private static void PlayAnimOnCaster(TimelineObj obj, params object[] args)
        {
            string animName = (string)args[0];
            obj.Caster.SpineCom.PlayAnim(animName, false);
        }

        /// <summary>
        /// 在施法者身上播放一个特效
        /// </summary>
        private static void PlayEffectOnCaster(TimelineObj obj, params object[] args)
        {
            PlayEffectWarp warp = (PlayEffectWarp)args[0];
            obj.Caster.BindCom.AddBindGameObject(warp.BindPoint, "Effects/" + warp.EffectName, warp.EffectName);
        }

        private static void PlayEffectOnTarget(TimelineObj obj, params object[] args)
        {
            PlayEffectWarp warp = (PlayEffectWarp)args[0];

            if (obj.LogicParams.TryGetValue("Targets", out object targetObjs))
            {
                List<HeroObj> targets = (List<HeroObj>)targetObjs;
                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].BindCom.AddBindGameObject(warp.BindPoint, "Effects/" + warp.EffectName, warp.EffectName);
                }
            }
        }

        private static void CreateDamage(TimelineObj obj, params object[] args)
        {
            CreateDamageWarp damageWarp = (CreateDamageWarp)args[0];
            GameManager.Instance.GetService(out DamageManager dmgManager);

            if (obj.LogicParams.TryGetValue("Targets",out object targetObjs))
            {
                List<HeroObj> targets = (List<HeroObj>)targetObjs;
                for (int i = 0; i < targets.Count; i++)
                {
                    dmgManager.AddDamage(
                        obj.Caster,
                        targets[i],
                        damageWarp.ConvertWarpToDamage(obj.Caster.Property.Attack),
                        obj.Caster.Property.CriticalRate,damageWarp.BeHurtEffect,obj.Model.Id);
                }
            }
        }

        private static void TransferBuff(TimelineObj obj, params object[] args)
        {
            TransferBuffWarp warp = (args.Length >= 1 ? (TransferBuffWarp)args[0] : default);
            if (obj.LogicParams.TryGetValue("Targets", out object targetObjs))
            {
                List<HeroObj> targets = (List<HeroObj>)targetObjs;
                GameManager.Instance.GetService(out BattleManager battleManager);

                EFaction faction = obj.Caster.FactionType == EFaction.Player ? EFaction.Enemy : EFaction.Player;
                List<HeroObj> noHasBuffHeroObjs = battleManager.GetNoHasBuffHeroObjs(warp.BuffKey, faction);
                if (noHasBuffHeroObjs.IsNullOrEmpty()) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i].BuffCom.HasBuff(warp.BuffKey, out BuffObj buffObj))
                    {
                        bool res = UnityEngine.Random.Range(0.00f, 1.00f) <= warp.Probability;
                        if (res && !noHasBuffHeroObjs.IsNullOrEmpty())
                        {
                            AddBuffInfo addBuffInfo = new AddBuffInfo(
                                obj.Caster, targets[i], buffObj.Model, 1, true, buffObj.Permanent, buffObj.Duration,
                                buffObj.BuffParams);

                            noHasBuffHeroObjs[i].BuffCom.AddBuff(addBuffInfo);
                        }
                    }
                }
            }
        }

        private static void AddBuff(TimelineObj obj, params object[] args)
        {
            mAddBuffTableIndex addBuffTableIndex = (mAddBuffTableIndex)args[0];
            AddBuffDefine addBuffDefine = addBuffTableIndex.Config();

            if (obj.LogicParams.TryGetValue("Targets", out object targetObjs))
            {
                List<HeroObj> targets = (List<HeroObj>)targetObjs;
                foreach (HeroObj heroObj in targets)
                {
                    foreach (AddBuffWarp warp in addBuffDefine.AddBuff)
                    {
                        bool res = UnityEngine.Random.Range(0.00f, 1.00f) <= warp.Probability;
                        if (!res) continue;

                        AddBuffInfo addBuffInfo = warp.ConvertWarpToAddBuffInfo();
                        addBuffInfo.Caster = obj.Caster;
                        addBuffInfo.Target = heroObj;
                        heroObj.BuffCom.AddBuff(addBuffInfo);
                    }
                }
            }
        }

        private static void RemoveBuff(TimelineObj obj, params object[] args)
        {
            if (args.IsNullOrEmpty())
            {
                throw new Exception("[TimelineFunction] RemoveBuff warp was not found.");
            }
            
            RemoveBuffWarp warp = (RemoveBuffWarp)args[0];

            if (obj.LogicParams.TryGetValue("Targets", out object targetObjs))
            {
                List<HeroObj> targets = (List<HeroObj>)targetObjs;
                foreach (HeroObj heroObj in targets)
                {
                    bool res = UnityEngine.Random.Range(0.00f, 1.00f) <= warp.Probability;
                    if (!res) continue;

                    switch (warp.RemoveBuffType)
                    {
                        // 指定buff
                        case ERemoveBuffType.AssignBuff:
                            heroObj.BuffCom.RemoveBuff(warp.BuffKey);
                            break;
                        case ERemoveBuffType.RandomBuff:
                            heroObj.BuffCom.RemoveBuffByRandom(warp.Count);
                            break;
                        case ERemoveBuffType.RandomDebuff:
                            heroObj.BuffCom.RemoveDebuffByRandom(warp.Count);
                            break;
                    }
                }
            }
        }

        #endregion
    }
}