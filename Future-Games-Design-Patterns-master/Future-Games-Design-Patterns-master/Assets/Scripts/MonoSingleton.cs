using System;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{

    private static T m_Instance;

    public static T MInstance
    {
        get
        {
            if (m_Instance == null)
            {
                T[] instances = FindObjectsOfType<T>();

                if (instances.Length > 1)
                {
                    throw new InvalidOperationException($"There is more than one {typeof(T).Name} instance in the scene");
                }

                if (instances.Length > 0)
                {
                    m_Instance = instances[0];
                    DontDestroyOnLoad(m_Instance.gameObject);
                }

                if (m_Instance == null)
                {
                    //Bad:
                    // GameObject go = new GameObject(typeof(EnemyManager).Name);
                    // m_Instance = go.AddComponent<EnemyManager>();
                    
                    //Good:
                    GameObject singletonPrefab = Resources.Load<GameObject>(typeof(T).Name);

                    if (singletonPrefab == null)
                    {
                        throw new NullReferenceException($"There is no {typeof(T).Name} prefab in the resources folder");
                    }
                    
                    GameObject goInstance = Instantiate(singletonPrefab);
                    m_Instance = goInstance.GetComponent<T>();

                    if (m_Instance == null)
                    {
                        throw new NullReferenceException($"There is no {typeof(T).Name} component attached to the singleton prefab");
                    }
                }
            }

            return m_Instance;
        }
        set => m_Instance = value;
    }

    private void Awake()
    {
        if (m_Instance = null)
        {
            m_Instance = (T)this;
            DontDestroyOnLoad(gameObject);
        }
        else if (m_Instance != this)
        {
            throw new InvalidOperationException($"There is more than one {typeof(T).Name} instance in the scene");
        }
    }
}
