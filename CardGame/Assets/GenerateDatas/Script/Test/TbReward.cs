
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Newtonsoft.Json.Linq;
using Luban;



namespace cfg.Test
{

public partial class TbReward
{
    private readonly System.Collections.Generic.Dictionary<int, Test.Reward> _dataMap;
    private readonly System.Collections.Generic.List<Test.Reward> _dataList;
    
    public TbReward(JArray _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, Test.Reward>();
        _dataList = new System.Collections.Generic.List<Test.Reward>();
        
        foreach(JObject _ele in _buf)
        {
            Test.Reward _v;
            _v = Test.Reward.DeserializeReward(_ele);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
         }
    }


    public System.Collections.Generic.Dictionary<int, Test.Reward> DataMap => _dataMap;
    public System.Collections.Generic.List<Test.Reward> DataList => _dataList;

    public Test.Reward GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public Test.Reward Get(int key) => _dataMap[key];
    public Test.Reward this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}
}

