using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickRectangle : MonoBehaviour
{

    private Collider2D brickCollider;

    void Start()
    {
        brickCollider=transform.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount==1)brickCollider.enabled=false;
        else if (transform.childCount==0)Destroy(transform.gameObject);
    }
}
