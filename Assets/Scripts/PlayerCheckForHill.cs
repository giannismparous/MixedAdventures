using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckForHill : MonoBehaviour
{

    private bool onHill;
    private PlayerMove playerMove;
    private int lastRotation;

    void Start(){
        playerMove=transform.parent.transform.Find("Body").GetComponent<PlayerMove>();
        onHill=false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!onHill){

            if (other.CompareTag("UpwardHill"))
            {
                playerMove.SetCurrentRotation(new Vector3(0,0,30));
                onHill=true;
                lastRotation=30;
            }
            else if (other.CompareTag("DownwardHill")){
                playerMove.SetCurrentRotation(new Vector3(0,0,-30));
                onHill=true;
                lastRotation=-30;
            }

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("UpwardHill") || other.CompareTag("DownwardHill"))
        {
            playerMove.SetCurrentRotation(new Vector3(0,0,-lastRotation));
            onHill=false;
            lastRotation=0;
        }

    }

    public bool IsOnHill(){
        return onHill;
    }


}
