using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonSample : MonoBehaviour
{
    static public SingletonSample instance;
    void Awake()
    {
        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
