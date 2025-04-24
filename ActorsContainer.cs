using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// 对象容器
/// 职责：
/// 我们可以通过对象容器来访问游戏对象。例如:玩家对象，敌人对象，NPC对象等
/// 但是有一个要求，你需要为你的对象添加一个ActorStats组件
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
    /// 对象表
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
    /// 添加对象到列表中
    /// </summary>
    /// <param name="ObjtectUID"></param>
    /// <param name="obj"></param>
    public void AddActor(string ObjtectUID, GameObject obj)
    {
        objectList.Add(ObjtectUID, obj);
    }

    // 移除对象
    public void RemoveActor(string ObjtectUID)
    {
        objectList.Remove(ObjtectUID);
    }

    // 通过Key获取对象
    public GameObject GetActor(string ObjtectUID)
    {
        return objectList[ObjtectUID];
    }
}
