using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCheckerBowserDoor : MonoBehaviour
{
    private Transform point1;
    private Transform point2;
    private Animator anim;
    private AnimatorClipInfo[] animatorinfo;
    private string current_animation;
    private bool openedDoor;
    private AudioSource bgMusic1;
    private AudioSource bgMusic2;
    private GameObject BGCanvas;
    [SerializeField] private bool fromMusic1ToMusic2;
    [SerializeField] private Transform bowserStartingPointTransform;

    void Start()
    {
        point1=transform.parent.transform.Find("Point1").transform;
        point2=transform.parent.transform.Find("Point2").transform;
        transform.parent.transform.Find("BowserDoorBody").position=point1.position;
        anim=transform.parent.transform.Find("BowserDoorBody").GetComponent<Animator>();
        openedDoor=false;
        bgMusic1=GameObject.Find("BGMusic1").GetComponent<AudioSource>();
        bgMusic2=GameObject.Find("BGMusic2").GetComponent<AudioSource>();
        BGCanvas=GameObject.Find("BGCanvas").gameObject;
    }

    void Update(){

        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        current_animation = animatorinfo[0].clip.name;

        if (current_animation.Equals("bowser_door_open") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
            if (bowserStartingPointTransform!=null)GameObject.Find("Bowser").transform.position=bowserStartingPointTransform.transform.position;
            transform.parent.transform.Find("BowserDoorBody").transform.position=point2.position;
            GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer().transform.position=point2.position;
            anim.SetTrigger("closeDoor");
            if (fromMusic1ToMusic2){
                bgMusic1.Stop();
                bgMusic2.Play();
                BGCanvas.transform.Find("BGBushes").gameObject.SetActive(false);
                BGCanvas.transform.Find("BGSkyWithClouds").gameObject.SetActive(false);
                BGCanvas.transform.Find("BGClouds").gameObject.SetActive(false);
            }
            else {
                bgMusic2.Stop();
                bgMusic1.Play();
                BGCanvas.transform.Find("BGBushes").gameObject.SetActive(true);
                BGCanvas.transform.Find("BGSkyWithClouds").gameObject.SetActive(true);
                BGCanvas.transform.Find("BGClouds").gameObject.SetActive(true);
            }
        }
        

    }

    void OnTriggerStay2D(Collider2D other)
    {

        
        if (other.CompareTag("PlayerButtonCheck") && !openedDoor)
        {
            if (Input.GetKey(KeyCode.Return)) {
                anim.SetTrigger("openDoor");
                openedDoor=true;
            }
        }

    }


    public void Reset(){
        anim.SetTrigger("reset");
        openedDoor=false;
    }

}
