
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

public sealed partial class AddBuffWarp : Luban.BeanBase
{
    public AddBuffWarp(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
        Probability = (float)_obj.GetValue("Probability");
        BuffKey = (string)_obj.GetValue("BuffKey");
        AddStack = (int)_obj.GetValue("AddStack");
        TimeModify = (bool)_obj.GetValue("TimeModify");
        Permanent = (bool)_obj.GetValue("Permanent");
        Duration = (int)_obj.GetValue("Duration");
        { var __json0 = _obj.GetValue("PropMod"); PropMod = new System.Collections.Generic.Dictionary<EPropertyModType, mObject>((__json0 as JArray).Count); foreach(JToken __e0 in __json0) { EPropertyModType _k0;  _k0 = (EPropertyModType)(int)__e0[0]; mObject _v0;  _v0 = mObject.DeserializemObject(__e0[1]);  PropMod.Add(_k0, _v0); }   }
        { var __json0 = _obj.GetValue("ControlMod"); ControlMod = new System.Collections.Generic.Dictionary<EControlModType, mObject>((__json0 as JArray).Count); foreach(JToken __e0 in __json0) { EControlModType _k0;  _k0 = (EControlModType)(int)__e0[0]; mObject _v0;  _v0 = mObject.DeserializemObject(__e0[1]);  ControlMod.Add(_k0, _v0); }   }
        { var __json0 = _obj.GetValue("EventValueWarp"); int _n0 = (__json0 as JArray).Count; EventValueWarp = new BuffEventValueWarp[_n0]; int __index0=0; foreach(JToken __e0 in __json0) { BuffEventValueWarp __v0;  __v0 = BuffEventValueWarp.DeserializeBuffEventValueWarp(__e0);  EventValueWarp[__index0++] = __v0; }   }
    }

    public static AddBuffWarp DeserializeAddBuffWarp(JToken _buf)
    {
        return new AddBuffWarp(_buf);
    }

    /// <summary>
    /// 概率
    /// </summary>
    public readonly float Probability;
    /// <summary>
    /// Buff表的Key
    /// </summary>
    public readonly string BuffKey;
    /// <summary>
    /// 添加的Buff层数
    /// </summary>
    public readonly int AddStack;
    /// <summary>
    /// Buff的持续时间修改，true是直接设置，flase是改变
    /// </summary>
    public readonly bool TimeModify;
    /// <summary>
    /// 是否永久Buff
    /// </summary>
    public readonly bool Permanent;
    /// <summary>
    /// 持续回合
    /// </summary>
    public readonly int Duration;
    /// <summary>
    /// 修改的属性
    /// </summary>
    public readonly System.Collections.Generic.Dictionary<EPropertyModType, mObject> PropMod;
    /// <summary>
    /// 修改的行为
    /// </summary>
    public readonly System.Collections.Generic.Dictionary<EControlModType, mObject> ControlMod;
    /// <summary>
    /// 触发时机参数
    /// </summary>
    public readonly BuffEventValueWarp[] EventValueWarp;


    public const int __ID__ = -828880612;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        foreach (var _e in PropMod.Values) { _e?.ResolveRef(tables); }
        foreach (var _e in ControlMod.Values) { _e?.ResolveRef(tables); }
        foreach (var _e in EventValueWarp) { _e?.ResolveRef(tables); }
    }

    public override string ToString()
    {
        return "{ "
        + "Probability:" + Probability + ","
        + "BuffKey:" + BuffKey + ","
        + "AddStack:" + AddStack + ","
        + "TimeModify:" + TimeModify + ","
        + "Permanent:" + Permanent + ","
        + "Duration:" + Duration + ","
        + "PropMod:" + Luban.StringUtil.CollectionToString(PropMod) + ","
        + "ControlMod:" + Luban.StringUtil.CollectionToString(ControlMod) + ","
        + "EventValueWarp:" + Luban.StringUtil.CollectionToString(EventValueWarp) + ","
        + "}";
    }
}
}

