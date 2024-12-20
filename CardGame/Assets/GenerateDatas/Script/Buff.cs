
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

public sealed partial class Buff : Luban.BeanBase
{
    public Buff(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
        Id = (string)_obj.GetValue("Id");
        Name = (string)_obj.GetValue("Name");
        { var __json0 = _obj.GetValue("Tags"); int _n0 = (__json0 as JArray).Count; Tags = new string[_n0]; int __index0=0; foreach(JToken __e0 in __json0) { string __v0;  __v0 = (string)__e0;  Tags[__index0++] = __v0; }   }
        { var __json0 = _obj.GetValue("PropMod"); PropMod = new System.Collections.Generic.Dictionary<EPropertyModType, mObject>((__json0 as JArray).Count); foreach(JToken __e0 in __json0) { EPropertyModType _k0;  _k0 = (EPropertyModType)(int)__e0[0]; mObject _v0;  _v0 = mObject.DeserializemObject(__e0[1]);  PropMod.Add(_k0, _v0); }   }
        { var __json0 = _obj.GetValue("ControlMod"); ControlMod = new System.Collections.Generic.Dictionary<EControlModType, mObject>((__json0 as JArray).Count); foreach(JToken __e0 in __json0) { EControlModType _k0;  _k0 = (EControlModType)(int)__e0[0]; mObject _v0;  _v0 = mObject.DeserializemObject(__e0[1]);  ControlMod.Add(_k0, _v0); }   }
        Priority = (int)_obj.GetValue("Priority");
        MaxStack = (int)_obj.GetValue("MaxStack");
        TickTime = (int)_obj.GetValue("TickTime");
        { var __json0 = _obj.GetValue("Event"); Event = new System.Collections.Generic.Dictionary<EBuffEventType, string>((__json0 as JArray).Count); foreach(JToken __e0 in __json0) { EBuffEventType _k0;  _k0 = (EBuffEventType)(int)__e0[0]; string _v0;  _v0 = (string)__e0[1];  Event.Add(_k0, _v0); }   }
    }

    public static Buff DeserializeBuff(JToken _buf)
    {
        return new Buff(_buf);
    }

    /// <summary>
    /// id
    /// </summary>
    public readonly string Id;
    /// <summary>
    /// 名称
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// 标记
    /// </summary>
    public readonly string[] Tags;
    public readonly System.Collections.Generic.Dictionary<EPropertyModType, mObject> PropMod;
    public readonly System.Collections.Generic.Dictionary<EControlModType, mObject> ControlMod;
    /// <summary>
    /// 优先级
    /// </summary>
    public readonly int Priority;
    /// <summary>
    /// 可堆叠最大层数
    /// </summary>
    public readonly int MaxStack;
    /// <summary>
    /// 工作周期，单位回合
    /// </summary>
    public readonly int TickTime;
    /// <summary>
    /// 时机方法
    /// </summary>
    public readonly System.Collections.Generic.Dictionary<EBuffEventType, string> Event;


    public const int __ID__ = 2081907;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        foreach (var _e in PropMod.Values) { _e?.ResolveRef(tables); }
        foreach (var _e in ControlMod.Values) { _e?.ResolveRef(tables); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Name:" + Name + ","
        + "Tags:" + Luban.StringUtil.CollectionToString(Tags) + ","
        + "PropMod:" + Luban.StringUtil.CollectionToString(PropMod) + ","
        + "ControlMod:" + Luban.StringUtil.CollectionToString(ControlMod) + ","
        + "Priority:" + Priority + ","
        + "MaxStack:" + MaxStack + ","
        + "TickTime:" + TickTime + ","
        + "Event:" + Luban.StringUtil.CollectionToString(Event) + ","
        + "}";
    }
}
}

