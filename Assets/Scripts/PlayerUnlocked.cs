using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnlocked : MonoBehaviour
{
    private Transform player;
    private bool activated;
    private int textShowCounter;
    private float textShowTimer;
    private Text newPlayerText;
    [SerializeField] private float moveSpeed;
    [SerializeField] int textShowCounterValue;
    [SerializeField] float textShowTimerValue;
    
    void Start()
    {   
        textShowCounter=0;
        player=transform.Find("UnlockedPlayer");
        newPlayerText=transform.Find("NewPlayer").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated){
            Debug.Log("NAI");
            player.position=Vector2.MoveTowards(player.position, new Vector2(1000,player.position.y), moveSpeed * Time.deltaTime);
            if (textShowCounter<textShowCounterValue){
                if (textShowTimer<=0){
                    textShowCounter++;
                    textShowTimer=textShowTimerValue;
                    if (newPlayerText.color.a==1)newPlayerText.color=new Color(newPlayerText.color.r,newPlayerText.color.g,newPlayerText.color.b,0);
                    else if (newPlayerText.color.a==0)newPlayerText.color=new Color(newPlayerText.color.r,newPlayerText.color.g,newPlayerText.color.b,1);
                }
                else textShowTimer-=Time.deltaTime;
            }
        }
    }

    public void ActivateAnimation(){
        activated=true;
        textShowTimer=textShowTimerValue;
        textShowCounter=0;
        Debug.Log("AAA");
    }

}
