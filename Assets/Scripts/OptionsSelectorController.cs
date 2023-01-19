using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsSelectorController : MonoBehaviour
{
    private int index;
    private bool moving;
    private List<RectTransform> selectionsTransforms;
    private List<Vector3> selectionsTargetPositions;
    private List<RectTransform> selectionsStatusTransforms;
    private List<Vector3> selectionsStatusTargetPositions;
    private AudioSource changeSelectionSound;
    [SerializeField] private float moveBy;
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<GameObject> selections;
    [SerializeField] private List<GameObject> selectionsStatus;
    [SerializeField] private MainMenuController mainMenuController;

    void Start()
    {
        index=0;
        moving=false;
        selectionsTransforms=new List<RectTransform>();
        selectionsTargetPositions=new List<Vector3>();
        selectionsStatusTransforms=new List<RectTransform>();
        selectionsStatusTargetPositions=new List<Vector3>();
        foreach(GameObject g in selections){
            g.GetComponent<Text>().color=new Color(0,0,0,0.5f);
            g.GetComponent<Text>().fontSize=25;
            selectionsTransforms.Add(g.GetComponent<RectTransform>());
        }
        selections[index].GetComponent<Text>().color=new Color(0,0,0,1);
        selections[index].GetComponent<Text>().fontSize=40;
        foreach(GameObject g in selectionsStatus){
            selectionsTransforms.Add(g.GetComponent<RectTransform>());
        }
        if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.soundIsOn=="y"){
            selectionsStatus[index].GetComponent<Text>().color=new Color(8/255f,135/255f,0,1);
            selectionsStatus[index].GetComponent<Text>().text="ON";
        }
        else if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.soundIsOn=="n"){
            selectionsStatus[index].GetComponent<Text>().color=new Color(161/255f,0,0,1);
            selectionsStatus[index].GetComponent<Text>().text="OFF";
        }
        if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.usingCheats=="y"){
            selectionsStatus[index+1].GetComponent<Text>().color=new Color(8/255f,135/255f,0,0.5f);
            selectionsStatus[index+1].GetComponent<Text>().text="ON";
        }
        else if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.usingCheats=="n"){
            selectionsStatus[index+1].GetComponent<Text>().color=new Color(161/255f,0,0,0.5f);
            selectionsStatus[index+1].GetComponent<Text>().text="OFF";
        }
        changeSelectionSound=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving){
            if (Input.GetKeyDown(KeyCode.DownArrow) && index<selections.Count-1) {
                selectionsTargetPositions.Clear();
                selectionsStatusTargetPositions.Clear();
                selections[index].GetComponent<Text>().color=new Color(0,0,0,0.5f);
                selections[index].GetComponent<Text>().fontSize=25;
                selectionsStatus[index].GetComponent<Text>().color=new Color(selectionsStatus[index].GetComponent<Text>().color.r,selectionsStatus[index].GetComponent<Text>().color.g,selectionsStatus[index].GetComponent<Text>().color.b,0.5f);
                selectionsStatus[index].GetComponent<Text>().fontSize=25;
                index++;
                foreach (RectTransform r in selectionsTransforms)selectionsTargetPositions.Add(new Vector3(r.position.x,r.position.y+moveBy,r.position.z));
                foreach (RectTransform r in selectionsStatusTransforms)selectionsStatusTargetPositions.Add(new Vector3(r.position.x,r.position.y+moveBy,r.position.z));
                moving=true;
                selections[index].GetComponent<Text>().color=new Color(0,0,0,1);
                selections[index].GetComponent<Text>().fontSize=40;
                selectionsStatus[index].GetComponent<Text>().color=new Color(selectionsStatus[index].GetComponent<Text>().color.r,selectionsStatus[index].GetComponent<Text>().color.g,selectionsStatus[index].GetComponent<Text>().color.b,1);
                selectionsStatus[index].GetComponent<Text>().fontSize=40;
                changeSelectionSound.Play();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && index>0) {
                selectionsTargetPositions.Clear();
                selectionsStatusTargetPositions.Clear();
                selections[index].GetComponent<Text>().color=new Color(0,0,0,0.5f);
                selections[index].GetComponent<Text>().fontSize=25;
                selectionsStatus[index].GetComponent<Text>().color=new Color(selectionsStatus[index].GetComponent<Text>().color.r,selectionsStatus[index].GetComponent<Text>().color.g,selectionsStatus[index].GetComponent<Text>().color.b,0.5f);
                selectionsStatus[index].GetComponent<Text>().fontSize=25;
                index--;
                foreach (RectTransform r in selectionsTransforms)selectionsTargetPositions.Add(new Vector3(r.position.x,r.position.y-moveBy,r.position.z));
                foreach (RectTransform r in selectionsStatusTransforms)selectionsStatusTargetPositions.Add(new Vector3(r.position.x,r.position.y-moveBy,r.position.z));
                moving=true;
                selections[index].GetComponent<Text>().color=new Color(0,0,0,1);
                selections[index].GetComponent<Text>().fontSize=40;
                selectionsStatus[index].GetComponent<Text>().color=new Color(selectionsStatus[index].GetComponent<Text>().color.r,selectionsStatus[index].GetComponent<Text>().color.g,selectionsStatus[index].GetComponent<Text>().color.b,1);
                selectionsStatus[index].GetComponent<Text>().fontSize=40;
                changeSelectionSound.Play();
            }
            if (Input.GetKeyDown(KeyCode.Return)){
                if (index==0){
                    if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.soundIsOn=="y"){
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.soundIsOn="n";
                        selectionsStatus[index].GetComponent<Text>().color=new Color(161/255f,0,0,1);
                        selectionsStatus[index].GetComponent<Text>().text="OFF";
                        AudioListener.pause=true;
                    }
                    else if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.soundIsOn=="n"){
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.soundIsOn="y";
                        selectionsStatus[index].GetComponent<Text>().color=new Color(8/255f,135/255f,0,1);
                        selectionsStatus[index].GetComponent<Text>().text="ON";
                        AudioListener.pause=false;
                    }
                }
                else if (index==1){
                    if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.usingCheats=="y"){
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.usingCheats="n";
                        selectionsStatus[index].GetComponent<Text>().color=new Color(161/255f,0,0,1);
                        selectionsStatus[index].GetComponent<Text>().text="OFF";
                    }
                    else if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.usingCheats=="n"){
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.usingCheats="y";
                        selectionsStatus[index].GetComponent<Text>().color=new Color(8/255f,135/255f,0,1);
                        selectionsStatus[index].GetComponent<Text>().text="ON";
                    }
                }
                else if (index==2){
                    CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.EraseData();
                }
                else if (index==3)mainMenuController.ActivateMainMenu();
            }
            if (Input.GetKeyDown(KeyCode.Escape)){
                SceneManager.LoadScene(0);
            }
        }
        else {
            for (int i=0;i<selectionsTransforms.Count;i++){
                selectionsTransforms[i].transform.position=Vector3.MoveTowards(selectionsTransforms[i].transform.position, selectionsTargetPositions[i], moveSpeed);
            }
            for (int i=0;i<selectionsStatusTransforms.Count;i++){
                selectionsStatusTransforms[i].transform.position=Vector3.MoveTowards(selectionsStatusTransforms[i].transform.position, selectionsStatusTargetPositions[i], moveSpeed);
            }
            if (Vector3.Distance(selectionsTransforms[selectionsTransforms.Count-1].position,selectionsTargetPositions[selectionsTransforms.Count-1])<=0.1f){
                moving=false;
            }
        }
    }
}
