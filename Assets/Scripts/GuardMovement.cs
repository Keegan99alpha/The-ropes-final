using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour
{
    public RuntimeAnimatorController[] animators;
    public GameObject handLight;
    private GameObject alertCamera;
    private double animateTimer;
    Vector2 left;
    Vector2 right;
    float speed;
    Rigidbody2D rigid;
    public GameObject[] partolPoints;
    bool arrived;
    bool reset;
    public Sprite[] sprites;
    public GameObject view;
    public GameObject keycardBlue;
    public bool stun;
    private bool tieUp;
    private bool inDark;
    public bool patrol;
    public bool waiting;
    public bool hunting;
    private int keycardCount;
    private float stunTime;
    public Vector2 alertPosition;
    Vector3 ViewOffsetL;
    Vector3 ViewOffsetR;
    // Start is called before the first frame update
    void Start()
    {
        ViewOffsetL = new Vector3(-0.26f, 0.43f, 0);
        ViewOffsetR = new Vector3(0.26f, 0.43f, 0);
        stun = false;
        tieUp = false;
        reset = true;
        arrived = true;
        inDark = false;
        hunting = false;
        left = new Vector2(-1.0f, 0);
        right = new Vector2(1.0f, 0);
        speed = 0.8f;
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = speed * left;
        stunTime = 0;
        if (transform.tag == "Commander")
        {
            keycardCount = 1;
        }
    }
    void huntingAction()
    {
        speed = 2.0f;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("QuestionMark");
        if (transform.position.x < alertPosition.x)
        {
            rigid.velocity = speed * right;
        }
        else
        {
            rigid.velocity = speed * left;
        }
        if (transform.position.x > (alertPosition.x - 0.5f) && transform.position.x < (alertPosition.x + 0.5f))
        {
            hunting = false;
            print("end of hunting");
            patrol = true;
            alertCamera.GetComponent<AudioSource>().enabled = false;
        }

    }

    void patrolAction()
    {
        speed = 0.8f;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;

        if (reset == false)
        {
            if (transform.position.x < partolPoints[0].transform.position.x)
            {
                rigid.velocity = speed * right;
            }
            else
            {
                rigid.velocity = speed * left;

            }
        }
        else
        {
            if (transform.position.x > partolPoints[0].transform.position.x && arrived == true)
            {
                rigid.velocity = speed * left;
            }
            else if (transform.position.x < partolPoints[1].transform.position.x && arrived == false)
            {
                rigid.velocity = speed * right;
            }
        }

        if (transform.position.x >= partolPoints[1].transform.position.x)
        {
            arrived = true;
        }
        else if (transform.position.x <= partolPoints[0].transform.position.x)
        {
            arrived = false;

        }
    }

    void walkingAnimation()
    {
        if (rigid.velocity.x > 0)
        {
                transform.GetComponent<Animator>().runtimeAnimatorController = animators[0];
                transform.GetComponent<Animator>().enabled = true;
         }
        else if (rigid.velocity.x < 0)
        {
                transform.GetComponent<Animator>().runtimeAnimatorController = animators[1];
                transform.GetComponent<Animator>().enabled = true;
        }
    }

    public void tieUpGuard()
    {
        transform.GetComponent<Animator>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        patrol = false;
        hunting = false;
        stun = false;
        tieUp = true;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f);
    }
    // Update is called once per frame
    void Update()
    {
        if (tieUp != true && stun != true)
        {
            if (waiting == false)
            {
                if (rigid.velocity.x > 0)
                {
                    walkingAnimation();
                    view.transform.position = gameObject.transform.position + ViewOffsetR;
                    if (inDark == false)
                        view.transform.GetComponentInChildren<BoxCollider2D>().offset = new Vector2(1.9f, -0.7f);
                }
                else
                {
                    walkingAnimation();
                    view.transform.position = gameObject.transform.position + ViewOffsetL;
                    if (inDark == false)
                        view.transform.GetComponentInChildren<BoxCollider2D>().offset = new Vector2(-1.9f, -0.7f);
                }
            }
            else
            {
                rigid.velocity = new Vector2(0, 0) ;
            }

        }
        if (patrol == true)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            patrolAction();
        }
        if (hunting == true)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            huntingAction();
        }

        if (stun == true&&tieUp!=true)
        {
            transform.GetComponent<Animator>().enabled = false;

            if (rigid.velocity.x < 0)
                gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<GuardMovement>().sprites[3];
            else
                gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<GuardMovement>().sprites[2];
            rigid.velocity = new Vector2(0, 0);
            view.SetActive(false);
            rigid.gravityScale = 0;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.GetComponent<BoxCollider2D>().isTrigger = true;
            stunTime += Time.deltaTime;
            if (stunTime > 4.0f)
            {
                stun = false;
                if (waiting == true)
                {

                }
                else
                {
                    patrol = true;
                }
                transform.GetComponent<BoxCollider2D>().isTrigger = false;
                rigid.gravityScale = 1;
                view.SetActive(true);
                stunTime = 0;
            }
            if (transform.tag == "Commander")
            {
                if (keycardCount != 0)
                {
                    GameObject keycard;
                    keycard = Instantiate(keycardBlue, transform.position, Quaternion.identity);
                    keycard.name = "Keycard";
                    keycard.tag = "Items";
                    keycardCount--;
                }
            }
        }
        if (tieUp == true)
        {
            transform.GetComponent<Animator>().enabled = false;

            if (transform.tag == "Commander")
            {
                if (keycardCount != 0)
                {
                    GameObject keycard;
                    keycard = Instantiate(keycardBlue, transform.position, Quaternion.identity);
                    keycard.name = "Keycard";
                    keycard.tag = "Items";
                    keycardCount--;
                }
            }
            transform.rotation = new Quaternion(0, 0, 0, 0);
            gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<GuardMovement>().sprites[4];
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Controller2d>().enabled = false;
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rigid.rotation = 0;
        }
        if (collision.gameObject.tag == "KnockDownItem")
        {
            patrol = false;
            stun = true;
        }
        if (stun == false)
        {
            if (collision.gameObject.tag == "Items")
            {
                if (collision.gameObject.name == "Keycard")
                {
                    keycardCount++;
                    Destroy(collision.gameObject);
                    speed = 0.8f;
                    if (rigid.velocity.x < 0)
                    {
                        rigid.velocity = speed * left;
                    }
                    else
                    {
                        rigid.velocity = speed * right;
                    }
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "TieRope"&&stun==true)
        {
            stun = false;
            tieUp = true;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.4f);
        }
        if (collision.gameObject.tag == "Alert" && stun != true)
        {
            print("Found");
            patrol = false;
            hunting = true;
            alertCamera = collision.gameObject;
            alertPosition = collision.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Darkness")
        {
            inDark = true;
            view.GetComponent<BoxCollider2D>().size = new Vector2(1.9f, view.GetComponent<BoxCollider2D>().size.y);

            handLight.SetActive(true);
            print(rigid.velocity.x);
            if (rigid.velocity.x > 0)
            {
                handLight.transform.rotation = new Quaternion(0, 180, 0, 0);
                view.GetComponent<BoxCollider2D>().offset = new Vector2(0.9f, view.GetComponent<BoxCollider2D>().offset.y);

            }
            else
            {
                handLight.transform.rotation = new Quaternion(0, 0, 0, 0);
                view.GetComponent<BoxCollider2D>().offset = new Vector2(-0.9f, view.GetComponent<BoxCollider2D>().offset.y);

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Darkness")
        {
            handLight.SetActive(false);
        }
    }
}
