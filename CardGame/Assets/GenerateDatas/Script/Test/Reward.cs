
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using Newtonsoft.Json.Linq;



namespace cfg.Test
{

public sealed partial class Reward : Luban.BeanBase
{
    public Reward(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
        Id = (int)_obj.GetValue("id");
        Name = (string)_obj.GetValue("name");
        Count = (int)_obj.GetValue("count");
    }

    public static Reward DeserializeReward(JToken _buf)
    {
        return new Test.Reward(_buf);
    }

    /// <summary>
    /// id
    /// </summary>
    public readonly int Id;
    /// <summary>
    /// 名称
    /// </summary>
    public readonly string Name;
    /// <summary>
    /// 个数
    /// </summary>
    public readonly int Count;


    public const int __ID__ = -69379061;
    public override int GetTypeId() => __ID__;

    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "id:" + Id + ","
        + "name:" + Name + ","
        + "count:" + Count + ","
        + "}";
    }
}
}

