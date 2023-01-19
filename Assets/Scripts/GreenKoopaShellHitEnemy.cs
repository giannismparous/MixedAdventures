using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKoopaShellHitEnemy : MonoBehaviour
{

    private GreenKoopaShellMove greenKoopaShellMove;

    void Start()
    {
        greenKoopaShellMove=transform.GetComponent<GreenKoopaShellMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (greenKoopaShellMove.IsMoving() && other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject.transform.parent.gameObject);
            greenKoopaShellMove.InvertDirX();
        }
    }
}
