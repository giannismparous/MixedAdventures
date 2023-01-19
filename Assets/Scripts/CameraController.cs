using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    private float initialY;
    private Transform cameraPositionTransform;
    public bool goUpwards;
    private Camera cam;
    private int zoomIndex;
    float curZoomPos, zoomTo; // curZoomPos will be the value
    float zoomFrom = 20f; //Midway point between nearest and farthest zoom values (a "starting position")

    void Start()
    {
        cameraPositionTransform= GameObject.Find("Player").transform.Find("CameraPosition").transform;
        initialY=cameraPositionTransform.position.y;
        cam=GameObject.Find("Camera").GetComponent<Camera>();
    }
     
     void Update ()
     {
        
        if (Input.GetKeyDown(KeyCode.I)){
            if (zoomIndex<0){
                zoomTo -= 5f;
                Debug.Log ("Zoomed In");
                zoomIndex++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.O)){
            if (zoomIndex>-3){
            zoomTo += 5f;
            Debug.Log ("Zoomed Out");
            zoomIndex--;
            }
        }
        
        // creates a value to raise and lower the camera's field of view
        curZoomPos =  zoomFrom + zoomTo;
        
        curZoomPos = Mathf.Clamp (curZoomPos, 5f, 35f);
        
        // Stops "zoomTo" value at the nearest and farthest zoom value you desire
        zoomTo = Mathf.Clamp (zoomTo, -15f, 30f);
        
        // Makes the actual change to Field Of View
        cam.fieldOfView = curZoomPos;

        if (goUpwards)transform.position=new Vector3(cameraPositionTransform.position.x,cameraPositionTransform.position.y,transform.position.z);
        else transform.position=new Vector3(cameraPositionTransform.position.x,initialY,transform.position.z);
     }
}
