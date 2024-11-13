
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using Newtonsoft.Json.Linq;



namespace MyGame
{

public partial struct PlayEffectWarp
{
    public PlayEffectWarp(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
        BindPoint = (string)_obj.GetValue("BindPoint");
        EffectName = (string)_obj.GetValue("EffectName");
        Duration = (float)_obj.GetValue("Duration");
        IsLoop = (bool)_obj.GetValue("IsLoop");
    }

    public static PlayEffectWarp DeserializePlayEffectWarp(JToken _buf)
    {
        return new PlayEffectWarp(_buf);
    }

    /// <summary>
    /// 绑点
    /// </summary>
    public readonly string BindPoint;
    /// <summary>
    /// 特效名称
    /// </summary>
    public readonly string EffectName;
    /// <summary>
    /// 持续时间
    /// </summary>
    public readonly float Duration;
    /// <summary>
    /// 循环
    /// </summary>
    public readonly bool IsLoop;



    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "BindPoint:" + BindPoint + ","
        + "EffectName:" + EffectName + ","
        + "Duration:" + Duration + ","
        + "IsLoop:" + IsLoop + ","
        + "}";
    }
}
}

