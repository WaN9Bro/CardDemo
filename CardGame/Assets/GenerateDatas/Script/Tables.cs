
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Newtonsoft.Json.Linq;

namespace cfg
{
public partial class Tables
{
    public Test.TbReward TbReward {get; }


      public Tables(System.Func<string, JArray> loader)
    {
        TbReward = new Test.TbReward(loader("test_tbreward"));
        ResolveRef();
    }
    
     private void ResolveRef()
    {
        TbReward.ResolveRef(this);
    }
}

}

