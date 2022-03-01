using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class DoorControl : MonoBehaviour
{
    public TileBase doorUpper;
    public TileBase doorLower;
    public TileBase doorOpen;
    public TileBase placeHolder;
    private bool Opening;
    public Tilemap tilemap;
    public bool NormalDoor;
    Vector3Int upperDoorCoord;
    Vector3Int lowerDoorCoord;
    public int x, y1, y2, z;

    // Start is called before the first frame update
    void Start()
    {

        if (NormalDoor == true)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("UNLOCKED_DOOR");
        }
        Opening = false;
        upperDoorCoord = new Vector3Int(x, y1, z);
        lowerDoorCoord = new Vector3Int(x, y2, z);
    }

    // Update is called once per frame
    void Update()
    {
        if (tilemap.GetTile(upperDoorCoord) == doorOpen)
        {
            Opening = true;
        }
        else
        {
            Opening = false;
        }
            if (NormalDoor == false)
        {
            if(Opening == false)
            {
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ID_DOOR");

            }
            else
            {
                transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("PASS_DOOR");

            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Player_Character")
        {
            if (NormalDoor == true)
            {
                tilemap.SetTile(upperDoorCoord, doorOpen);
                tilemap.SetTile(lowerDoorCoord, placeHolder);
            }
        }

        //print("colliding");
        if (col.gameObject.tag == "Guard" || col.gameObject.tag == "Commander")
        {
            if (tilemap.GetTile(upperDoorCoord) == doorOpen)
            {
                tilemap.SetTile(upperDoorCoord, doorUpper);
                tilemap.SetTile(lowerDoorCoord, doorLower);
            }
            else
            {
                tilemap.SetTile(upperDoorCoord, doorOpen);
                tilemap.SetTile(lowerDoorCoord, placeHolder);
            }
        }
    }
     void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name== "Player_Character" && Input.GetKeyDown(KeyCode.UpArrow)&&collision.GetComponent<PlayerMovement>().hasKeycard==true)
        {

            if (tilemap.GetTile(upperDoorCoord) == doorOpen)
            {
                tilemap.SetTile(upperDoorCoord, doorUpper);
                tilemap.SetTile(lowerDoorCoord, doorLower);
            }
            else
            {
                tilemap.SetTile(upperDoorCoord, doorOpen);
                tilemap.SetTile(lowerDoorCoord, placeHolder);
            }
        }
    }
}
