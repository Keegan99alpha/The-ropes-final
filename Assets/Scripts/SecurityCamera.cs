using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    public bool onOff;
    bool triggering;
    // Start is called before the first frame update
    void Start()
    {
        onOff = true;
        triggering = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (onOff == false)
        {
            transform.parent.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("CameraOff");
            transform.GetComponent<SpriteRenderer>().enabled = false;
            transform.GetComponentInChildren<AudioSource>().enabled = false;

        }
        if (onOff == true && triggering == false)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ViewOn");
        }
    }
    public void onClick()
    {
        onOff = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && onOff == true)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ViewActive");
            triggering = true;
            transform.GetComponentInChildren<BoxCollider2D>().enabled = true;
            transform.GetComponentInChildren<AudioSource>().enabled = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player"&&onOff==true)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ViewOn");
            triggering = false;
            transform.GetComponentInChildren<BoxCollider2D>().enabled = false;
           // transform.GetComponentInChildren<AudioSource>().enabled = false;

        }
    }
}
