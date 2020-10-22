using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameMode{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;
    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButton;
    public Text highScoreScreen;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";
    // Start is called before the first frame update
    void Awake(){
        highScoreScreen.enabled = false;
    }
    void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        if(!PlayerPrefs.HasKey("HighScore0")){
            PlayerPrefs.SetInt("HighScore0", 20);
        }
        if(!PlayerPrefs.HasKey("HighScore1")){
            PlayerPrefs.SetInt("HighScore1", 20);
        }
        if(!PlayerPrefs.HasKey("HighScore2")){
            PlayerPrefs.SetInt("HighScore2", 20);
        }
        if(!PlayerPrefs.HasKey("HighScore3")){
            PlayerPrefs.SetInt("HighScore3", 20);
        }
        
        StartLevel();
    }

    void StartLevel(){
        if(castle != null){
            Destroy(castle);
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos){
            Destroy (pTemp);
        }

        castle = Instantiate<GameObject>( castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false;
        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI(){
        uitLevel.text = "Level: "+(level+1) +" of " +levelMax;
        uitShots.text = "Shots Taken: " +shotsTaken;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        if((mode == GameMode.playing) && Goal.goalMet){
            mode = GameMode.levelEnd;

            SwitchView("Show Both");
            if(level == 0){
                if(PlayerPrefs.GetInt("HighScore0") > shotsTaken){
                    PlayerPrefs.SetInt("HighScore0", shotsTaken);
                }
                highScoreScreen.text = "Level Highscore: " +shotsTaken;
            }
            if(level == 1){
                if(PlayerPrefs.GetInt("HighScore1") > shotsTaken){
                    PlayerPrefs.SetInt("HighScore1", shotsTaken);
                }
                highScoreScreen.text = "Level Highscore: " +shotsTaken;
            }
            if(level == 2){
                if(PlayerPrefs.GetInt("HighScore2") > shotsTaken){
                    PlayerPrefs.SetInt("HighScore2", shotsTaken);
                }
                highScoreScreen.text = "Level Highscore: " +shotsTaken;
            }
            if(level == 3){
                if(PlayerPrefs.GetInt("HighScore3") > shotsTaken){
                    PlayerPrefs.SetInt("HighScore3", shotsTaken);
                }
                highScoreScreen.text = "Level Highscore: " +shotsTaken;
            }



            highScoreScreen.enabled = true;
            Invoke("Nextlevel", 2f);
        }
    }

    void Nextlevel(){
        highScoreScreen.enabled = false;
        level++;
        if(level == levelMax){
            level = 0;
            SceneManager.LoadScene("_Scene_Start");
        }
        StartLevel();
    }

    public void SwitchView(string eView = ""){
        if(eView == ""){
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
        
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired(){
        S.shotsTaken++;
    }
}
