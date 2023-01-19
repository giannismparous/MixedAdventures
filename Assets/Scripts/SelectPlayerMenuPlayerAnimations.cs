using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlayerMenuPlayerAnimations : MonoBehaviour
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
        if (mario && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMario=="n")spriteRenderer.color=new Color(0,0,0,1);
        else if (sonic && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedSonic=="n")spriteRenderer.color=new Color(0,0,0,1);
        else if (pacman && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPacMan=="n")spriteRenderer.color=new Color(0,0,0,1);
        else if (pikachu && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPikachu=="n")spriteRenderer.color=new Color(0,0,0,1);
        else if (mitsos && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMitsos=="n")spriteRenderer.color=new Color(0,0,0,1);
        else if (antigoni && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedAntigoni=="n")spriteRenderer.color=new Color(0,0,0,1);
        else if (giorgakis && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedGiorgakis=="n")spriteRenderer.color=new Color(0,0,0,1);
        else if (mpario && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMpario=="n")spriteRenderer.color=new Color(0,0,0,1);
        anim=GetComponent<Animator>();
        if (!sonic)anim.SetBool("isRunning",true);
    }

    void Update(){
        if (mitsos && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMitsos=="y")spriteRenderer.color=new Color(1,1,1,1);
        if (antigoni && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedAntigoni=="y")spriteRenderer.color=new Color(1,1,1,1);
        if (giorgakis && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedGiorgakis=="y")spriteRenderer.color=new Color(1,1,1,1);
        if (mpario && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedMpario=="y")spriteRenderer.color=new Color(1,1,1,1);
    }

}
