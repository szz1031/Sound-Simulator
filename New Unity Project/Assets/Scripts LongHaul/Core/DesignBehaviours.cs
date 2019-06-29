using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SimpleBehaviour
{
    protected Transform transform;
    public Transform Transform
    {
        get
        {
            return transform;
        }
    }
    public SimpleBehaviour(Transform _transform)
    {
        transform = _transform;
    }
}

public class SimpleMonoLifetime
{
    public virtual void Awake() { }
    public virtual void Update() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnDestroy() { }
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
}

public class SimpleSingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; protected set; }
    protected virtual void Awake()
    {
        Instance = this.GetComponent<T>();
    }
}

public class SingletonMono<T>  : MonoBehaviour where T : MonoBehaviour
{     
    protected virtual void Awake()
    {
        instance = this.GetComponent<T>();
        this.name = typeof(T).Name;
        DontDestroyOnLoad(this);
    }
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj=new GameObject();
                obj.name = typeof(T).Name;
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
    public static bool HaveInstance
    {
        get
        {
            return instance != null;
        }
    }
}

public class SingleTon<T> where T:new()
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}