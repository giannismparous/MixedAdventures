using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingFlagController : MonoBehaviour
{

    private Animator anim;
    private bool loadingNextLevel;
    private PlayerMove playerMove;
    private bool triggered;
    private AudioSource audioSource;
    [SerializeField] private PlayerUnlocked playerUnlocked;
    [SerializeField] private float nextLevelTimer;

    void Start(){
        anim=GetComponent<Animator>();
        GameObject player = GameObject.Find("InitializeScene").GetComponent<SceneInitializer>().GetPlayer();
        playerMove = player.transform.Find("Body").GetComponent<PlayerMove>();
        audioSource=GetComponent<AudioSource>();
    }

    void Update(){
        if (loadingNextLevel){
            if (nextLevelTimer<=0)SceneManager.LoadScene(1);
            else nextLevelTimer-=Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && !triggered)
        {
            audioSource.Play();
            if (GameObject.Find("BG Music")!=null)GameObject.Find("BG Music").GetComponent<AudioSource>().Stop();
            if (GameObject.Find("BGMusic1")!=null)GameObject.Find("BGMusic1").GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().Play();
            CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.coins=playerMove.GetCoins().ToString();
            anim.SetTrigger("checked");
            int levelIndex=CurrentGameData.currentGameData.currentLevelIndex-3;
            if (levelIndex!=15 && levelIndex+1>int.Parse(CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.reachedLevel))CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.reachedLevel=(levelIndex+1).ToString();
            if (levelIndex==3 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedSonic=="n"){
                CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedSonic="y";
                CurrentGameData.currentGameData.newPlayerIndex=1;
                playerUnlocked.ActivateAnimation();
            }
            else if (levelIndex==7 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPacMan=="n"){
                CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPacMan="y";
                CurrentGameData.currentGameData.newPlayerIndex=2;
                playerUnlocked.ActivateAnimation();
            }
            else if (levelIndex==11 && CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPikachu=="n"){
                CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.playerData.unlockedPikachu="y";
                CurrentGameData.currentGameData.newPlayerIndex=3;
                playerUnlocked.ActivateAnimation();
            }
            loadingNextLevel=true;
            triggered=true;
            CurrentGameData.currentGameData.jsonReadWriteFilesToSystemInstance.SaveToJSON();
        }

    }
}
