using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiglettJumped : MonoBehaviour
{
    private DiglettMove diglettMove;

    void Start() {
        diglettMove =transform.parent.transform.Find("DiglettBody").GetComponent<DiglettMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<CheckEnemyHead>() && !diglettMove.IsUnderground())
        {
            diglettMove.Jumped();
        }
    }
}
