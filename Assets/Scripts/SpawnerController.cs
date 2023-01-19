using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{

    private float spawnTimer;
    [SerializeField] private float spawnTimerValue;
    [SerializeField] private float initialDelay;
    [SerializeField] private GameObject mobPrefab;

    void Start(){
        spawnTimer=spawnTimerValue+initialDelay;
    }

    void FixedUpdate(){
        
        if (spawnTimer<=0){
            SpawnMob();
            spawnTimer=spawnTimerValue;
        }
        else spawnTimer-=Time.deltaTime;
    }

    void SpawnMob(){
        GameObject temp;
        temp = Instantiate(mobPrefab) as GameObject;
        temp.transform.position=transform.position;
    }

}
