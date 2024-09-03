using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (!instance)
            {
                if (Debug.isDebugBuild)
                    Debug.LogError(typeof(T) + "has no instance");
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (!instance)
        {
            if (Debug.isDebugBuild)
                Debug.LogError(typeof(T) + "has no instance");
        }
        instance = this as T;
    }
}
