using System.Collections.Generic;

namespace MyGame
{
    public class BuffFunction
    {
        public static readonly Dictionary<EBuffEventType, Dictionary<string,BuffEvent>> Functions = new Dictionary<EBuffEventType, Dictionary<string, BuffEvent>>()
        {
            { EBuffEventType.OnOccur , new Dictionary<string, BuffEvent>
                {
                    
                }
            },
            { EBuffEventType.OnCast , new Dictionary<string, BuffEvent>
                {
                    
                }
            },
            { EBuffEventType.OnTick , new Dictionary<string, BuffEvent>
                {
                    {"EveryRoundDealDamage",EveryRoundDealDamage}
                }
            },
            { EBuffEventType.OnRemoved , new Dictionary<string, BuffEvent>
                {
                    
                }
            },
            { EBuffEventType.OnHit , new Dictionary<string, BuffEvent>
                {
                    
                }
            },
            { EBuffEventType.OnBeHurt , new Dictionary<string, BuffEvent>
                {
                    
                }
            },
            { EBuffEventType.OnKill , new Dictionary<string, BuffEvent>
                {
                    
                }
            },
            { EBuffEventType.OnBeKilled , new Dictionary<string, BuffEvent>
                {
                    
                }
            }
        };

        private static void EveryRoundDealDamage(BuffObj buff, params object[] args)
        {
            CreateDamageWarp warp = (CreateDamageWarp)buff.Model.EventWarps[EBuffEventType.OnTick].EventParameters[0];
            GameManager.Instance.GetService(out DamageManager dmgManager);
            dmgManager.AddDamage(buff.Caster,buff.Target,warp.ConvertWarpToDamage(buff.Caster.Property.Attack),buff.Caster.Property.CriticalRate,source:"Ignite");
        }
    }
}