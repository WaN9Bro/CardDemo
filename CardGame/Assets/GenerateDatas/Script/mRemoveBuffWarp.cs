
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

public sealed partial class mRemoveBuffWarp : mObject
{
    public mRemoveBuffWarp(JToken _buf)  : base(_buf) 
    {
        JObject _obj = _buf as JObject;
        Data = RemoveBuffWarp.DeserializeRemoveBuffWarp(_obj.GetValue("data"));
    }

    public static mRemoveBuffWarp DeserializemRemoveBuffWarp(JToken _buf)
    {
        return new mRemoveBuffWarp(_buf);
    }

    public readonly RemoveBuffWarp Data;


    public const int __ID__ = 1290442732;
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

