
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

public abstract partial class mObject : Luban.BeanBase
{
    public mObject(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
    }

    public static mObject DeserializemObject(JToken _buf)
    {
        var _obj=_buf as JObject;
        switch (_obj.GetValue("$type").ToString())
        {
            case "mInt": return new mInt(_buf);
            case "mFloat": return new mFloat(_buf);
            case "mString": return new mString(_buf);
            case "mCreateDamageWarp": return new mCreateDamageWarp(_buf);
            case "mAddBuffWarp": return new mAddBuffWarp(_buf);
            default: throw new SerializationException();
        }
    }




    public virtual void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "}";
    }
}
}
