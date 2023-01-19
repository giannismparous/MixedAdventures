using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs=GameObject.FindGameObjectsWithTag("MenuMusic");
        if (objs.Length>1)Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }
}