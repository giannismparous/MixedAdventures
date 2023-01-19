using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCrackController : MonoBehaviour
{
    private int num;
    private float selfdestructTimer;
    private float destroyCrackTimerValue;
    private float createNextCrackTimer;
    private float createNextCrackTimerValue;
    private bool createdNextCrack;
    private GameObject nextGroundCrackGameObject;
    private Transform nextGroundCrackTransform;
    private GameObject groundCrackPrefab;

    void Start() {
        createdNextCrack = false;
        nextGroundCrackTransform = transform.Find("NextGroundCrackPosition").transform;
        groundCrackPrefab = (GameObject)Resources.Load("GroundCrack");
    }

    void FixedUpdate()
    {
        Debug.Log(num);
        if (selfdestructTimer <= 0) Destroy(transform.gameObject);
        else selfdestructTimer -= Time.deltaTime;

        if (!createdNextCrack && createNextCrackTimer <= 0 && num>=0) {
           
            createdNextCrack = true;
            nextGroundCrackGameObject = Instantiate(groundCrackPrefab) as GameObject;
            nextGroundCrackGameObject.GetComponent<GroundCrackController>().SetCrack(num-1, createNextCrackTimerValue,destroyCrackTimerValue);
            nextGroundCrackGameObject.transform.position = new Vector2(nextGroundCrackTransform.position.x,nextGroundCrackTransform.position.y);
        }
        else createNextCrackTimer -= Time.deltaTime;
    }

    public void SetCrack(int num, float createNextCrackTimerValue, float destroyCrackTimerValue) {
        this.num = num;
        createNextCrackTimer = createNextCrackTimerValue;
        this.createNextCrackTimerValue = createNextCrackTimerValue;
        selfdestructTimer = destroyCrackTimerValue;
        this.destroyCrackTimerValue = destroyCrackTimerValue;
    }
}
