using System.Collections.Generic;
using System.Resources;
using UnityEditor;

namespace MyGame
{
    public static class MObjectHelper
    {
        public static List<object> ConvertMObjectToObject(List<mObject> self)
        {
            List<object> objects = new List<object>(self.Count);

            foreach (mObject mObject in self)
            {
                objects.Add(ConvertMObjectToObject(mObject));
            }

            return objects;
        }
        
        public static List<object> ConvertMObjectToObject(this mObject[] self)
        {
            List<object> objects = new List<object>(self.Length);

            foreach (mObject mObject in self)
            {
                objects.Add(ConvertMObjectToObject(mObject));
            }

            return objects;
        }

        public static object ConvertMObjectToObject(this mObject mObj)
        {
            if (mObj is mInt mInt)
            {
                return mInt.Data;
            }
            
            if (mObj is mFloat mFloat)
            {
                return mFloat.Data;
            }
            
            if (mObj is mString mString)
            {
                return mString.Data;
            }
            
            if (mObj is mBool mBool)
            {
                return mBool.Data;
            }
            
            if (mObj is mCreateDamageWarp mCreateDamageWarp)
            {
                return mCreateDamageWarp.Data;
            }
            
            if (mObj is mAddBuffWarp mAddBuffWarp)
            {
                return mAddBuffWarp.Data;
            }
            
            if (mObj is mTransferBuffWarp mTransferBuffWarp)
            {
                return mTransferBuffWarp.Data;
            }
            
            if (mObj is mRemoveBuffWarp mRemoveBuffWarp)
            {
                return mRemoveBuffWarp.Data;
            }
            
            if (mObj is mPlayEffectWarp mPlayEffectWarp)
            {
                return mPlayEffectWarp.Data;
            }
            
            if (mObj is mTargetWarp mTargetWarp)
            {
                return mTargetWarp.Data;
            }
            
            if (mObj is mAddBuffTableIndex mAddBuffTableIndex)
            {
                return mAddBuffTableIndex;
            }

            return null;
        }
    }
}