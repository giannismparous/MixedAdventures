using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuAvailableLevels : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> levels;


    void Start(){
        int biggestLevelIndex=int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.reachedLevel);
        for (int i=biggestLevelIndex+1;i<levels.Count;i++)levels[i].GetComponent<Text>().text="???";
    }

}
