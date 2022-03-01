using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    public Color dark;
    public Color light;
    public Color equiping;
    public GameObject rope;
    public GameObject rock;
    public GameObject hook;
    public GameObject trap;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (rope.GetComponent<Item>().count == 0)
        {
            transform.GetChild(0).GetComponent<Image>().color = dark;
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "0";
        }
        else if (player.GetComponent<PlayerMovement>().equipingSlot == 0)
        {
            transform.GetChild(0).GetComponent<Image>().color = equiping;
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = rope.GetComponent<Item>().count.ToString();
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().color = light;
            transform.GetChild(0).GetChild(0).GetComponent<Text>().text = rope.GetComponent<Item>().count.ToString();

        }
        if (rock.GetComponent<Item>().count == 0)
        {
            transform.GetChild(1).GetComponent<Image>().color = dark;
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "0";

        }
        else if (player.GetComponent<PlayerMovement>().equipingSlot == 1)
        {
            transform.GetChild(1).GetComponent<Image>().color = equiping;
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = rock.GetComponent<Item>().count.ToString();
        }
        else
        {
            transform.GetChild(1).GetComponent<Image>().color = light;
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = rock.GetComponent<Item>().count.ToString();

        }
        if (hook.GetComponent<Item>().count == 0)
        {
            transform.GetChild(2).GetComponent<Image>().color = dark;
            transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "0";

        }
        else if (player.GetComponent<PlayerMovement>().equipingSlot == 2)
        {
            transform.GetChild(2).GetComponent<Image>().color = equiping;
            transform.GetChild(2).GetChild(0).GetComponent<Text>().text = hook.GetComponent<Item>().count.ToString();
        }
        else
        {
            transform.GetChild(2).GetComponent<Image>().color = light;
            transform.GetChild(2).GetChild(0).GetComponent<Text>().text = hook.GetComponent<Item>().count.ToString();

        }

        if (trap.GetComponent<Item>().count == 0)
        {
            transform.GetChild(3).GetComponent<Image>().color = dark;
            transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "0";

        }
        else if (player.GetComponent<PlayerMovement>().equipingSlot == 3)
        {
            transform.GetChild(3).GetComponent<Image>().color = equiping;
            transform.GetChild(3).GetChild(0).GetComponent<Text>().text = trap.GetComponent<Item>().count.ToString();
        }
        else
        {
            transform.GetChild(3).GetComponent<Image>().color = light;
            transform.GetChild(3).GetChild(0).GetComponent<Text>().text = trap.GetComponent<Item>().count.ToString();

        }


        if (player.GetComponent<PlayerMovement>().hasKeycard == true)
        {
            transform.GetChild(4).GetComponent<Image>().color = light;
        }
        else
        {
            transform.GetChild(4).GetComponent<Image>().color = dark;
        }
        if (player.GetComponent<PlayerMovement>().hasUSB == true)
        {
            transform.GetChild(5).GetComponent<Image>().color = light;
        }
        else
        {
            transform.GetChild(5).GetComponent<Image>().color = dark;
        }
    }
}
