using System.Collections.Generic;

namespace MyGame
{
    public class BuffFunction
    {
        #region EventFunction
        
            public static Dictionary<string, BuffOnOccur> OnOccurFunc = new Dictionary<string, BuffOnOccur>
            {
                
            };
            
            public static Dictionary<string, BuffOnRemoved> OnRemovedFunc = new Dictionary<string, BuffOnRemoved>
            {
                
            };

            public static Dictionary<string, BuffOnTick> OnTickFunc = new Dictionary<string, BuffOnTick>
            {

            };

            public static Dictionary<string, BuffOnHit> OnHitFunc = new Dictionary<string, BuffOnHit>
            {

            };

            public static Dictionary<string, BuffOnBeHurt> OnBeHurtFunc = new Dictionary<string, BuffOnBeHurt>
            {

            };

            public static Dictionary<string, BuffOnKill> OnKillFunc = new Dictionary<string, BuffOnKill>
            {

            };

            public static Dictionary<string, BuffOnBeKilled> OnBeKilledFunc = new Dictionary<string, BuffOnBeKilled>
            {

            };

            public static Dictionary<string, BuffOnCast> OnCastFunc = new Dictionary<string, BuffOnCast>
            {

            };

            public static Dictionary<string, BuffOnRound> OnRoundFunc = new Dictionary<string, BuffOnRound>
            {
                {"Ignite",IgniteOnRound}
            };

            #endregion

        #region FunctionLogic

            private static void IgniteOnRound(BuffObj buffObj)
            {
                
            }

        #endregion
    }
}