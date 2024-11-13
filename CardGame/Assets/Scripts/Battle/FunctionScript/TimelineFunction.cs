using System.Collections.Generic;
using UnityEngine;

namespace MyGame
{
    //  回调点的函数具体实现定义
    public class TimelineFunction
    {
        #region EventFunction

            public static Dictionary<string, TimelineEvent> Functions = new Dictionary<string, TimelineEvent>
            {
                { "PlaySkillLihui", PlaySkillLihui },
                { "PlayAnimOnCaster", PlayAnimOnCaster },
                { "PlayEffectOnCaster", PlayEffectOnCaster },
                { "PlayEffectOnTarget", PlayEffectOnTarget },
                { "CreateDamage", CreateDamage },
                { "TransferBuff", TransferBuff },
                { "RemoveBuff", RemoveBuff }
            };

        #endregion

        #region FunctionLogic

            /// <summary>
            /// 播放技能立绘
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="args"></param>
            private static void PlaySkillLihui(TimelineObj obj, params object[] args)
            {
                string param1 = (string)args[0];
                Debug.Log($"播放立绘：{param1}");
            }
            
            /// <summary>
            /// 播放施法者指定动画
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="args"></param>
            private static void PlayAnimOnCaster(TimelineObj obj, params object[] args)
            {
                string animName = (string)args[0];
                obj.Caster.SpineCom.PlayAnim(animName,false);
            }
            
            /// <summary>
            /// 在施法者身上播放一个特效
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="args"></param>
            private static void PlayEffectOnCaster(TimelineObj obj, params object[] args)
            {
                string effectName = (string)args[0];
                // 生成特效对象，
                
            }
            
            private static void PlayEffectOnTarget(TimelineObj obj, params object[] args)
            {
                
            }
            
            private static void CreateDamage(TimelineObj obj, params object[] args)
            {
                
            }
            
            private static void TransferBuff(TimelineObj obj, params object[] args)
            {
                
            }
            
            private static void RemoveBuff(TimelineObj obj, params object[] args)
            {
                
            }

        #endregion
        
    }
}