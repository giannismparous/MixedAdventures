using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAvailability : MonoBehaviour
{
    private bool available;

    void Start() {
        available = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Ground")||other.CompareTag("Enemy"))available = false;

    }

    void OnTriggerExit2D(Collider2D other)
    {

        available = true;

    }

    public bool IsAvailable() {
        return available;
    }
}
