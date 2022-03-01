using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Terminal : MonoBehaviour
{
    public GameObject controled;
    public AudioClip[] sounds;
    public GameObject mainCamera;
    public GameObject gameWonPanel;
    public GameObject needUsbPanel;
    int nextScene;

    // Start is called before the first frame update
    void Start()
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (gameObject.name == "EndTerminal")
        {
            gameWonPanel.SetActive(false);
            needUsbPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "EndTerminal")
        {
            if (gameWonPanel.activeSelf == true)
            {
                StartCoroutine(Proceed());
            }
            if (needUsbPanel.activeSelf == true)
            {
                StartCoroutine(TextDisappear());
            }
        }
    }

    IEnumerator Proceed()
    {
        yield return new WaitForSeconds(2);
        if (Input.anyKeyDown)
        {
            if(nextScene < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
        }
    }

    IEnumerator TextDisappear()
    {
        yield return new WaitForSeconds(5);
        needUsbPanel.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gameObject.name == "Terminal")
            {
                if (collision.name == "Player_Character")
                {
                    collision.GetComponent<PlayerMovement>().frozen = true;
                    mainCamera.GetComponent<Camera>().orthographicSize = 15;
                    controled.SetActive(true);
                }
                /*
                for (int a = 0; a < controled.Length; a++)
                {
                    if (controled[a].gameObject.name == "View")
                        controled[a].GetComponent<SecurityCamera>().onOff = false;
                    else if (controled[a].gameObject.name == "Light")
                    {
                        controled[a].GetComponent<LightControl>().onOff = false;
                    }
                }
                */
            }
            else if (gameObject.name == "EndTerminal")
            {
                if (collision.name== "Player_Character"&& collision.GetComponent<PlayerMovement>().hasUSB == true)
                {
                    //Show mission complete on screen and press any key to restart like game over
                    gameWonPanel.SetActive(true);
                }
                else if(collision.name == "Player_Character" && collision.GetComponent<PlayerMovement>().hasUSB == false)
                {
                    gameObject.GetComponent<AudioSource>().clip = sounds[0];
                    gameObject.GetComponent<AudioSource>().Play();
                    needUsbPanel.SetActive(true);
                    //Show message on screen that we need to find USB
                }
            }
        }
        if (collision.name == "Player_Character"&&Input.GetKeyDown(KeyCode.DownArrow))
        {
            controled.SetActive(false);
            mainCamera.GetComponent<Camera>().orthographicSize = 5;
        }
    }
}
