
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

public partial struct CallbackWarp
{
    public CallbackWarp(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
        FuncKey = (string)_obj.GetValue("FuncKey");
        { var __json0 = _obj.GetValue("Params"); int _n0 = (__json0 as JArray).Count; Params = new System.Object[_n0]; int __index0=0; foreach(JToken __e0 in __json0) { System.Object __v0;  __v0 = ExternalTypeUtil.NewObject(MObject.DeserializeMObject(__e0));  Params[__index0++] = __v0; }   }
    }

    public static CallbackWarp DeserializeCallbackWarp(JToken _buf)
    {
        return new CallbackWarp(_buf);
    }

    public readonly string FuncKey;
    public readonly System.Object[] Params;



    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "FuncKey:" + FuncKey + ","
        + "Params:" + Luban.StringUtil.CollectionToString(Params) + ","
        + "}";
    }
}
}

