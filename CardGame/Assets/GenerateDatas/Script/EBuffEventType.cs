
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



namespace MyGame
{ 
    public enum EBuffEventType
    {
        /// <summary>
        /// buff在被添加、改变层数时候触发的事件
        /// </summary>
        OnOccur = 0,
        /// <summary>
        /// 在释放技能的时候运行的buff，执行这个buff获得最终技能要产生的Timeline
        /// </summary>
        OnCast = 1,
        /// <summary>
        /// buff在每个工作周期会执行的函数，如果这个函数为空，或者tickTime&lt;=0，都不会发生周期性工作
        /// </summary>
        OnTick = 2,
        /// <summary>
        /// 在这个buffObj被移除之前要做的事情，如果运行之后buffObj又不足以被删除了就会被保留
        /// </summary>
        OnRemoved = 3,
        /// <summary>
        /// 在伤害流程中，持有这个buff的人作为攻击者会发生的事情
        /// </summary>
        OnHit = 4,
        /// <summary>
        /// 在伤害流程中，持有这个buff的人作为挨打者会发生的事情
        /// </summary>
        OnBeHurt = 5,
        /// <summary>
        /// 在伤害流程中，如果击杀目标，则会触发的啥事情
        /// </summary>
        OnKill = 6,
        /// <summary>
        /// 在伤害流程中，持有这个buff的人被杀死了，会触发的事情
        /// </summary>
        OnBeKilled = 7,
    }

} 

