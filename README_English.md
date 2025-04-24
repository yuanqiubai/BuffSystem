# BuffSystem
This is a Buff system designed for Unity, offering a more flexible approach to implementing Buffs.
You can fully customize your own Buffs.

# How to Customize a Buff?
Create a class that inherits from Buff and implements the IBuffEffect interface.
Implement the three methods defined in IBuffEffect:
1. BuffEffectOnActivate: Triggered when the Buff activates (once per lifetime).
2. BuffEffectOnSustained: Triggered repeatedly at intervals (intervalTime).
3. BuffEffectOnDeactivate: Triggered when the Buff ends (once per lifetime).

Example: Attack Boost Buff
```csharp
	public class AttackBuff : Buff, IBuffEffect  
    {  
        public AttackBuff()  
        {  
            this.buffData.buffName = "Attack Boost";  
            this.buffData.timeLife = 5f;  
            this.buffData.intervalTime = 1.0f; // Buff trigger interval  

            // Register event handlers  
            this.OnActivate += this.BuffEffectOnActivate + MyBuffEffectOnActivate;  
            this.OnUpdate += this.BuffEffectOnSustained;  
            this.OnDeactivate += this.BuffEffectOnDeactivate;  
        }  

        AttackBuff(Buff buff)  
            : base(buff) { }  

        // Called once when the Buff activates  
        public void BuffEffectOnActivate(ActorStats target)  
        {  
            Debug.Log(target.name + ": Attack Buff activated");  
            target.attack = 20;  
        }  

        // Called repeatedly (every intervalTime)  
        public void BuffEffectOnSustained(ActorStats target)  
        {  
            Debug.Log(target.name + ": Attack Buff sustained");  
            target.attack += 5;  
        }  

        // Called once when the Buff ends  
        public void BuffEffectOnDeactivate(ActorStats target)  
        {  
            Debug.Log(target.name + ": Attack Buff ended");  
            target.attack = 10;  
        }  

        // Additional activation delegate (optional)  
        BuffEffectDelegate MyBuffEffectOnActivate =  
            (n) =>  
            {  
                n.attack += 20;  
                Debug.Log(n.name + ": Second activation delegate called");  
            };  
    }  
```

# Key Notes                                                                                   
1. Constructor Setup:Set buffData properties:
- buffName: Buff name
- timeLife: Duration (seconds)
- intervalTime: Interval between sustained effects
- Register events: OnActivate, OnUpdate, OnDeactivate
Register events: OnActivate, OnUpdate, OnDeactivate.

2. Implementation Options:
Implement IBuffEffect for structured lifecycle handling.
Alternatively, use BuffEffectDelegate directly (ensure delegates match method signatures).

# Usage Workflow
Scene Setup
1. Required Components:
- BuffManager: Add to a GameObject (e.g., "Systems").
- ActorsContainer: Add to a GameObject to manage actors.
- ActorStats: Add to both Player and Enemy GameObjects.

Applying Buffs
```csharp
    // Example: Apply Buff on player attack  
    public class PlayerAttackBox : MonoBehaviour  
    {  
        void OnTriggerEnter(Collider2D other)  
        {  
            if (other.CompareTag("Enemy"))  
            {  
                // Create and apply AttackBuff  
                AttackBuff attackBuff = new AttackBuff();  
                BuffManager.Instance.PublishBuffBuffer(  
                    other.GetComponent<ActorStats>(),  
                    attackBuff  
                );  
            }  
        }  
    }  
```
Key Methods : BuffManager.Instance.PublishBuffBuffer(): Applies a Buff to a target ActorStats.

# Lifecycle Overview
1. Activation Phase:
BuffEffectOnActivate is called once.
Ideal for initial stat modifications.

2. Sustained Phase:
BuffEffectOnSustained triggers every intervalTime.
Use for periodic effects (e.g., damage over time).

3. Deactivation Phase:
BuffEffectOnDeactivate cleans up effects when the Buff expires.
Restore modified stats here.

# Advanced Tips
Multiple Delegates: Register additional delegates for events (e.g., OnActivate += Method1 + Method2).
Buff Stacking: Implement custom logic in BuffEffectOnSustained for stacking effects.
Data-Driven Design: Extend BuffData to include custom fields (e.g., effect magnitude).

This system provides a foundation for implementing diverse Buff mechanics while maintaining flexibility and scalability.