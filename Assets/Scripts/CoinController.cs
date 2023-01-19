using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            
            AudioSource.PlayClipAtPoint(clip, GameObject.Find("Camera").transform.position,1);
            Destroy(transform.gameObject);
        }

    }
}
