using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;                                                                                  //Generic <T> is created here <T> can replace any type like strings,int,GameObject,etc

    public static T Instance{
        get{
            if (instance == null)
                instance = FindObjectOfType<T>();                                                                //instance is created for itself.FindObjectOfType<T> finds the type of generic.
            else if(instance != FindObjectOfType<T>())
                Destroy(FindObjectOfType<T>());                                                                   //instance is destroyed

            DontDestroyOnLoad(FindObjectOfType<T>());
            return instance;
        }
    }
}
