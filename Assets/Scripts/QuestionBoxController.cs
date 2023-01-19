using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxController : MonoBehaviour
{
   private Animator anim;
   private SpawnItemController spawnItemController;
   private PlayerMove playerMove;
   private bool hit;
   [SerializeField] private bool containsItem;
   [SerializeField] private AudioClip clip;

    void Start()
    {
        anim=transform.GetComponent<Animator>();
        if (containsItem)spawnItemController=transform.Find("SpawnerWithItem").transform.GetComponent<SpawnItemController>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("PlayerHead") && !hit)
        {
            if (containsItem){
                anim.SetTrigger("hit");
                spawnItemController.Appear();
            }
            else {
                AudioSource.PlayClipAtPoint(clip, GameObject.Find("Camera").transform.position,1);
                anim.SetTrigger("hitWithCoin");
                playerMove.AddCoin();
            }
            hit=true;
        }

    }
}
