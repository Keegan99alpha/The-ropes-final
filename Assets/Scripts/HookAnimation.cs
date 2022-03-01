using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookAnimation : MonoBehaviour
{
    private float increaseSpeed=0.0475f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<Animator>().isActiveAndEnabled == true)
        {
            transform.GetComponent<BoxCollider2D>().offset += new Vector2(increaseSpeed, 0);
        }
        else
        {
            transform.GetComponent<BoxCollider2D>().offset = new Vector2(0.62f, 0);
        }
        if (transform.GetComponent<BoxCollider2D>().offset.x > 3.24f)
        {
            transform.GetComponent<BoxCollider2D>().offset = new Vector2(0.62f, 0);
            gameObject.SetActive(false);

        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Light")
        {
            transform.GetComponent<BoxCollider2D>().offset = new Vector2(0.62f, 0);
            transform.parent.parent.GetComponent<PlayerMovement>().hookedLight();
            gameObject.SetActive(false);
        }
        if (collision.transform.tag == "Ground")
        {
            transform.GetComponent<BoxCollider2D>().offset = new Vector2(0.62f, 0);
            gameObject.SetActive(false);
        }
    }
}
