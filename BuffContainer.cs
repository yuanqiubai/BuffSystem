using UnityEngine;
using System.Collections.Generic;

namespace BuffSystem
{
    /// <summary>
    /// Buff�б�
    /// �����ڶ����ϣ����ڹ�����������Buff
    /// </summary>
    public class BuffContainer
    {
        Dictionary<string, Buff> buffDict = new Dictionary<string, Buff>();

        ActorStatsSystem.ActorStats actorStats;

        public void InitBuffContainer(ActorStatsSystem.ActorStats Target)
        {
            actorStats = Target;
        }

        // ���Buff
        public void AddBuff(Buff newBuff)
        {
            if(buffDict.TryGetValue(newBuff.buffData.buffID, out Buff existingBuff))
            {
                // �Ѵ�����ͬBuff�Ĵ���ˢ��ʱ��
                existingBuff.RemakingTime();

                existingBuff.ActivateBuffEffect(actorStats);   // ����Buff����ʱ��Ч��
            }
            else
            {
                buffDict.Add(newBuff.buffData.buffID, newBuff);
                newBuff.ActivateBuffEffect(actorStats);        // ����Buff����ʱ��Ч��
            }
        }

        /// <summary>
        /// �Ƴ�Buff
        /// ˵��������Buff����ʱ��ر�Buff,�Ƴ�Buff
        /// </summary>
        public void UpdateBuffTimeLife()
        {
            // ���ռ���Ҫ�Ƴ���buffID
            List<string> toRemove = new List<string>();

            foreach (Buff buff in buffDict.Values)
            {
                if (!buff.buffData.isActive)
                {
                    buff.DeactivateBuffEffect(actorStats);     // ����Buff����ʱ��Ч��
                    Debug.Log("Buff�������Ƴ�Buff");
                    toRemove.Add(buff.buffData.buffID);
                    continue;
                }

                buff.HandleSustainedEffect(actorStats);        // ����Buff�����ڼ�Ч��
                buff.UpdateTime();
            }

            // �������������Ƴ�
            foreach (string id in toRemove)
            {
                buffDict.Remove(id);
            }
        }

        /// <summary>
        /// �Ƴ�Buff
        /// ˵��������BuffID���Ƴ�Buff
        /// </summary>
        /// <param name="buffID"></param>
        public void RemoveBuffByID(string buffID)
        {
            buffDict.Remove(buffID);
        }

        /// <summary>
        /// �Ƴ�����Buff
        /// ˵������ն���Buff��
        /// </summary>
        public void RemoveAllBuff()
        {
            buffDict.Clear();
        }

        /// <summary>
        /// ��ȡBuff����״̬
        /// ˵��������BuffID����ȡ����Buff����״̬
        /// </summary>
        /// <param name="buffID"></param>
        /// <returns></returns>
        public bool GetBuffActive(string buffID)
        {
            return buffDict[buffID].buffData.isActive;
        }

        // ���buff�Ƿ����
        public bool CheckBuffExist(string buffID)
        {
            return buffDict.ContainsKey(buffID);
        }

        /// <summary>
        /// ��ȡBuff
        /// ˵��������BuffID���Ӷ���Buff���л�ȡBuff
        /// һ�����ڷ���Buff��Ϣ
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
                Debug.Log(actorStats.name + "��Buff��Ϊ��!");
                return;
            }

            foreach (var item in buffDict)
            {
                Debug.Log("BuffID: " + item.Value.buffData.buffID + 
                    " BuffName: " + item.Value.buffData.buffName + 
                    "����ʱ��: " + item.Value.buffData.timeLife + 
                    "�ѳ���ʱ��: " + (Time.time - item.Value.buffData.currentTime));
            }
        }
        #endregion
    }
}
