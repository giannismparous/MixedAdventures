using UnityEngine;
using System.IO;

public class CurrentGameData
{
    public static CurrentGameData currentGameData=new CurrentGameData();
    public JsonReadWriteFilesToSystem jsonReadWriteFilesToSystemInstance;

    public int currentLevelIndex;
    public int newPlayerIndex=-1;
    public int playerID;

    public CurrentGameData(){
        jsonReadWriteFilesToSystemInstance=new JsonReadWriteFilesToSystem();
    }

    public void SetPlayerID(int id){
        playerID=id;
    }

    public int GetPlayerID(){
        return playerID;
    }

}
