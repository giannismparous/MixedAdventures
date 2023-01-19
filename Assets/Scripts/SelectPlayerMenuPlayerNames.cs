using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayerMenuPlayerNames : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    [SerializeField] private bool mario;
    [SerializeField] private bool sonic;
    [SerializeField] private bool pacman;
    [SerializeField] private bool pikachu;
    [SerializeField] private bool mitsos;
    [SerializeField] private bool antigoni;
    [SerializeField] private bool giorgakis;
    [SerializeField] private bool mpario;

    void Start(){
        spriteRenderer=GetComponent<SpriteRenderer>();
        if (mario && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMario=="n")GetComponent<Text>().text="???";
        else if (sonic && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedSonic=="n")GetComponent<Text>().text="???";
        else if (pacman && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPacMan=="n")GetComponent<Text>().text="???";
        else if (pikachu && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPikachu=="n")GetComponent<Text>().text="???";
        else if (mitsos && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMitsos=="n")GetComponent<Text>().text="???";
        else if (antigoni && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedAntigoni=="n")GetComponent<Text>().text="???";
        else if (giorgakis && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedGiorgakis=="n")GetComponent<Text>().text="???";
        else if (mpario && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMpario=="n")GetComponent<Text>().text="???";
    }

    void Update(){
        if (mitsos && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMitsos=="y")GetComponent<Text>().text="Mitsos";
        if (antigoni && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedAntigoni=="y")GetComponent<Text>().text="Antigoni";
        if (giorgakis && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedGiorgakis=="y")GetComponent<Text>().text="Giorgakis";
        if (mpario && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMpario=="y")GetComponent<Text>().text="Mpario";
    }

}
