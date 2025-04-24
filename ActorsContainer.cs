using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 对象容器
/// 职责
/// 我们可以通过它管理所有的游戏对象，例如玩家、敌人和NPC等等
/// 每个对象必须且需要为其添加一个ActorStats组件
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
    /// 对象列表
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
    /// 添加对象到列表中
    /// Add an object to the list
    /// </summary>
    /// <param name="ObjtectUID">对象的唯一ID / The unique ID of the object</param>
    /// <param name="obj">游戏对象 / The game object</param>
    public void AddActor(string ObjtectUID, GameObject obj)
    {
        objectList.Add(ObjtectUID, obj);
    }

    /// <summary>
    /// 移除对象
    /// Remove an object
    /// </summary>
    /// <param name="ObjtectUID">对象的唯一ID / The unique ID of the object</param>
    public void RemoveActor(string ObjtectUID)
    {
        objectList.Remove(ObjtectUID);
    }

    /// <summary>
    /// 通过Key获取对象
    /// Get an object by its key
    /// </summary>
    /// <param name="ObjtectUID">对象的唯一ID / The unique ID of the object</param>
    /// <returns>游戏对象 / The game object</returns>
    public GameObject GetActor(string ObjtectUID)
    {
        return objectList[ObjtectUID];
    }
}

