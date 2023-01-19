using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChecker : MonoBehaviour
{
    private Transform teleportPositionTransform;

    void Start()
    {
        teleportPositionTransform=transform.Find("TeleportPosition").transform;
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("PlayerButtonCheck"))
        {
            if (Input.GetKeyDown(KeyCode.Return)) {
                GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer().transform.position=teleportPositionTransform.position;
            }
        }

    }
}
