using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKoopaShellHitEnemy : MonoBehaviour
{

    [SerializeField] private RedKoopaShellMove redKoopaShellMove;

    void Start()
    {
        redKoopaShellMove=transform.GetComponent<RedKoopaShellMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (redKoopaShellMove.IsMoving() && other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject.transform.parent.gameObject);
            redKoopaShellMove.InvertDirX();
        }
    }
}
