using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioPicker : MonoBehaviour
{

    private PlayerMove playerMove;
    [SerializeField] private AudioClip itemPickSound;

    void Start(){
        playerMove=transform.GetComponent<PlayerMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("RedMushroom")){
            playerMove.JumpHigh();
            AudioSource.PlayClipAtPoint(itemPickSound, GameObject.Find("Camera").transform.position,1);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("MarioFireFlower")){
            playerMove.FireBall();
            AudioSource.PlayClipAtPoint(itemPickSound, GameObject.Find("Camera").transform.position,1);
            Destroy(other.gameObject);
        }

    }
}
