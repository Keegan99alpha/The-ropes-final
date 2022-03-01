using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    public bool onOff;
    public GameObject darkness;
    public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        onOff = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (onOff == true)
        {
            transform.GetComponent<SpriteRenderer>().sprite = sprites[0];
            darkness.SetActive(false);
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = sprites[1];
            darkness.SetActive(true);
        }
    }

    public void onClick()
    {
        onOff = false;
    }
}
