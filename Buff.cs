using System;
using UnityEngine;
using ActorStatsSystem;

namespace BuffSystem
{

    public interface IBuffEffect
    {
        /// <summary>
        /// BuffЧ������ʱ��Ч��
        /// ʹ�ù�������Buff��ʱ��ʵ��Buff����ʱ�̵�Ч��
        /// </summary>
        void BuffEffectOnActivate(ActorStats target);

        /// <summary>
        /// Buff�����ڼ��Ч��
        /// ʹ�ù�������Buff��ʱ��ʵ��Buff�����ڼ��Ч��
        /// </summary>
        void BuffEffectOnSustained(ActorStats target);

        /// <summary>
        /// Buff����ʱ�̵�Ч��
        /// ʹ�ù�������Buff��ʱ��ʵ��Buff����ʱ�̵�Ч��
        /// </summary>
        void BuffEffectOnDeactivate(ActorStats target);
    }

    public class BuffData
    {
        public Sprite buffIcon;
        public string IconPath;                 // Buffͼ��·��

        public string buffID;                   // BuffID
        public string buffName;                 // Buff����

        public float timeLife = 5;              // Buff����ʱ�� (-1Ϊ����ʱ��)
        public float currentTime = 0;           // Buff��ǰʱ��

        // �������������ز���
        public float intervalTime = 1f;         // Ч������������룩
        public float lastTriggerTime;           // �ϴδ���ʱ��

        public bool isActive = false;           // Buff�Ƿ񼤻�(Ĭ�ϲ�����)

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
    /// Buff����
    /// </summary>
    [Serializable]
    public class Buff
    {
        public BuffData buffData = new BuffData();

        // BuffЧ���ص�����
        protected delegate void BuffEffectDelegate(ActorStats target);    // BuffЧ��ί��
        protected BuffEffectDelegate OnActivate;                           // Buff����ʱ��ί��
        protected BuffEffectDelegate OnUpdate;                             // BUff�����ڼ�ί��
        protected BuffEffectDelegate OnDeactivate;                         // Buff����ʱ��ί��

        //  Ĭ�Ϲ���
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

        // ����Buff����ʱ��
        public void RemakingTime()
        {
            buffData.currentTime = Time.time;
        }

        // ����Buff����ʱ��ر�Buff
        public void UpdateTime()
        {   
            if (buffData.timeLife <= 0) return;                  // ����Buff

            if(Time.time - buffData.currentTime >= buffData.timeLife)     // ���ʱ���Ƿ���
            {
                // �ر�Buff
                buffData.isActive = false;
            }
        }

        // Buff����ʱ��Ч��
        public void ActivateBuffEffect(ActorStats Target)
        {
            if (!buffData.isActive)                      // �����ظ�������BuffЧ�����ӻ��߸���
            {
                buffData.isActive = true;

                RemakingTime();
                buffData.lastTriggerTime = buffData.currentTime;

                OnActivate?.Invoke(Target);     // BuffЧ��Ӧ��
            }
        }

        // Buff����ʱ��Ч��
        public void HandleSustainedEffect(ActorStats Target)
        {
            // ����������
            if(buffData.intervalTime > 0 && Time.time - buffData.lastTriggerTime >= buffData.intervalTime)
            {
                OnUpdate?.Invoke(Target);
                buffData.lastTriggerTime = Time.time;
            }
        }

        // Buff�ر�ʱ��Ч��
        public void DeactivateBuffEffect(ActorStats Target)
        {
            if (!buffData.isActive)
            {
                OnDeactivate?.Invoke(Target);                               // BuffЧ���Ƴ�
            }
        }
    }
}
