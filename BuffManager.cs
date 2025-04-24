using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using ActorStatsSystem;

namespace BuffSystem
{
    /// <summary>
    /// Buff管理器
    /// 1. Buff管理器采用了单例模式，用来全局处理游戏对象之间的Buff传递.
    /// 2. 发布者需要调用 PublishBuffBuffer 方法，发布上来的Buff会放置在消息队列中，最后由 ISubscribeBuff 方法进行发布.
    /// 3. ISubscribeBuff 会根据队列中的 Buff 和 Buff 的接收者，发布到接受者的的"信箱"中(即: BuffContainer)
    /// 
    /// Buff Manager
    /// 1. The Buff Manager uses the Singleton pattern to globally handle the transmission of Buffs between game objects.
    /// 2. Publishers need to call the PublishBuffBuffer method. Buffs published will be placed in a message queue and finally distributed by the ISubscribeBuff method.
    /// 3. ISubscribeBuff will distribute Buffs from the queue to the "mailbox" (i.e., BuffContainer) of the respective receiver.
    /// </summary>
    public class BuffManager : MonoBehaviour
    {
        private BuffManager() { }
        private static BuffManager instance;
        public static BuffManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject gameObject = new GameObject("BuffManager");
                    instance = gameObject.AddComponent<BuffManager>();
                }
                return instance;
            }
        }

        bool isProcessedInFrames = false;                    // 是否在帧中处理
                                                             // Whether Buffs are processed in frames
        int NumberOfFramesProcessed = 20;                    // 每帧处理的Buff数量
                                                             // Number of Buffs processed per frame

        Queue<Util.Pair<ActorStats, Buff>> BuffBuffer =
            new Queue<Util.Pair<ActorStats, Buff>>();        // buff队列缓冲区
                                                             // Buff queue buffer

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            StartCoroutine(ISubscribeBuff());                 // 启动Buff订阅协程
                                                              // Start the Buff subscription coroutine
            DontDestroyOnLoad(this);                          // 防止场景切换时销毁  
                                                              // Prevent destruction during scene transitions
        }

        /// <summary>
        /// 发布Buff
        /// Publish Buff
        /// </summary>
        /// <param name="actorState">目标对象状态 / The target actor's state</param>
        /// <param name="buff">需要发布的Buff / The Buff to be published</param>
        public void PublishBuffBuffer(ActorStats actorState, Buff buff)
        {
            BuffBuffer.Enqueue(new Util.Pair<ActorStats, Buff>(actorState, buff));
        }

        // 依次发配给Buff订阅者（协程全局处理）
        // Distribute Buffs to subscribers one by one (global processing via coroutine)
        IEnumerator ISubscribeBuff()
        {
            while (true) // 无限循环保证协程持续运行
                         // Infinite loop to ensure the coroutine keeps running
            {
                if (BuffBuffer.Count == 0)
                {
                    yield return null; // 队列为空时等待下一帧
                                       // Wait for the next frame if the queue is empty
                    continue;
                }

                int processedCount = 0;
                while (BuffBuffer.Count > 0)
                {
                    // 分帧处理检查
                    // Check for frame-based processing
                    if (isProcessedInFrames && processedCount >= NumberOfFramesProcessed)
                    {
                        yield return null; // 等待下一帧继续处理
                                           // Wait for the next frame to continue processing
                        processedCount = 0;
                        continue;
                    }

                    Util.Pair<ActorStats, Buff> pair = BuffBuffer.Dequeue();
                    pair.First.AddBuff(pair.Second);               // 添加Buff到对象Buff列表
                                                                   // Add Buff to the target's Buff list

                    processedCount++;
                }

                yield return null; // 确保每帧至少有一次 yield
                                   // Ensure at least one yield per frame
            }
        }

        /// <summary>
        /// 设置分帧处理
        /// Set frame-based Buff processing
        /// </summary>
        /// <param name="isProcessedInFrames">是否分帧处理 / Whether to process Buffs in frames</param>
        /// <param name="NumberOfFramesProcessed">每帧处理的Buff数量 / Number of Buffs processed per frame</param>
        public void SetProcessedBuffInFrame(bool isProcessedInFrames, int NumberOfFramesProcessed = 20)
        {
            this.isProcessedInFrames = isProcessedInFrames;
            this.NumberOfFramesProcessed = NumberOfFramesProcessed;
        }
    }
}

