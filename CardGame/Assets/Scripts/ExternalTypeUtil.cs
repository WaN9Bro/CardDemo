using System;
using System.Numerics;

public static class ExternalTypeUtil
{
    public static object NewObject(MyGame.MObject v)
    {
        
        object obj = new object();
        obj = v.Data;
        return obj;
    }
}