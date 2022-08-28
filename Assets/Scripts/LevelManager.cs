using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] AudioClip BossClip;

    
    public void PauseToggle()
    {
        if (PauseMenu.active == false)
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void MusicChange()
    {
        var AS = gameObject.GetComponent<AudioSource>();
        AS.Stop();
        AS.clip = BossClip;
        AS.Play();
    }

    public void NextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(currentIndex+1);
        }
        else
        {
            //last level

            //set game finish to true
            PlayerPrefs.SetInt("FinishGame", 1);
            SceneManager.LoadScene(0);

        }
    }
}
