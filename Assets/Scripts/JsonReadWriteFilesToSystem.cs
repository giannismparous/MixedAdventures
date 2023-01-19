using UnityEngine;
using System.IO;

public class JsonReadWriteFilesToSystem
{

    public PlayerData playerData;

    public JsonReadWriteFilesToSystem(){
        LoadFromJSON();
    }

    public void SaveToJSON(){
        string json = JsonUtility.ToJson(playerData,true);
        File.WriteAllText(Application.streamingAssetsPath+"/PlayerDataFile.json",json);
        Debug.Log("Saved Player Data");
    }

    public void LoadFromJSON(){
        playerData=new PlayerData();
        string json = File.ReadAllText(Application.streamingAssetsPath+ "/PlayerDataFile.json");
        playerData= JsonUtility.FromJson<PlayerData>(json);
        Debug.Log("Loaded Player Data");
    }

    public void EraseData(){
        playerData.unlockedMario="y";
        playerData.unlockedSonic="n";
        playerData.unlockedPacMan="n";
        playerData.unlockedPikachu="n";
        playerData.unlockedMitsos="n";
        playerData.unlockedAntigoni="n";
        playerData.unlockedGiorgakis="n";
        playerData.unlockedMpario="n";
        playerData.coins="0";
        playerData.reachedLevel="0";
        playerData.usingCheats="n";
        playerData.soundIsOn="y";
    }
}
