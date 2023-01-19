using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItemController : MonoBehaviour
{

    [SerializeField] private float jumpForce;
    [SerializeField] private GameObject itemPrefab;
    private bool appeared;
    private bool addedForce;

    void Start(){
        appeared=false;
        addedForce=false;
    }

    void FixedUpdate(){
        
        if (appeared && !addedForce){
            SpawnItem();
            addedForce=true;
        }
    }

    void SpawnItem(){
        GameObject temp;
        temp = Instantiate(itemPrefab) as GameObject;
        temp.transform.position=transform.position;
        temp.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce);
    }

    public void Appear(){
        appeared=true;
    }

}
