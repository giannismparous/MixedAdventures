using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
public class Scroller : MonoBehaviour {

    [SerializeField] private RawImage img;
    [SerializeField] private float x, y;
    [SerializeField] private bool clouds;
    [SerializeField] private bool bgNonClouds;
    private float tempX;
    private float dirX;
    private PlayerMove playerMove;


    void Start(){
        playerMove=GameObject.Find("Player").transform.Find("Body").GetComponent<PlayerMove>();
    }

    void Update()
    {

        if (!playerMove.IsStunned()){
            dirX=playerMove.GetDirX();

            if (clouds){
                if (playerMove.isSprinting()){
                    if (dirX>0)tempX=x*10;
                    else if (dirX<0)tempX=x*(-5);
                }
                else if (playerMove.isMoving()){
                    if (dirX>0)tempX=x*5;
                    else if (dirX<0)tempX=x*(-3);
                }
                else tempX=x;
                img.uvRect = new Rect(img.uvRect.position + new Vector2(tempX,y) * Time.deltaTime,img.uvRect.size);
            }
            else {
                if (bgNonClouds){
                    if (playerMove.isSprinting()){
                        if (dirX>0)tempX=0.03f;
                        else if (dirX<0)tempX=-0.03f;
                    }
                    else if (playerMove.isMoving()){
                        if (dirX>0)tempX=0.008f;
                        else if (dirX<0)tempX=-0.008f;
                    }
                    else {
                        tempX=0;
                    }
                    img.uvRect = new Rect(img.uvRect.position + new Vector2(tempX,y) * Time.deltaTime,img.uvRect.size);
                }
                else {
                    if (playerMove.isSprinting()){
                        if (dirX>0)tempX=0.05f;
                        else if (dirX<0)tempX=-0.05f;
                    }
                    else if (playerMove.isMoving()){
                        if (dirX>0)tempX=0.02f;
                        else if (dirX<0)tempX=-0.02f;
                    }
                    else {
                        tempX=0;
                    }
                    img.uvRect = new Rect(img.uvRect.position + new Vector2(tempX,y) * Time.deltaTime,img.uvRect.size);
                    }
            }
        }
        else img.uvRect = new Rect(img.uvRect.position + new Vector2(x,y) * Time.deltaTime,img.uvRect.size);
    }
}