using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Script found and used at https://www.sitepoint.com/adding-pause-main-menu-and-game-over-screens-in-unity/

public class UIControl : MonoBehaviour
{
    public GameObject panel;
    GameObject[] pauseObjects;
    GameObject[] menuObjects;
    GameObject[] levelSelectObjects;
    bool activeGameOver = false;
    bool mainMenu = false;

    void Start()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        menuObjects = GameObject.FindGameObjectsWithTag("Menu");
        levelSelectObjects = GameObject.FindGameObjectsWithTag("LevelSelect");
        if(panel==null)
        panel = GameObject.Find("GameOverText");
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            mainMenu = true;
            showMenu();
        }
        hidePaused();
    }

    void Update()
    {
        if (mainMenu == false)
        {
            activeGameOver = panel.activeSelf;
        }
        if (activeGameOver == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 1)
                {
                    Time.timeScale = 0;
                    showPaused();
                }
                else if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                    hidePaused();
                }
            }
        }
    }

    //shows all menu buttons and makes sure level select isn't shown
    public void showMenu()
    {
        foreach (GameObject x in levelSelectObjects)
        {
            x.SetActive(false);
        }
        foreach (GameObject x in menuObjects)
        {
            x.SetActive(true);
        }     
    }

    //same as showmenu() but other way around
    public void showLevelSelect()
    {
        foreach (GameObject x in menuObjects)
        {
            x.SetActive(false);
        }
        foreach (GameObject x in levelSelectObjects)
        {
            x.SetActive(true);
        }
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void pause()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }
        else
        {
            Debug.Log("Already paused");
        }
    }

    public void unPause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
        else
        {
            Debug.Log("Already playing");
        }
    }


    public void showPaused()
    {
        foreach (GameObject x in pauseObjects)
        {
            x.SetActive(true);
        }
    }

    public void hidePaused()
    {
        foreach (GameObject x in pauseObjects)
        {
            x.SetActive(false);
        }
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
