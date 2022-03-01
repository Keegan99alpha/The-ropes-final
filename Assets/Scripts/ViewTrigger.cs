using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewTrigger : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject player;
    GameObject Ui;
    UIControl uiControl;

    // Start is called before the first frame update
    void Start()
    {
        Ui = GameObject.Find("UIController");
        uiControl = Ui.GetComponent<UIControl>();
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverPanel.activeSelf == true)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            StartCoroutine(Proceed());
        }
    }

    IEnumerator Proceed()
    {
        yield return new WaitForSeconds(1);
        if (Input.anyKeyDown)
        {
            uiControl.Reload();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ExclamationMark");
            gameObject.GetComponentInParent<GuardMovement>().patrol = false;
            gameObject.GetComponentInParent<Rigidbody2D>().velocity = new Vector2(0, 0);
            gameOverPanel.SetActive(true);
        }
    }
}
