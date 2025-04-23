using UnityEngine;

// ��������
namespace ActorStatsSystem
{
	/// <summary>
	/// �������Ի���
	/// Author: Firstname Lastname
	/// </summary>
	public class ActorStats : MonoBehaviour
	{
		public string UniqueID;                         // ΨһID
		public string ActorID;                          // ����ID
		public string ActorName;                        // ��������

		public float Hp = 100f;                         // ��ǰѪ��
		public float MaxHp = 100f;                      // ���Ѫ��
		public float OriginalMaxHp = 100f;              // ��ʼ���Ѫ��

		public float attack = 10;                       // �˺�
		public float OriginalAttack = 10;               // ��ʼ�˺�
		public float defence = 10;                      // ����

		public float moveSpeed = 10;                    // �ƶ��ٶ�

		public bool IsDead = false;                     // �Ƿ�����
		public bool IsInvulnerable = false;             // �Ƿ��޵�

		public AttributeMultipliers stateCoefficient;       // ״̬ϵ��
		public ActorsContainer SceneObjectList;         // ���������
		public ActorStatsList SceneObjectStateList;     // �����������Ա�

		BuffSystem.BuffContainer BuffList =
			new BuffSystem.BuffContainer();                  // Buff�б�

		void Awake()
		{
			BuffList.InitBuffContainer(this);                // ��ʼ��Buff��

			// �������ΨһID
			for (int i = 0; i < 8; i++)
			{
				UniqueID += Random.Range(0, 10).ToString();
			}
			SceneObjectList.AddActor(UniqueID, gameObject);        // ע����󵽳��������
		}

		void Update()
		{
			if (IsDead)
			{
				return;
			}

			// ����Buffʱ��
			BuffList.UpdateBuffTimeLife();

			if (Input.GetKeyDown(KeyCode.Space))
			{
				Test_PrintObjectStateInfo();
			}
		}

		/// <summary>
		/// Buff��ӽӿ�
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