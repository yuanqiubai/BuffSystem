using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using ActorStatsSystem;


namespace BuffSystem
{
    /// <summary>
    /// Buff管理器
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

        bool isProcessedInFrames = false;                    // 是否在帧中处理
        int NumberOfFramesProcessed = 20;                    // 每帧处理的Buff数量

        Queue<Util.Pair<ActorStats, Buff>> BuffBuffer = 
            new Queue<Util.Pair<ActorStats, Buff>>();        // buff队列缓冲区

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

            StartCoroutine(ISubscribeBuff());                 // 启动Buff订阅协程
            DontDestroyOnLoad(this);                          // 防止场景切换时销毁  
        }


        /// <summary>
        /// 发布Buff
        /// 职责：将将发布者的Buff传送到BuffBuffer队列中
        /// </summary>
        /// <param name="ActorState"></param>
        /// <param name="buffID", Buff的ID></param>

        public void PublishBuffBuffer(ActorStats ActorState, string buffID)
        {
            Buff buff = new Buff(StaticBuffList.Instance.GetBuffByID(buffID));
                
            BuffBuffer.Enqueue(new Util.Pair<ActorStats, Buff>(ActorState, buff));
        }

        /// <summary>
        /// 发布Buff
        /// </summary>
        /// <param name="objectState"></param>
        /// <param name="buff"></param>
        public void PublishBuffBuffer(ActorStats actorState, Buff buff)
        {
            BuffBuffer.Enqueue(new Util.Pair<ActorStats, Buff>(actorState, buff));
        }

        // 依次发配给Buff订阅者（协程全局处理）
        IEnumerator ISubscribeBuff()
        {
            while (true) // 无限循环保证协程持续运行
            {
                if (BuffBuffer.Count == 0)
                {
                    yield return null; // 队列为空时等待下一帧
                    continue;
                }

                int processedCount = 0;
                while (BuffBuffer.Count > 0)
                {
                    // 分帧处理检查
                    if (isProcessedInFrames && processedCount >= NumberOfFramesProcessed)
                    {
                        yield return null; // 等待下一帧继续处理
                        processedCount = 0;
                        continue;
                    }

                    Util.Pair<ActorStats, Buff> pair = BuffBuffer.Dequeue();
                    pair.First.AddBuff(pair.Second);               // 添加Buff到对象Buff列表

                    processedCount++;
                }

                yield return null; // 确保每帧至少有一次 yield
            }
        }


        // 设置分帧处理
        public void SetProcessedBuffInFrame(bool isProcessedInFrames, int NumberOfFramesProcessed = 20)
        {
            this.isProcessedInFrames = isProcessedInFrames;
            this.NumberOfFramesProcessed = NumberOfFramesProcessed;
        }

    }
}