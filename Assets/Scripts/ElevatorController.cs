using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public GameObject upperLift;
    public GameObject downLift;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player_Character")
        {
            if (collision.GetComponent<PlayerMovement>().hasKeycard == true)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)&&upperLift!=null)
                {
                    collision.transform.position = new Vector2(upperLift.transform.position.x, upperLift.transform.position.y + 0.8f);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow)&&downLift!=null)
                {
                    collision.transform.position = new Vector2(downLift.transform.position.x, downLift.transform.position.y + 0.8f);
                }
            }
        }
    }
}
