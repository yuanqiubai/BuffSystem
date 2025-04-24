# BuffSystem
这是适用于Unity的Buff系统，我为它提供了更自由的Buff实现方式
你可以完全自定义自己的buff

# 怎么自定义Buff？
首先你需要创建一个继承自Buff和IBuffEffect的类。
然后实现IBuffEffect接口中的三个方法

下面是一个简单的攻击提升Buff示例:
```csharp
public class AttackBuff : Buff, IBuffEffect
{
    public AttackBuff() 
    {
        this.buffData.buffID = "1";
        this.buffData.buffName = "攻击提升";
        this.buffData.timeLife = 5f;
        this.buffData.intervalTime = 1.0f;

        this.OnActivate += this.BuffEffectOnActivate;
        this.OnUpdate += this.BuffEffectOnSustained;
        this.OnDeactivate += this.BuffEffectOnDeactivate;
    }

    AttackBuff(Buff buff)
        : base(buff) { }

    public void BuffEffectOnActivate(ActorStats target)
    {
        Debug.Log(target.name + " 攻击Buff激活");
        target.attack = 20;
    }

    public void BuffEffectOnSustained(ActorStats target)
    {
        Debug.Log(target.name + " 攻击Buff持续");
    }

    public void BuffEffectOnDeactivate(ActorStats target)
    {
        Debug.Log(target.name +  " 攻击Buff结束");
        target.attack = 10;
    }
}
```
自定义新的Buff有几个注意事项：
1. Buff类的构造函数中需要设置buffData的属性
2. Buff类的构造函数中需要注册OnActivate、OnUpdate、OnDeactivate事件