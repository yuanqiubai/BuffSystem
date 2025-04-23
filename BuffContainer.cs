using UnityEngine;
using System.Collections.Generic;

namespace BuffSystem
{
    /// <summary>
    /// Buff列表
    /// 放置在对象上，用于管理对象自身的Buff
    /// </summary>
    public class BuffContainer
    {
        Dictionary<string, Buff> buffDict = new Dictionary<string, Buff>();

        ActorStatsSystem.ActorStats actorStats;

        public void InitBuffContainer(ActorStatsSystem.ActorStats Target)
        {
            actorStats = Target;
        }

        // 添加Buff
        public void AddBuff(Buff newBuff)
        {
            if(buffDict.TryGetValue(newBuff.buffData.buffID, out Buff existingBuff))
            {
                // 已存在相同Buff的处理：刷新时间
                existingBuff.RemakingTime();

                existingBuff.ActivateBuffEffect(actorStats);   // 触发Buff激活时刻效果
            }
            else
            {
                buffDict.Add(newBuff.buffData.buffID, newBuff);
                newBuff.ActivateBuffEffect(actorStats);        // 触发Buff激活时刻效果
            }
        }

        /// <summary>
        /// 移除Buff
        /// 说明：根据Buff持续时间关闭Buff,移除Buff
        /// </summary>
        public void UpdateBuffTimeLife()
        {
            // 先收集需要移除的buffID
            List<string> toRemove = new List<string>();

            foreach (Buff buff in buffDict.Values)
            {
                if (!buff.buffData.isActive)
                {
                    buff.DeactivateBuffEffect(actorStats);     // 触发Buff结束时刻效果
                    Debug.Log("Buff结束，移除Buff");
                    toRemove.Add(buff.buffData.buffID);
                    continue;
                }

                buff.HandleSustainedEffect(actorStats);        // 触发Buff持续期间效果
                buff.UpdateTime();
            }

            // 遍历结束后再移除
            foreach (string id in toRemove)
            {
                buffDict.Remove(id);
            }
        }

        /// <summary>
        /// 移除Buff
        /// 说明：根据BuffID，移除Buff
        /// </summary>
        /// <param name="buffID"></param>
        public void RemoveBuffByID(string buffID)
        {
            buffDict.Remove(buffID);
        }

        /// <summary>
        /// 移除所有Buff
        /// 说明：清空对象Buff表
        /// </summary>
        public void RemoveAllBuff()
        {
            buffDict.Clear();
        }

        /// <summary>
        /// 获取Buff激活状态
        /// 说明：根据BuffID，获取对象Buff激活状态
        /// </summary>
        /// <param name="buffID"></param>
        /// <returns></returns>
        public bool GetBuffActive(string buffID)
        {
            return buffDict[buffID].buffData.isActive;
        }

        // 检测buff是否存在
        public bool CheckBuffExist(string buffID)
        {
            return buffDict.ContainsKey(buffID);
        }

        /// <summary>
        /// 获取Buff
        /// 说明：根据BuffID，从对象Buff表中获取Buff
        /// 一般用于反馈Buff信息
        /// </summary>
        /// <param name="buffID"></param>
        /// <returns></returns>
        public Buff GetBuff(string buffID)
        {
            return buffDict[buffID];
        }


        #region TestDebug
        public void Test_PrintBuff()
        {
            if(buffDict.Count == 0)
            {
                Debug.Log(actorStats.name + "的Buff表为空!");
                return;
            }

            foreach (var item in buffDict)
            {
                Debug.Log("BuffID: " + item.Value.buffData.buffID + 
                    " BuffName: " + item.Value.buffData.buffName + 
                    "持续时间: " + item.Value.buffData.timeLife + 
                    "已持续时间: " + (Time.time - item.Value.buffData.currentTime));
            }
        }
        #endregion
    }
}
