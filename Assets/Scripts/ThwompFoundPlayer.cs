using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThwompFoundPlayer : MonoBehaviour
{

    private ThwompMove thwompMove;
    
    void Start()
    {
        thwompMove=transform.parent.transform.Find("ThwompBody").GetComponent<ThwompMove>();
    }

    void OnTriggerStay2D(Collider2D collision){

        if (collision.CompareTag("Player") && !thwompMove.IsRecovering() && !thwompMove.IsTriggered())thwompMove.Trigger();

    }

}
