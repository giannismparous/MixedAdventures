using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StopGameMenuController : MonoBehaviour
{
    GameObject selector;
    private float pressKeyTimer;
    private bool active;
    private Camera cam;
    private PlayerMove playerMove;

    void Start()
    {
        selector=transform.Find("Selector").gameObject;
        selector.SetActive(false);
        pressKeyTimer=0;
        active=false;
        cam=GameObject.Find("Camera").GetComponent<Camera>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && pressKeyTimer<=0){
            active=!active;
            selector.SetActive(active);
            pressKeyTimer=0.3f;
        }

        if (Input.GetKey(KeyCode.Return) && active && pressKeyTimer<=0){
            if (selector.GetComponent<SelectorController>().GetIndex()==0){
                active=!active;
                selector.SetActive(active);
            }
            else if (selector.GetComponent<SelectorController>().GetIndex()==1){
                SceneManager.LoadScene(1);
            }
            pressKeyTimer=0.3f;
        }

        if (pressKeyTimer>0)pressKeyTimer-=Time.deltaTime;

        transform.position=new Vector3(cam.transform.position.x,cam.transform.position.y,transform.position.y);

        if (active)playerMove.EscapePressed();
        else playerMove.EscapeNotPressed();
    }
}
