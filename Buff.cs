using System;
using UnityEngine;
using ActorStatsSystem;

namespace BuffSystem
{
    /*
    这是实现buff效果的接口，派生的buff类需要继承它
    */
    public interface IBuffEffect
    {
        /// <summary>
        /// Buff效果激活时的效果
        /// 使用规则：派生Buff类时，实现Buff激活时刻的效果
        /// </summary>
        void BuffEffectOnActivate(ActorStats target);

        /// <summary>
        /// Buff持续期间的效果
        /// 使用规则：派生Buff类时，实现Buff持续期间的效果
        /// </summary>
        void BuffEffectOnSustained(ActorStats target);

        /// <summary>
        /// Buff结束时刻的效果
        /// 使用规则：派生Buff类时，实现Buff结束时刻的效果
        /// </summary>
        void BuffEffectOnDeactivate(ActorStats target);
    }

    /*
    BuffData是buff的基本数据。
    */
    public class BuffData
    {
        public Sprite buffIcon;
        public string IconPath;                 // Buff图标路径

        public string buffName;                 // Buff名称

        public float timeLife = 5;              // Buff持续时间 (-1为无限时间)
        public float currentTime = 0;           // Buff当前时间

        // 新增间隔触发相关参数
        public float intervalTime = 1f;         // 效果触发间隔（秒）
        public float lastTriggerTime;           // 上次触发时间

        public bool isActive = false;           // Buff是否激活(默认不激活)

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
    /// </summary>
    [Serializable]
    public class Buff
    {
        public BuffData buffData = new BuffData();

        // Buff效果回调函数
        protected delegate void BuffEffectDelegate(ActorStats target);     // Buff效果委托
        protected BuffEffectDelegate OnActivate;                           // Buff激活时刻委托
        protected BuffEffectDelegate OnUpdate;                             // BUff持续期间委托
        protected BuffEffectDelegate OnDeactivate;                         // Buff结束时刻委托

        //  默认构造
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
                    {;
                        OnDeactivate += target.BuffEffectOnDeactivate;
                    }
                }
            }
        }

        // 重置Buff持续时间
        public void RemakingTime()
        {
            buffData.currentTime = Time.time;
        }

        // 处理Buff持续时间关闭Buff
        public void UpdateTime()
        {   
            if (buffData.timeLife <= 0) return;   // 永久Buff处理

            if(Time.time - buffData.currentTime >= buffData.timeLife)     // 检测时间是否到期
            {
                // 关闭Buff
                buffData.isActive = false;
            }
        }

        // Buff激活时的效果
        public void ActivateBuffEffect(ActorStats Target)
        {
            if (!buffData.isActive)                      // 避免重复激活，造成Buff效果叠加或者覆盖
            {
                buffData.isActive = true;

                RemakingTime();
                buffData.lastTriggerTime = buffData.currentTime;

                OnActivate?.Invoke(Target);     // Buff效果应用
            }
        }

        // Buff持续时的效果
        public void HandleSustainedEffect(ActorStats Target)
        {
            // 间隔触发检测
            if(buffData.intervalTime > 0 && Time.time - buffData.lastTriggerTime >= buffData.intervalTime)
            {
                OnUpdate?.Invoke(Target);
                buffData.lastTriggerTime = Time.time;
            }
        }

        // Buff关闭时的效果
        public void DeactivateBuffEffect(ActorStats Target)
        {
            if (!buffData.isActive)
            {
                OnDeactivate?.Invoke(Target);                               // Buff效果移除
            }
        }
    }
}
