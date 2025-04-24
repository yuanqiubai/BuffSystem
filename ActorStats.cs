using UnityEngine;

// 对象属性
// Object Attributes
namespace ActorStatsSystem
{
    /// <summary>
    /// 对象属性基类
    /// 1. 需要创建一个游戏对象，然后把ActorSstats组件拖给它。
    /// 2. 你可能看到我这里有一个UID，但请不要在意它，它是临时生成的。
    /// 3. 在发布buff时，你需要将对方的ActorID和和需要发布的buff都告诉给BuffManager。
    /// 4. 它是可拓展基类，你可以选择性的为它添加更多的属性词条，或者删除不需要的属性词条。
    /// 
    /// Actor Attributes Base Class
    /// 1. You need to create a game object and attach the ActorStats component to it.
    /// 2. You might notice a UID here, but don't worry about it; it's generated temporarily.
    /// 3. When publishing a buff, you need to provide the target's ActorID and the buff to the BuffManager.
    /// 4. This is an extensible base class. You can optionally add more attribute entries or remove unnecessary ones.
    /// Author: Firstname Lastname
    /// </summary>
    public class ActorStats : MonoBehaviour
    {
        public string UniqueID;                         // 唯一ID
                                                        // Unique ID
        public string ActorID;                          // 对象ID
                                                        // Object ID
        public string ActorName;                        // 对象名称
                                                        // Object Name

        public float Hp = 100f;                         // 当前血量
                                                        // Current Health
        public float MaxHp = 100f;                      // 最大血量
                                                        // Maximum Health
        public float OriginalMaxHp = 100f;              // 初始最大血量
                                                        // Original Maximum Health

        public float attack = 10;                       // 伤害
                                                        // Attack Damage
        public float OriginalAttack = 10;               // 初始伤害
                                                        // Original Attack Damage
        public float defence = 10;                      // 防御
                                                        // Defense

        public float moveSpeed = 10;                    // 移动速度
                                                        // Movement Speed

        public bool IsDead = false;                     // 是否死亡
                                                        // Is Dead
        public bool IsInvulnerable = false;             // 是否无敌
                                                        // Is Invulnerable

        public AttributeMultipliers stateCoefficient;   // 状态系数
                                                        // State Coefficient

        BuffSystem.BuffContainer BuffList =
            new BuffSystem.BuffContainer();             // Buff容器
                                                        // Buff Container

        void Awake()
        {
            BuffList.InitBuffContainer(this);                // 初始化Buff表
                                                             // Initialize Buff List

            // 随机生成唯一ID
            // Randomly generate a unique ID
            for (int i = 0; i < 8; i++)
            {
                UniqueID += Random.Range(0, 10).ToString();
            }
            ActorsContainer.Instance.AddActor(UniqueID, gameObject);        // 注册对象到场景对象表
                                                                            // Register the object to the scene object list
        }

        void Update()
        {
            if (IsDead)
            {
                return;
            }

            // 更新Buff时间
            // Update Buff duration
            BuffList.UpdateBuffTimeLife();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Test_PrintObjectStateInfo();
            }
        }

        /// <summary>
        /// Buff添加接口
        /// Add Buff Interface
        /// </summary>
        /// <param name="newBuff">新的Buff / The new Buff</param>
        public void AddBuff(BuffSystem.Buff newBuff)
        {
            BuffList.AddBuff(newBuff);
        }

        #region DebugTest
        /// <summary>
        /// 测试打印对象状态信息
        /// Test: Print Actor stats information
        /// </summary>
        public void Test_PrintActorStatsInfo()
        {
            Debug.Log("ObjectUID: " + UniqueID +
                " ObjectName: " + ActorName + "MoveSpeed: " + moveSpeed);
        }

        #endregion
    }
}

