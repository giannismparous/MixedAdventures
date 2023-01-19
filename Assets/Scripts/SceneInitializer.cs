using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{

    private int playerID;
    private GameObject player;
    private bool reachedCheckPoint;
    [SerializeField] private GameObject marioPrefab;
    [SerializeField] private GameObject sonicPrefab;
    [SerializeField] private GameObject pacManPrefab;
    [SerializeField] private GameObject pikachuPrefab;
    [SerializeField] private GameObject mitsosPrefab;
    [SerializeField] private GameObject antigoniPrefab;
    [SerializeField] private GameObject giorgakisPrefab;
    [SerializeField] private GameObject mparioPrefab;

    void Awake()
    {
        reachedCheckPoint=false;
        playerID=CurrentGameData.currentGameData.GetPlayerID();
        if (playerID==0)player= Instantiate(marioPrefab) as GameObject;
        else if (playerID==1)player= Instantiate(sonicPrefab) as GameObject;
        else if (playerID==2)player= Instantiate(pacManPrefab) as GameObject;
        else if (playerID==3) player= Instantiate(pikachuPrefab) as GameObject;
        else if (playerID==4)player= Instantiate(mitsosPrefab) as GameObject;
        else if (playerID==5)player= Instantiate(antigoniPrefab) as GameObject;
        else if (playerID==6) player= Instantiate(giorgakisPrefab) as GameObject;
        else if (playerID==7) player= Instantiate(mparioPrefab) as GameObject;
        player.name="Player";
        player.transform.position = GameObject.Find("PlayerStartingPoint").transform.position;
        GameObject.Find("CameraPosition").transform.position=new Vector3(GameObject.Find("PlayerStartingPoint").transform.position.x,GameObject.Find("PlayerStartingPoint").transform.position.y,-40);
        GameObject.Find("CameraPosition").transform.SetParent(player.transform);
    }

    void Start(){
        GameManager.gameManager.SetCurrentScene(transform.gameObject);
        GameObject[] objs=GameObject.FindGameObjectsWithTag("MenuMusic");
        foreach (GameObject g in objs)Destroy(g);
    }

    public GameObject GetPlayer(){
        return player;
    }

    public void RestartLevel(){
        if (!reachedCheckPoint)player.transform.position = GameObject.Find("PlayerStartingPoint").transform.position;
        else player.transform.position = GameObject.Find("CheckPoint").transform.position;
        player.transform.Find("Body").GetComponent<PlayerMove>().Heal(100);
        if (GameObject.Find("BowserDoor")!=null){
            GameObject.Find("BowserDoor").transform.Find("BowserDoorBody").transform.position=GameObject.Find("BowserDoor").transform.Find("Point1").transform.position;
            GameObject.Find("BowserDoor2").transform.Find("BowserDoorBody").transform.position=GameObject.Find("BowserDoor2").transform.Find("Point1").transform.position;
            GameObject.Find("BowserDoor").transform.Find("ButtonChecker").GetComponent<ButtonCheckerBowserDoor>().Reset();
            GameObject.Find("BowserDoor2").transform.Find("ButtonChecker").GetComponent<ButtonCheckerBowserDoor>().Reset();
            GameObject.Find("BGMusic1").GetComponent<AudioSource>().Stop();
            GameObject.Find("BGMusic2").GetComponent<AudioSource>().Stop();
            GameObject.Find("BGMusic1").GetComponent<AudioSource>().Play();
            GameObject.Find("BGCanvas").transform.Find("BGBushes").gameObject.SetActive(true);
            GameObject.Find("BGCanvas").transform.Find("BGSkyWithClouds").gameObject.SetActive(true);
            GameObject.Find("BGCanvas").transform.Find("BGClouds").gameObject.SetActive(true);
        }
        
    }

    public void CheckPointReached(){
        reachedCheckPoint=true;
    }

}
