using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private GameObject selector;
    private GameObject optionsSelector;

    void Start()
    {
        selector=transform.Find("Selector").gameObject;
        optionsSelector=transform.Find("OptionsSelector").gameObject;
        if (CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.soundIsOn=="n")AudioListener.pause=true;
        optionsSelector.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateMainMenu(){
        selector.SetActive(true);
        optionsSelector.SetActive(false);
    }

    public void ActivateOptionsMenu(){
        selector.SetActive(false);
        optionsSelector.SetActive(true);
    }

}
