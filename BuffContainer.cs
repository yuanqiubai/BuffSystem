using UnityEngine;
using System.Collections.Generic;

namespace BuffSystem
{
    /// <summary>
    /// Buff容器
    /// 组合在ActorStats中，管理对象自身的Buff生命周期
    /// 
    /// Buff Container
    /// This class is used in ActorStats to manage the lifecycle of Buffs for an object.
    /// </summary>
    public class BuffContainer
    {
        Dictionary<string, Buff> buffDict = new Dictionary<string, Buff>(); // Buff字典
                                                                            // Dictionary to store Buffs

        ActorStatsSystem.ActorStats actorStats;                             // 关联的ActorStats
                                                                            // Associated ActorStats

        /// <summary>
        /// 初始化Buff容器
        /// Initialize the Buff container
        /// </summary>
        /// <param name="Target">目标ActorStats / Target ActorStats</param>
        public void InitBuffContainer(ActorStatsSystem.ActorStats Target)
        {
            actorStats = Target;
        }

        /// <summary>
        /// 添加Buff
        /// Add a Buff
        /// </summary>
        /// <param name="newBuff">新的Buff / The new Buff</param>
        public void AddBuff(Buff newBuff)
        {
            if (buffDict.TryGetValue(newBuff.buffData.buffID, out Buff existingBuff))
            {
                // 已存在相同Buff的处理：刷新时间
                // If the same Buff already exists: refresh its duration
                existingBuff.RemakingTime();

                existingBuff.ActivateBuffEffect(actorStats);   // 触发Buff激活时刻效果
                                                               // Trigger the Buff activation effect
            }
            else
            {
                buffDict.Add(newBuff.buffData.buffID, newBuff);
                newBuff.ActivateBuffEffect(actorStats);        // 触发Buff激活时刻效果
                                                               // Trigger the Buff activation effect
            }
        }

        /// <summary>
        /// 移除Buff
        /// 说明：根据Buff持续时间关闭Buff,移除Buff
        /// 
        /// Remove Buffs
        /// Description: Deactivate and remove Buffs based on their duration.
        /// </summary>
        public void UpdateBuffTimeLife()
        {
            // 先收集需要移除的buffID
            // Collect Buff IDs to be removed
            List<string> toRemove = new List<string>();

            foreach (Buff buff in buffDict.Values)
            {
                if (!buff.buffData.isActive)
                {
                    buff.DeactivateBuffEffect(actorStats);     // 触发Buff结束时刻效果
                                                               // Trigger the Buff deactivation effect
                    Debug.Log("Buff结束，移除Buff");            // Buff ended, removing Buff
                    toRemove.Add(buff.buffData.buffID);
                    continue;
                }

                buff.HandleSustainedEffect(actorStats);        // 触发Buff持续期间效果
                                                               // Trigger the Buff sustained effect
                buff.UpdateTime();                             // 更新Buff时间
                                                               // Update Buff time
            }

            // 遍历结束后再移除
            // Remove Buffs after iteration
            foreach (string id in toRemove)
            {
                buffDict.Remove(id);
            }
        }

        /// <summary>
        /// 移除Buff
        /// 说明：根据BuffID，移除Buff
        /// 
        /// Remove a Buff
        /// Description: Remove a Buff based on its ID.
        /// </summary>
        /// <param name="buffID">Buff的ID / The ID of the Buff</param>
        public void RemoveBuffByID(string buffID)
        {
            buffDict.Remove(buffID);
        }

        /// <summary>
        /// 移除所有Buff
        /// 说明：清空对象Buff表
        /// 
        /// Remove all Buffs
        /// Description: Clear the Buff table for the object.
        /// </summary>
        public void RemoveAllBuff()
        {
            buffDict.Clear();
        }

        /// <summary>
        /// 获取Buff激活状态
        /// 说明：根据BuffID，获取对象Buff激活状态
        /// 
        /// Get Buff activation status
        /// Description: Retrieve the activation status of a Buff based on its ID.
        /// </summary>
        /// <param name="buffID">Buff的ID / The ID of the Buff</param>
        /// <returns>是否激活 / Whether the Buff is active</returns>
        public bool GetBuffActive(string buffID)
        {
            return buffDict[buffID].buffData.isActive;
        }

        /// <summary>
        /// 检测Buff是否存在
        /// Check if a Buff exists
        /// </summary>
        /// <param name="buffID">Buff的ID / The ID of the Buff</param>
        /// <returns>是否存在 / Whether the Buff exists</returns>
        public bool CheckBuffExist(string buffID)
        {
            return buffDict.ContainsKey(buffID);
        }

        /// <summary>
        /// 获取Buff
        /// 说明：根据BuffID，从对象Buff表中获取Buff
        /// 一般用于反馈Buff信息
        /// 
        /// Get a Buff
        /// Description: Retrieve a Buff from the object's Buff table based on its ID.
        /// Typically used to provide Buff information.
        /// </summary>
        /// <param name="buffID">Buff的ID / The ID of the Buff</param>
        /// <returns>目标Buff / The target Buff</returns>
        public Buff GetBuff(string buffID)
        {
            return buffDict[buffID];
        }
    }
}


