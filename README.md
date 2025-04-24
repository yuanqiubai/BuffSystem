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
        this.buffData.buffName = "攻击提升";
        this.buffData.timeLife = 5f;
        this.buffData.intervalTime = 1.0f;          // buff的触发间隔时间

        this.OnActivate += this.BuffEffectOnActivate + MyBuffEffectOnActivate;
        this.OnUpdate += this.BuffEffectOnSustained;
        this.OnDeactivate += this.BuffEffectOnDeactivate;
    }

    AttackBuff(Buff buff)
        : base(buff) { }

    /*
    BuffEffectOnActivate方法是在 buff 激活时调用的，它在 buff 的生命周期中只会被调用一次。
    你可以在这里设置 buff 的初始效果，比如提升攻击力、增加生命值等。
    */
    public void BuffEffectOnActivate(ActorStats target)
    {
        Debug.Log(target.name + " 攻击Buff激活");
        target.attack = 20;
    }

    /*
    BuffEffectOnSustained方法是在buff持续时调用的，它会在buff的生命周期中根据 intervalTime 调用。
    假如你的 intervalTime = 1.0f，那么这个方法会在 buff 激活后每隔 1 秒调用一次。
    */
    public void BuffEffectOnSustained(ActorStats target)
    {
        Debug.Log(target.name + " 攻击Buff持续");
        target.attack += 5;
    }

    /*
    BuffEffectOnDeactivate方法是在 buff 结束时调用的，它在 buff 的生命周期中只会被调用一次。
    你可以在这里清除 buff 的效果，比如恢复攻击力、减少生命值等。
    */
    public void BuffEffectOnDeactivate(ActorStats target)
    {
        Debug.Log(target.name +  " 攻击Buff结束");
        target.attack = 10;
    }

    /*
    我猜测你可能会想要在激活时调用多个委托
    你也可以在这里定义多个委托
    但是你需要注意: 你需要在构造函数中注册它们,否则它们不会被调用！
    不过Buff的生命周期只有3个阶段，我觉得你只需要实现 IBuffEffect 接口中的3个委托就可以了。
    */
    BuffEffectDelegate MyBuffEffectOnActivate = 
        (n) => 
        {
            n.attack += 20; 
            Debug.Log(n.name + "攻击Buff激活时调用第二个委托");
        };
}
```
自定义新的Buff有几个注意事项：
1. Buff类的构造函数中需要设置buffData的属性:
   - buffName: buff的名称
   - timeLife: buff的持续时间
   - intervalTime: buff的触发间隔时间
2. Buff类的构造函数中需要注册OnActivate、OnUpdate、OnDeactivate事件
   - 这三个事件分别对应buff的激活、持续和结束阶段
   - 你可以继承IBuffEffect接口，来实现自己的buff效果
   - 你也可以不继承IBuffEffect接口，直接在Buff类中实现自己的buff效果，但是你的委托类型必须是BuffEffectDelegate，并且在构造函数中注册它们

# 使用流程图

场景需求:
```requirement
    
    场景列表:
        BuffManager----------------> Add BuffManager Component
        ActorsContainer------------> Add ActorsContainer Component
        Player---------------------> Add ActorStats Component
        Enemy----------------------> Add ActorStats Component

```

脚本调用方式:
```csharp
    
    // 创建一个处理玩家攻击的脚本
    public class PlayerAttackBox : MonoBehaviour
    {
        void OnTriggerEnter(Collider2D other)
        {
            if (other.CompareTag("Enemy"))      // 你需要修改Enemy的tag为"Enemy"
            {
                // 玩家攻击敌人
                AttackBuff attackBuff = new AttackBuff();
                BuffManager.Instance.PublishBuffBuffer(other.gameObject.GetComponent<ActorStats>(), attackBuff);
            }
        }
    }

```


