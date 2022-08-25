using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;



public class MainMenu : MonoBehaviour
{

    
    

    void Start()
    {
        //CameraBrain.m_DefaultBlend.m_Time = AnimationTime_s;
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
}
