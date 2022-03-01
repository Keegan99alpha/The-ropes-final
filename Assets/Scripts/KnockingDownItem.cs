using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockingDownItem : MonoBehaviour
{
    public GameObject rope;
    public GameObject hand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (gameObject.name == "ThrowingRock" && collision.transform.name!="Player_Character")
        {
            print(collision.transform.name);
            Destroy(gameObject);
        }
       

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (gameObject.name == "TieRope")
         {
            if (collision.gameObject.tag == "Guard" || collision.gameObject.tag == "Commander")
            {
                if(collision.GetComponent<GuardMovement>().stun==true)
                gameObject.SetActive(false);
                rope.GetComponent<Item>().count--;
            }
        }
        if (gameObject.name == "TrapInScene")
        {
            if (collision.gameObject.tag == "Guard" || collision.gameObject.tag == "Commander")
            {
                collision.GetComponent<GuardMovement>().tieUpGuard();
                Destroy(gameObject);
            }
        }
    }
}
