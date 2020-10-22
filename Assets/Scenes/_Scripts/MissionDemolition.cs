﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        GameObject levelBox = GameObject.Find("UIText_Level");
        uitLevel = levelBox.GetComponent<Text>();
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

    void UpdateGUI{
        print("hi");
        //uitLevel.text = "Level: "+(level+1) +" of " +levelMax;
        //uitShots.text = "Shots Taken: " +shotsTaken;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        if((mode == GameMode.playing) && Goal.goalMet){
            mode = GameMode.levelEnd;

            SwitchView("Show Both");

            Invoke("Nextlevel", 2f);
        }
    }

    void Nextlevel(){
        level++;
        if(level == levelMax){
            level = 0;
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
