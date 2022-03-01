using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : MonoBehaviour
{
    public GameObject targetStairUp;
    public GameObject targetStairDown;
    public bool playerIn;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        //玩家相关部分，如果按上箭头并且可以上楼的话，将玩家移动到楼上（同理下箭头）

        if (playerIn)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && targetStairUp != null)
            {
                player.transform.position = new Vector3(targetStairUp.transform.position.x, targetStairUp.transform.position.y + 0.6f, player.transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) && targetStairDown != null)
            {
                player.transform.position = new Vector3(targetStairDown.transform.position.x, targetStairDown.transform.position.y + 0.6f, player.transform.position.z);
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player_Character")
        {
            playerIn = true;
            player = collision.gameObject;
        }


            //保安碰撞部分。如果楼梯检测到保安碰撞。
        if (collision.gameObject.tag == "Guard" || collision.gameObject.tag == "Commander")
        {
            //确认保安状态是否为hunting模式
            if (collision.GetComponent<GuardMovement>().hunting == true)
            {
                //如果保安在警报的左侧，那么在换层后保安偏移1.0f
                //If guard on the left side of alert. (Offset after changed floor is 1.0f to avoid coillding with stair)
                if (collision.transform.position.x < collision.gameObject.GetComponent<GuardMovement>().alertPosition.x)
                {
                    //如果保安在高层，那么使用下台阶
                    //If guard on the high level. Goes to down stair.
                    if (collision.transform.position.y > collision.gameObject.GetComponent<GuardMovement>().alertPosition.y)
                    {
                        collision.transform.position = new Vector2(targetStairDown.transform.position.x + 1.0f, targetStairDown.transform.position.y);
                    }
                    //如果保安在低层，意味着警报可能在保安上层，或者和保安同层
                    //Else means alert could be in upper level or same level of guard.
                    else if (collision.transform.position.y < collision.gameObject.GetComponent<GuardMovement>().alertPosition.y)
                    {
                        //如果保安的高度增加2.0f后仍然小于警报高度。说明警报在保安的上层，将保安移动到上层，否则说明警报和保安在同层。不移动保安
                        //After added 2.0f to guard's y axis and if alert still higher than guard which means alert located in upper level, move guard to upper level. Else means guard
                        //and alert are in same level. Not move guard.
                        if (collision.transform.position.y + 2.0f < collision.gameObject.GetComponent<GuardMovement>().alertPosition.y){
                            collision.transform.position = new Vector2(targetStairUp.transform.position.x + 1.0f, targetStairUp.transform.position.y);
                        }
                    }
                }
                //如果保安在警报的右侧，那么偏移量为-1.0f。
                //If guard on the right side of alert. (Offset after changed floor is -1.0f)
                else
                {
                    print(collision.gameObject.GetComponent<GuardMovement>().alertPosition.y);
                    //高度检查，如果保安在高层
                    //High check, if guard in high level
                    if (collision.transform.position.y > collision.gameObject.GetComponent<GuardMovement>().alertPosition.y)
                    {
                        collision.transform.position = new Vector2(targetStairDown.transform.position.x - 1.0f, targetStairDown.transform.position.y);
                    }
                    //如果保安在低层或者同层
                    //if guard in lower level or same level
                    else if (collision.transform.position.y < collision.gameObject.GetComponent<GuardMovement>().alertPosition.y)
                    {
                        if (collision.transform.position.y + 2.0f < collision.gameObject.GetComponent<GuardMovement>().alertPosition.y)
                        {
                            collision.transform.position = new Vector2(targetStairUp.transform.position.x - 1.0f, targetStairUp.transform.position.y);
                        }
                    }
                }
            }//End of Hunting mode check
            //如果保安在巡逻模式
            //If guard in patrol mode
            else if (collision.GetComponent<GuardMovement>().patrol == true)
            {
                if(collision.GetComponent<GuardMovement>().partolPoints[0].transform.position.y==transform.position.y && collision.GetComponent<GuardMovement>().partolPoints[1].transform.position.y == transform.position.y)
                {

                }
               else if (collision.transform.position.x < collision.gameObject.GetComponent<GuardMovement>().partolPoints[0].transform.position.x)
                {
                    if (collision.transform.position.y > collision.gameObject.GetComponent<GuardMovement>().partolPoints[0].transform.position.y)
                    {
                        collision.transform.position = new Vector2(targetStairDown.transform.position.x - 1.0f, targetStairDown.transform.position.y);
                    }
                    else if (collision.transform.position.y < collision.gameObject.GetComponent<GuardMovement>().partolPoints[0].transform.position.y)
                    {
                        collision.transform.position = new Vector2(targetStairUp.transform.position.x - 1.0f, targetStairUp.transform.position.y);
                    }
                }
                else
                {
                    if (collision.transform.position.y > collision.gameObject.GetComponent<GuardMovement>().partolPoints[0].transform.position.y)
                    {
                        collision.transform.position = new Vector2(targetStairDown.transform.position.x - 1.0f, targetStairDown.transform.position.y);
                    }
                    else if (collision.transform.position.y < collision.gameObject.GetComponent<GuardMovement>().partolPoints[0].transform.position.y   )
                    {
                        collision.transform.position = new Vector2(targetStairUp.transform.position.x - 1.0f, targetStairUp.transform.position.y);
                    }
                }
            }
        }
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player_Character")
        {
            playerIn = false;
            player = null;
        }
    }

}
