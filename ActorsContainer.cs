using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ��������
/// ְ��
/// ���ǿ���ͨ�����������е���Ϸ����������ҡ����˺�NPC�ȵ�
/// ÿ�������������ҪΪ�����һ��ActorStats���
/// 
/// Actors Container
/// Responsibilities:
/// This class is used to manage all game objects, such as players, enemies, and NPCs.
/// Each object must have an ActorStats component attached to it.
/// </summary>
public class ActorsContainer : MonoBehaviour
{
    static ActorsContainer instance;

    public static ActorsContainer Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ActorsContainer");
                instance = go.AddComponent<ActorsContainer>();
            }
            return instance;
        }
    }

    /// <summary>
    /// �����б�
    /// Object list
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
    /// Add an object to the list
    /// </summary>
    /// <param name="ObjtectUID">�����ΨһID / The unique ID of the object</param>
    /// <param name="obj">��Ϸ���� / The game object</param>
    public void AddActor(string ObjtectUID, GameObject obj)
    {
        objectList.Add(ObjtectUID, obj);
    }

    /// <summary>
    /// �Ƴ�����
    /// Remove an object
    /// </summary>
    /// <param name="ObjtectUID">�����ΨһID / The unique ID of the object</param>
    public void RemoveActor(string ObjtectUID)
    {
        objectList.Remove(ObjtectUID);
    }

    /// <summary>
    /// ͨ��Key��ȡ����
    /// Get an object by its key
    /// </summary>
    /// <param name="ObjtectUID">�����ΨһID / The unique ID of the object</param>
    /// <returns>��Ϸ���� / The game object</returns>
    public GameObject GetActor(string ObjtectUID)
    {
        return objectList[ObjtectUID];
    }
}

