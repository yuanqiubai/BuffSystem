using System;
using UnityEngine;
using ActorStatsSystem;

namespace BuffSystem
{
    /*
    这是实现buff效果的接口，派生的buff类需要继承它
    This is the interface for implementing Buff effects. Derived Buff classes need to inherit it.
    */
    public interface IBuffEffect
    {
        /// <summary>
        /// Buff效果激活时的效果
        /// 使用规则：派生Buff类时，实现Buff激活时刻的效果
        /// 
        /// Effect when the Buff is activated.
        /// Usage: Implement the activation effect in derived Buff classes.
        /// </summary>
        void BuffEffectOnActivate(ActorStats target);

        /// <summary>
        /// Buff持续期间的效果
        /// 使用规则：派生Buff类时，实现Buff持续期间的效果
        /// 
        /// Effect during the Buff's duration.
        /// Usage: Implement the sustained effect in derived Buff classes.
        /// </summary>
        void BuffEffectOnSustained(ActorStats target);

        /// <summary>
        /// Buff结束时刻的效果
        /// 使用规则：派生Buff类时，实现Buff结束时刻的效果
        /// 
        /// Effect when the Buff ends.
        /// Usage: Implement the deactivation effect in derived Buff classes.
        /// </summary>
        void BuffEffectOnDeactivate(ActorStats target);
    }

    /*
    BuffData是buff的基本数据。
    BuffData is the basic data structure for Buffs.
    */
    public class BuffData
    {
        public Sprite buffIcon;                 // Buff图标
                                                // Buff Icon
        public string IconPath;                 // Buff图标路径
                                                // Path to the Buff icon

        public string buffName;                 // Buff名称
                                                // Buff Name

        public float timeLife = 5;              // Buff持续时间 (-1为无限时间)
                                                // Buff duration (-1 for infinite duration)
        public float currentTime = 0;           // Buff当前时间
                                                // Current Buff time

        // 新增间隔触发相关参数
        // Added parameters for interval-based effects
        public float intervalTime = 1f;         // 效果触发间隔（秒）
                                                // Effect trigger interval (in seconds)
        public float lastTriggerTime;           // 上次触发时间
                                                // Last trigger time

        public bool isActive = false;           // Buff是否激活(默认不激活)
                                                // Whether the Buff is active (default: inactive)

        public BuffData() { }

        public BuffData(BuffData otherBuffData)
        {
            buffID = otherBuffData.buffID;
            buffName = otherBuffData.buffName;
            timeLife = otherBuffData.timeLife;
            intervalTime = otherBuffData.intervalTime;
            isActive = otherBuffData.isActive;
        }
    }

    /// <summary>
    /// Buff基类
    /// Buff Base Class
    /// </summary>
    [Serializable]
    public class Buff
    {
        public BuffData buffData = new BuffData();

        // Buff效果回调函数
        // Buff effect callback functions
        protected delegate void BuffEffectDelegate(ActorStats target);     // Buff效果委托
                                                                           // Buff effect delegate
        protected BuffEffectDelegate OnActivate;                           // Buff激活时刻委托
                                                                           // Delegate for Buff activation
        protected BuffEffectDelegate OnUpdate;                             // BUff持续期间委托
                                                                           // Delegate for Buff sustain
        protected BuffEffectDelegate OnDeactivate;                         // Buff结束时刻委托
                                                                           // Delegate for Buff deactivation

        // 默认构造
        // Default constructor
        public Buff() { }

        public Buff(Buff otherBuff)
        {
            this.buffData = otherBuff.buffData;

            if (otherBuff.OnActivate != null)
            {
                foreach (var handler in otherBuff.OnActivate.GetInvocationList())
                {
                    var target = handler.Target as IBuffEffect;
                    if (target != null)
                    {
                        OnActivate += target.BuffEffectOnActivate;
                    }
                }
            }

            if (otherBuff.OnUpdate != null)
            {
                foreach (var handler in otherBuff.OnUpdate.GetInvocationList())
                {
                    var target = handler.Target as IBuffEffect;
                    if (target != null)
                    {
                        OnUpdate += target.BuffEffectOnSustained;
                    }
                }
            }

            if (otherBuff.OnDeactivate != null)
            {
                foreach (var handler in otherBuff.OnDeactivate.GetInvocationList())
                {
                    var target = handler.Target as IBuffEffect;
                    if (target != null)
                    {
                        OnDeactivate += target.BuffEffectOnDeactivate;
                    }
                }
            }
        }

        // 重置Buff持续时间
        // Reset Buff duration
        public void RemakingTime()
        {
            buffData.currentTime = Time.time;
        }

        // 处理Buff持续时间关闭Buff
        // Handle Buff expiration and deactivate it
        public void UpdateTime()
        {
            if (buffData.timeLife <= 0) return;   // 永久Buff处理
                                                  // Handle infinite Buffs

            if (Time.time - buffData.currentTime >= buffData.timeLife)     // 检测时间是否到期
                                                                           // Check if the Buff has expired
            {
                // 关闭Buff
                // Deactivate the Buff
                buffData.isActive = false;
            }
        }

        // Buff激活时的效果
        // Effect when the Buff is activated
        public void ActivateBuffEffect(ActorStats Target)
        {
            if (!buffData.isActive)                      // 避免重复激活，造成Buff效果叠加或者覆盖
                                                         // Avoid reactivation to prevent stacking or overwriting effects
            {
                buffData.isActive = true;

                RemakingTime();
                buffData.lastTriggerTime = buffData.currentTime;

                OnActivate?.Invoke(Target);     // Buff效果应用
                                                // Apply Buff effect
            }
        }

        // Buff持续时的效果
        // Effect during the Buff's duration
        public void HandleSustainedEffect(ActorStats Target)
        {
            // 间隔触发检测
            // Check for interval-based triggers
            if (buffData.intervalTime > 0 && Time.time - buffData.lastTriggerTime >= buffData.intervalTime)
            {
                OnUpdate?.Invoke(Target);
                buffData.lastTriggerTime = Time.time;
            }
        }

        // Buff关闭时的效果
        // Effect when the Buff is deactivated
        public void DeactivateBuffEffect(ActorStats Target)
        {
            if (!buffData.isActive)
            {
                OnDeactivate?.Invoke(Target);                               // Buff效果移除
                                                                            // Remove Buff effect
            }
        }
    }
}

