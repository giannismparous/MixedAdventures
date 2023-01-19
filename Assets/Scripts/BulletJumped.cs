using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletJumped : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>())
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
