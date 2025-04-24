using UnityEngine;

// 对象属性
namespace ActorStatsSystem
{
	/// <summary>
	/// 对象属性基类
	/// 1. 需要创建一个游戏对象，然后把ActorSstats组件拖给它。
	/// 2. 你可能看到我这里有一个UID，但请不要在意它，它是临时生成的。
	/// 3. 在发布buff时，你需要将对方的ActorID和和需要发布的buff都告诉给BuffManager。
	/// 4. 它是可拓展基类，你可以选择性的为它添加更多的属性词条，或者删除不需要的属性词条。
	/// Author: Firstname Lastname
	/// </summary>
	public class ActorStats : MonoBehaviour
	{
		public string UniqueID;                         // 唯一ID
		public string ActorID;                          // 对象ID
		public string ActorName;                        // 对象名称

		public float Hp = 100f;                         // 当前血量
		public float MaxHp = 100f;                      // 最大血量
		public float OriginalMaxHp = 100f;              // 初始最大血量

		public float attack = 10;                       // 伤害
		public float OriginalAttack = 10;               // 初始伤害
		public float defence = 10;                      // 防御

		public float moveSpeed = 10;                    // 移动速度

		public bool IsDead = false;                     // 是否死亡
		public bool IsInvulnerable = false;             // 是否无敌

		public AttributeMultipliers stateCoefficient;   // 状态系数
		public ActorsContainer SceneObjectList;         // 场景对象表
		public ActorStatsList SceneObjectStateList;     // 场景对象属性表

		BuffSystem.BuffContainer BuffList =
			new BuffSystem.BuffContainer();         // Buff容器

		void Awake()
		{
			BuffList.InitBuffContainer(this);                // 初始化Buff表

			// 随机生成唯一ID
			for (int i = 0; i < 8; i++)
			{
				UniqueID += Random.Range(0, 10).ToString();
			}
			SceneObjectList.AddActor(UniqueID, gameObject);        // 注册对象到场景对象表
		}

		void Update()
		{
			if (IsDead)
			{
				return;
			}

			// 更新Buff时间
			BuffList.UpdateBuffTimeLife();

			if (Input.GetKeyDown(KeyCode.Space))
			{
				Test_PrintObjectStateInfo();
			}
		}

		/// <summary>
		/// Buff添加接口
		/// </summary>
		/// <param name="newBuff"></param>
		public void AddBuff(BuffSystem.Buff newBuff)
		{
			BuffList.AddBuff(newBuff);
		}



		#region DebugTest

		public void Test_PrintObjectStateInfo()
		{
			Debug.Log("ObjectUID: " + UniqueID +
				" ObjectName: " + ActorName + "MoveSpeed: " + moveSpeed);
		}

		#endregion
	}
}
