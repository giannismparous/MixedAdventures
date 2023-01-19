using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsCounterSelectPlayerMenuCanvas : MonoBehaviour
{

    private Text coinsCounter;
    
    void Start()
    {
        coinsCounter=GetComponent<Text>();
    }

    void Update()
    {
        coinsCounter.text="X "+CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins;
    }
}
