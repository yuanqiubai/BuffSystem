using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// ��������
/// ְ��
/// ���ǿ���ͨ������������������Ϸ��������:��Ҷ��󣬵��˶���NPC�����
/// ������һ��Ҫ������ҪΪ��Ķ������һ��ActorStats���
/// </summary>
public class ActorsContainer : MonoBehaviour
{
    static ActorsContainer instance;

    public static ActorsContainer Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject("ActorsContainer");
                instance = go.AddComponent<ActorsContainer>();
            }
            return instance;
        }
    }

    /// <summary>
    /// �����
    /// </summary>
    Dictionary<string, GameObject> objectList = new Dictionary<string, GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ��Ӷ����б���
    /// </summary>
    /// <param name="ObjtectUID"></param>
    /// <param name="obj"></param>
    public void AddActor(string ObjtectUID, GameObject obj)
    {
        objectList.Add(ObjtectUID, obj);
    }

    // �Ƴ�����
    public void RemoveActor(string ObjtectUID)
    {
        objectList.Remove(ObjtectUID);
    }

    // ͨ��Key��ȡ����
    public GameObject GetActor(string ObjtectUID)
    {
        return objectList[ObjtectUID];
    }
}
