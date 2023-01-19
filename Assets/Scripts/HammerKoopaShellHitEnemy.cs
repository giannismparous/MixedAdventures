using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerKoopaShellHitEnemy : MonoBehaviour
{

    [SerializeField] private HammerKoopaShellMove hammerKoopaShellMove;

    void Start()
    {
        hammerKoopaShellMove=transform.GetComponent<HammerKoopaShellMove>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hammerKoopaShellMove.IsMoving() && other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject.transform.parent.gameObject);
            hammerKoopaShellMove.InvertDirX();
        }
    }
}
