using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent LevelStart;
    public UnityEvent SecretBossFinished;

    [SerializeField] GameObject Player;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject GameOverMenu;
    [SerializeField] AudioClip BossClip;
    [SerializeField] AudioClip MainBGClip;
    [SerializeField] AudioClip RoomSongClip;
    bool gameOver=false;
    
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
        LevelStart.Invoke();
        if (PlayerPrefs.GetInt("SecretBoss") != 0)
        {
            SecretBossFinished.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
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

    public void BossMusicChange()
    {
        var AS = gameObject.GetComponent<AudioSource>();
        AS.Stop();
        AS.clip = BossClip;
        AS.Play();
        AS.volume = 0.4f;
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

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        GameOverMenu.SetActive(true);
        gameOver = true;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }


    public void EnterRoomMusicChange()
    {
        var AS = gameObject.GetComponent<AudioSource>();
        AS.Stop();
        AS.clip = RoomSongClip;
        AS.Play();
        AS.volume = 0.5f;
    }

    public void ExitRoomMusicChange()
    {
        var AS = gameObject.GetComponent<AudioSource>();
        AS.Stop();
        AS.clip = MainBGClip;
        AS.Play();
        AS.volume = 0.2f;
    }


    public void SecretBossFinish()
    {
        PlayerPrefs.SetInt("SecretBoss", 1);
    }
}
