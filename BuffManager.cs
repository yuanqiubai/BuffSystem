using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using ActorStatsSystem;


namespace BuffSystem
{
    /// <summary>
    /// Buff������
    /// </summary>
    public class BuffManager : MonoBehaviour
    {
        private BuffManager() { }
        private static BuffManager instance;
        public static BuffManager Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject gameObject = new GameObject("BuffManager");
                    instance = gameObject.AddComponent<BuffManager>();
                }
                return instance;
            }
        }

        bool isProcessedInFrames = false;                    // �Ƿ���֡�д���
        int NumberOfFramesProcessed = 20;                    // ÿ֡�����Buff����

        Queue<Util.Pair<ActorStats, Buff>> BuffBuffer = 
            new Queue<Util.Pair<ActorStats, Buff>>();        // buff���л�����

        void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            StartCoroutine(ISubscribeBuff());                 // ����Buff����Э��
            DontDestroyOnLoad(this);                          // ��ֹ�����л�ʱ����  
        }


        /// <summary>
        /// ����Buff
        /// ְ�𣺽��������ߵ�Buff���͵�BuffBuffer������
        /// </summary>
        /// <param name="ActorState"></param>
        /// <param name="buffID", Buff��ID></param>

        public void PublishBuffBuffer(ActorStats ActorState, string buffID)
        {
            Buff buff = new Buff(StaticBuffList.Instance.GetBuffByID(buffID));
                
            BuffBuffer.Enqueue(new Util.Pair<ActorStats, Buff>(ActorState, buff));
        }

        /// <summary>
        /// ����Buff
        /// </summary>
        /// <param name="objectState"></param>
        /// <param name="buff"></param>
        public void PublishBuffBuffer(ActorStats actorState, Buff buff)
        {
            BuffBuffer.Enqueue(new Util.Pair<ActorStats, Buff>(actorState, buff));
        }

        // ���η����Buff�����ߣ�Э��ȫ�ִ���
        IEnumerator ISubscribeBuff()
        {
            while (true) // ����ѭ����֤Э�̳�������
            {
                if (BuffBuffer.Count == 0)
                {
                    yield return null; // ����Ϊ��ʱ�ȴ���һ֡
                    continue;
                }

                int processedCount = 0;
                while (BuffBuffer.Count > 0)
                {
                    // ��֡������
                    if (isProcessedInFrames && processedCount >= NumberOfFramesProcessed)
                    {
                        yield return null; // �ȴ���һ֡��������
                        processedCount = 0;
                        continue;
                    }

                    Util.Pair<ActorStats, Buff> pair = BuffBuffer.Dequeue();
                    pair.First.AddBuff(pair.Second);               // ���Buff������Buff�б�

                    processedCount++;
                }

                yield return null; // ȷ��ÿ֡������һ�� yield
            }
        }


        // ���÷�֡����
        public void SetProcessedBuffInFrame(bool isProcessedInFrames, int NumberOfFramesProcessed = 20)
        {
            this.isProcessedInFrames = isProcessedInFrames;
            this.NumberOfFramesProcessed = NumberOfFramesProcessed;
        }

    }
}