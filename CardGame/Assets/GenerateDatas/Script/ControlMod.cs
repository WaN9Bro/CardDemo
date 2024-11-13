
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

public partial struct ControlMod
{
    public ControlMod(JToken _buf) 
    {
        JObject _obj = _buf as JObject;
        CanAttack = (bool)_obj.GetValue("CanAttack");
        CanSkill = (bool)_obj.GetValue("CanSkill");
        CanDead = (bool)_obj.GetValue("CanDead");
        CanBeHurt = (bool)_obj.GetValue("CanBeHurt");
    }

    public static ControlMod DeserializeControlMod(JToken _buf)
    {
        return new ControlMod(_buf);
    }

    /// <summary>
    /// 是否可以进行普攻
    /// </summary>
    public readonly bool CanAttack;
    /// <summary>
    /// 是否可以释放技能
    /// </summary>
    public readonly bool CanSkill;
    /// <summary>
    /// 是否可以死亡
    /// </summary>
    public readonly bool CanDead;
    /// <summary>
    /// 是否可以受到伤害
    /// </summary>
    public readonly bool CanBeHurt;



    public  void ResolveRef(Tables tables)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "CanAttack:" + CanAttack + ","
        + "CanSkill:" + CanSkill + ","
        + "CanDead:" + CanDead + ","
        + "CanBeHurt:" + CanBeHurt + ","
        + "}";
    }
}
}
