using System;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace MyGame
{
    //  回调点的函数具体实现定义
    public class TimelineFunction
    {
        #region EventFunction

        public static readonly Dictionary<string, TimelineEvent> Functions = new Dictionary<string, TimelineEvent>
        {
            { "PlaySkillLihui", PlaySkillLihui },
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

        /// <summary>
        /// 播放技能立绘
        /// </summary>
        private static void PlaySkillLihui(TimelineObj obj, params object[] args)
        {
            string param1 = (string)args[0];
            Debug.Log($"播放立绘：{param1}");
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
            string effectName = args.Length >= 1 ? (string)args[0] : "";
            string bindPoint = args.Length >= 2 ? (string)args[1] : "Body";
            bool loop = args.Length >= 2 ? (bool)args[2] : false;
            obj.Caster.BindCom.AddBindGameObject(bindPoint, "Effects/" + effectName, effectName, loop);
        }

        private static void PlayEffectOnTarget(TimelineObj obj, params object[] args)
        {
            string effectName = args.Length >= 1 ? (string)args[0] : "";
            string bindPoint = args.Length >= 2 ? (string)args[1] : "Body";
            bool loop = args.Length >= 3 ? (bool)args[2] : false;

            List<HeroObj> targets = (List<HeroObj>)obj.GetParam("Targets");
            if (!targets.IsNullOrEmpty())
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    targets[i].BindCom.AddBindGameObject(bindPoint, "Effects/" + effectName, effectName, loop);
                }
            }
        }

        private static void CreateDamage(TimelineObj obj, params object[] args)
        {
            CreateDamageWarp damageWarp = (CreateDamageWarp)args[0];
            GameManager.Instance.GetService(out BattleManager battleManager);
            GameManager.Instance.GetService(out DamageManager dmgManager);

            List<HeroObj> targets = battleManager.GetFilterTargets(damageWarp, obj.Caster);
            if (targets.Count > 0)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    dmgManager.AddDamage(
                        obj.Caster,
                        targets[i],
                        damageWarp.ConvertWarpToDamage(obj.Caster.Property.Attack),
                        obj.Caster.Property.CriticalRate);
                }

                obj.LogicParams.Add("Targets", targets);
            }
        }

        private static void TransferBuff(TimelineObj obj, params object[] args)
        {
            TransferBuffWarp warp = (args.Length >= 1 ? (TransferBuffWarp)args[0] : default);
            List<HeroObj> targets = (List<HeroObj>)obj.GetParam("Targets");

            if (!targets.IsNullOrEmpty())
            {
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
            AddBuffWarp warp = args.Length >= 1 ? (AddBuffWarp)args[0] : null;
            if (warp == null) return;
            
            List<HeroObj> targets = (List<HeroObj>)obj.GetParam("Targets");
            if (targets.IsNullOrEmpty()) return;
            
            foreach (HeroObj heroObj in targets)
            {
                bool res = UnityEngine.Random.Range(0.00f, 1.00f) <= warp.Probability;
                if (!res) continue;
                
                AddBuffInfo addBuffInfo = warp.ConvertWarpToAddBuffInfo();
                addBuffInfo.Caster = obj.Caster;
                addBuffInfo.Target = heroObj;
                heroObj.BuffCom.AddBuff(addBuffInfo);
            }
        }

        private static void RemoveBuff(TimelineObj obj, params object[] args)
        {
            if (args.IsNullOrEmpty())
            {
                throw new Exception("[TimelineFunction] RemoveBuff warp was not found.");
            }
            
            RemoveBuffWarp warp = (RemoveBuffWarp)args[0];
            List<HeroObj> targets = (List<HeroObj>)obj.GetParam("Targets");
            if (targets.IsNullOrEmpty())
            { 
                throw new Exception("[TimelineFunction] RemoveBuff targets was not found.");
            }
            
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

        #endregion
    }
}