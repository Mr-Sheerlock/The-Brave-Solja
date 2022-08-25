using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera LevelCamera;
    [SerializeField] Transform[] Positions;
    float Delay=2;

    //LevelCamera.Follow = Levels[CurrentLevelIndex].LevelObject.transform;
    //        LevelCamera.LookAt = Levels[CurrentLevelIndex].LevelObject.transform;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Maybe Control the HUD from here??? Up and down and enter etc
        
    }
}
