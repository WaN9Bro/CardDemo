
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

public sealed partial class mCreateDamageWarp : mObject
{
    public mCreateDamageWarp(JToken _buf)  : base(_buf) 
    {
        JObject _obj = _buf as JObject;
        Data = CreateDamageWarp.DeserializeCreateDamageWarp(_obj.GetValue("data"));
    }

    public static mCreateDamageWarp DeserializemCreateDamageWarp(JToken _buf)
    {
        return new mCreateDamageWarp(_buf);
    }

    public readonly CreateDamageWarp Data;


    public const int __ID__ = 689798976;
    public override int GetTypeId() => __ID__;

    public override void ResolveRef(Tables tables)
    {
        base.ResolveRef(tables);
    }

    public override string ToString()
    {
        return "{ "
        + "data:" + Data + ","
        + "}";
    }
}
}
