using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectorController : MonoBehaviour
{
    
    private int index;
    private List<Vector3> selectionsTargetPositions;
    private bool moving;
    private List<RectTransform> selectionsTransforms;
    private List<Vector3> charactersTransformsTargetPositions;
    private GameObject BGCanvasGrassland;
    private GameObject BGCanvasDesert;
    private GameObject BGCanvasForest;
    private GameObject BGCanvasDarkWorld;
    private AudioSource changeSelectionSound;
    private GameObject coinsToUnlockNewPlayerCanvas;
    [SerializeField] private bool levelMenu;
    [SerializeField] private bool selectPlayerMenu;
    [SerializeField] private bool stopGameMenu;
    [SerializeField] private float moveBy;
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<GameObject> selections;
    [SerializeField] private List<Transform> charactersTransforms;
    [SerializeField] private MainMenuController mainMenuController;

    void Start()
    {
        index=0;
        moving=false;
        selectionsTransforms=new List<RectTransform>();
        selectionsTargetPositions=new List<Vector3>();
        charactersTransformsTargetPositions=new List<Vector3>();
        foreach(GameObject g in selections){
            g.GetComponent<Text>().color=new Color(0,0,0,0.5f);
            g.GetComponent<Text>().fontSize=25;
            selectionsTransforms.Add(g.GetComponent<RectTransform>());
        }
        if (selectPlayerMenu){
            if (CurrentGameData.currentGameData.newPlayerIndex>0){
                selections[index].GetComponent<Text>().color=new Color(224/255f,231/255f,34/255f,0.5f);
            }
            coinsToUnlockNewPlayerCanvas=GameObject.Find("UnlockPlayerCoinsCanvas").gameObject;
        }
        selections[index].GetComponent<Text>().color=new Color(0,0,0,1);
        selections[index].GetComponent<Text>().fontSize=40;
        if (levelMenu){
            BGCanvasGrassland=transform.parent.transform.parent.transform.Find("BGCanvasGrassland").transform.gameObject;
            BGCanvasDesert=transform.parent.transform.parent.transform.Find("BGCanvasDesert").transform.gameObject;
            BGCanvasForest=transform.parent.transform.parent.transform.Find("BGCanvasForest").transform.gameObject;
            BGCanvasDarkWorld=transform.parent.transform.parent.transform.Find("BGCanvasDarkWorld").transform.gameObject;
            ChooseBG();
        }
        changeSelectionSound=GetComponent<AudioSource>();
    }

    void Update()
    {

        if (!moving){

            if (selectPlayerMenu){
                    if (index==4 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMitsos=="n"){
                        coinsToUnlockNewPlayerCanvas.SetActive(true);
                    }
                    else if (index==5 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedAntigoni=="n"){
                        coinsToUnlockNewPlayerCanvas.SetActive(true);
                    }
                    else if (index==6 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedGiorgakis=="n"){
                        coinsToUnlockNewPlayerCanvas.SetActive(true);
                    }
                    else if (index==7 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMpario=="n"){
                        coinsToUnlockNewPlayerCanvas.SetActive(true);
                    }
                    else coinsToUnlockNewPlayerCanvas.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && index<selections.Count-1) {
                selectionsTargetPositions.Clear();
                charactersTransformsTargetPositions.Clear();
                selections[index].GetComponent<Text>().color=new Color(0,0,0,0.5f);
                selections[index].GetComponent<Text>().fontSize=25;
                index++;
                foreach (RectTransform r in selectionsTransforms)selectionsTargetPositions.Add(new Vector3(r.position.x,r.position.y+moveBy,r.position.z));
                foreach (Transform c in charactersTransforms)charactersTransformsTargetPositions.Add(new Vector3(c.position.x,c.position.y+moveBy,c.position.z));
                moving=true;
                selections[index].GetComponent<Text>().color=new Color(0,0,0,1);
                selections[index].GetComponent<Text>().fontSize=40;
                if (levelMenu)ChooseBG();
                changeSelectionSound.Play();
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) && index>0) {
                selectionsTargetPositions.Clear();
                charactersTransformsTargetPositions.Clear();
                selections[index].GetComponent<Text>().color=new Color(0,0,0,0.5f);
                selections[index].GetComponent<Text>().fontSize=25;
                index--;
                foreach (RectTransform r in selectionsTransforms)selectionsTargetPositions.Add(new Vector3(r.position.x,r.position.y-moveBy,r.position.z));
                foreach (Transform c in charactersTransforms)charactersTransformsTargetPositions.Add(new Vector3(c.position.x,c.position.y-moveBy,c.position.z));
                moving=true;
                selections[index].GetComponent<Text>().color=new Color(0,0,0,1);
                selections[index].GetComponent<Text>().fontSize=40;
                if (levelMenu)ChooseBG();
                changeSelectionSound.Play();
            }
            if (Input.GetKeyDown(KeyCode.Return)){
                if (levelMenu){
                    if (index<=int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.reachedLevel)){
                        CurrentGameData.currentGameData.currentLevelIndex=index+3;
                        SceneManager.LoadScene(2);
                    }
                }
                else if (selectPlayerMenu){
                    if (index==0 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMario=="y"){
                        CurrentGameData.currentGameData.SetPlayerID(index);
                        SceneManager.LoadScene(CurrentGameData.currentGameData.currentLevelIndex);
                    }
                    else if (index==1 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedSonic=="y"){
                        CurrentGameData.currentGameData.SetPlayerID(index);
                        SceneManager.LoadScene(CurrentGameData.currentGameData.currentLevelIndex);
                    }
                    else if (index==2 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPacMan=="y"){
                        CurrentGameData.currentGameData.SetPlayerID(index);
                        SceneManager.LoadScene(CurrentGameData.currentGameData.currentLevelIndex);
                    }
                    else if (index==3 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPikachu=="y"){
                        CurrentGameData.currentGameData.SetPlayerID(index);
                        SceneManager.LoadScene(CurrentGameData.currentGameData.currentLevelIndex);
                    }
                    else if (index==4 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMitsos=="y"){
                        CurrentGameData.currentGameData.SetPlayerID(index);
                        SceneManager.LoadScene(CurrentGameData.currentGameData.currentLevelIndex);
                    }
                    else if (index==5 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedAntigoni=="y"){
                        CurrentGameData.currentGameData.SetPlayerID(index);
                        SceneManager.LoadScene(CurrentGameData.currentGameData.currentLevelIndex);
                    }
                    else if (index==6 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedGiorgakis=="y"){
                        CurrentGameData.currentGameData.SetPlayerID(index);
                        SceneManager.LoadScene(CurrentGameData.currentGameData.currentLevelIndex);
                    }
                    else if (index==7 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMpario=="y"){
                        CurrentGameData.currentGameData.SetPlayerID(index);
                        SceneManager.LoadScene(CurrentGameData.currentGameData.currentLevelIndex);
                    }
                    else if (index==4 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMitsos=="n" && int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins)>=300){
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMitsos="y";
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins=(int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins)-300).ToString();
                    }
                    else if (index==5 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedAntigoni=="n" && int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins)>=300){
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedAntigoni="y";
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins=(int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins)-300).ToString();
                    }
                    else if (index==6 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedGiorgakis=="n" && int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins)>=300){
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedGiorgakis="y";
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins=(int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins)-300).ToString();
                    }
                    else if (index==7 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMpario=="n" && int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins)>=300){
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMpario="y";
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins=(int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins)-300).ToString();
                    }
                }
                else if (!stopGameMenu){
                    if (index==0)SceneManager.LoadScene(1);
                    else if (index==1)mainMenuController.ActivateOptionsMenu();
                    else if (index==2){
                        Debug.Log("Quit");
                        CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.SaveToJSON();
                        Application.Quit();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape)){
                if (levelMenu)SceneManager.LoadScene(0);
                else if (selectPlayerMenu)SceneManager.LoadScene(1);
            }
        }
        else {
            for (int i=0;i<selectionsTransforms.Count;i++){
                selectionsTransforms[i].transform.position=Vector3.MoveTowards(selectionsTransforms[i].transform.position, selectionsTargetPositions[i], moveSpeed);
            }
            for (int i=0;i<charactersTransforms.Count;i++){
                charactersTransforms[i].transform.position=Vector3.MoveTowards(charactersTransforms[i].transform.position, charactersTransformsTargetPositions[i], moveSpeed);
            }
            if (Vector3.Distance(selectionsTransforms[selectionsTransforms.Count-1].position,selectionsTargetPositions[selectionsTransforms.Count-1])<=0.1f){
                moving=false;
            }
        }

    }

    void ChooseBG(){
        if (index<4){
            BGCanvasGrassland.SetActive(true);
            transform.SetParent(transform.parent.transform.parent.Find("BGCanvasGrassland"),false);
            BGCanvasDesert.SetActive(false);
            BGCanvasForest.SetActive(false);
            BGCanvasDarkWorld.SetActive(false);
        }
        else if (index<8){
            BGCanvasDesert.SetActive(true);
            transform.SetParent(transform.parent.transform.parent.Find("BGCanvasDesert"),false);
            BGCanvasGrassland.SetActive(false);
            BGCanvasForest.SetActive(false);
            BGCanvasDarkWorld.SetActive(false);
        }
        else if (index<12){
            BGCanvasForest.SetActive(true);
            transform.SetParent(transform.parent.transform.parent.Find("BGCanvasForest"),false);
            BGCanvasGrassland.SetActive(false);
            BGCanvasDesert.SetActive(false);
            BGCanvasDarkWorld.SetActive(false);
        }
        else if (index<16){
            BGCanvasDarkWorld.SetActive(true);
            transform.SetParent(transform.parent.transform.parent.Find("BGCanvasDarkWorld"),false);
            BGCanvasGrassland.SetActive(false);
            BGCanvasDesert.SetActive(false);
            BGCanvasForest.SetActive(false);
        }
    }

    public int GetIndex(){
        return index;
    }
}
