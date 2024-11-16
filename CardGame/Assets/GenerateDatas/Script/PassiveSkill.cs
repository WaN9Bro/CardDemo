
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

public sealed partial class PassiveSkill : Luban.BeanBase
{
    public PassiveSkill(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
        Id = (int)_obj.GetValue("Id");
        Group = (int)_obj.GetValue("Group");
        { var __json0 = _obj.GetValue("AutoAddBuff"); int _n0 = (__json0 as JArray).Count; AutoAddBuff = new AutoAddBuffWarp[_n0]; int __index0=0; foreach(JToken __e0 in __json0) { AutoAddBuffWarp __v0;  __v0 = AutoAddBuffWarp.DeserializeAutoAddBuffWarp(__e0);  AutoAddBuff[__index0++] = __v0; }   }
    }

    public static PassiveSkill DeserializePassiveSkill(JToken _buf)
    {
        return new PassiveSkill(_buf);
    }

    /// <summary>
    /// id
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 技能组
    /// </summary>
    public readonly int Group;
    /// <summary>
    /// 学习技能后自动添加的buff
    /// </summary>
    public readonly AutoAddBuffWarp[] AutoAddBuff;


    public const int __ID__ = -662757494;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
        foreach (var _e in AutoAddBuff) { _e?.ResolveRef(tables); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Group:" + Group + ","
        + "AutoAddBuff:" + Luban.StringUtil.CollectionToString(AutoAddBuff) + ","
        + "}";
    }
}
}

