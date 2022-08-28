using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;



public class MainMenu : MonoBehaviour
{

    [SerializeField] bool FinishGame=false;
    [SerializeField] GameObject MainMenuu;
    [SerializeField] GameObject FinishPrompt;

    void Start()
    {
        //CameraBrain.m_DefaultBlend.m_Time = AnimationTime_s;
        if (PlayerPrefs.GetInt("FinishGame") != 0 || FinishGame)
        {
            FinishGame=true;
            MainMenuu.active = false;
            FinishPrompt.active = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    
    public void QuitGame()
    {
        Application.Quit(); 
    }

  
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void AfterFinish()
    {
        FinishPrompt.active = false;
        MainMenuu.active = true;
    }
}
