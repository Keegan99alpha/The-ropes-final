using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Controller2d))]
public class PlayerMovement : MonoBehaviour
{
    public GameObject trapProfab;
    public GameObject rope;
    public GameObject rock;
    public GameObject hook;
    public GameObject hookInScene;
    public GameObject trap;
    public GameObject rockPrefab;
    public GameObject tieRope;
    public GameObject hand;
    public Sprite[] playerAnimate;
    GameObject buttons;
    Transform inventory;
    private float flyingSpeedX;
    private float flyingSpeedY;
    Vector3 velocity;
    [SerializeField]
    public float timeToJumpApex = 0.35f;
    [SerializeField]
    public float maxJumpHeight = 2.5f;
    [SerializeField]
    public float minJumpHeight = 0.5f;
    [SerializeField]
    private float playerSpeed = 5f;
    [SerializeField]
    private float accelTimeAir = 0.3f;
    [SerializeField]
    private float accelTimeGround = 0.2f;

    [SerializeField]
    private float wallSlideSpeedMax = 2.0f;
    [SerializeField]
    private Vector2 wallJumpUp;
    [SerializeField]
    private Vector2 wallJumpOff;
    [SerializeField]
    private Vector2 WallJumpAcross;
    [SerializeField]
    private float wallClimbStickTime = 0.35f;
    private float wallClimbDeStickTime;
    public bool hasUSB;
    private bool secondJump = false;

    Controller2d controller;

    private Rigidbody2D playerbody;

    private float maxJumpSpeed;
    private float minJumpSpeed;
    float gravity;
    float horizontalSmooth;
    public int equipingSlot;
    public bool frozen;
    public GameObject KeycardBlue;
    GameObject keycard;
    public bool hasKeycard = false;
    private bool hooking = false;
    private bool hookAnimation = false;
    private Vector2 lightPosition = new Vector2();
    private double animateTimer;
    public RuntimeAnimatorController[] animators;
    void Start()
    {
        frozen = false;
        equipingSlot = 0;
        buttons = GameObject.Find("Buttons");
        inventory = transform.GetChild(0);
        controller = GetComponent<Controller2d>();
        playerbody = gameObject.GetComponent<Rigidbody2D>();
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpSpeed = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpSpeed = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        hasUSB = false;
        transform.GetComponent<Animator>().enabled = false;
    }

    public void hookedLight()
    {
        hook.GetComponent<Item>().count--;
        hooking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (frozen == false)
        {
            tieRope.transform.position = tieRope.transform.parent.transform.position;
            if (velocity.x > 0.1)
            {
                tieRope.GetComponent<BoxCollider2D>().offset = new Vector2(-0.2f, tieRope.GetComponent<BoxCollider2D>().offset.y);
                hand.transform.position = new Vector2(transform.position.x + 0.5f, hand.transform.position.y);
                transform.GetComponent<Animator>().runtimeAnimatorController = animators[0];
                transform.GetComponent<Animator>().enabled = true;
            }
            else if (velocity.x > 0 && velocity.x < 0.5)
            {
                tieRope.GetComponent<BoxCollider2D>().offset = new Vector2(-0.2f, tieRope.GetComponent<BoxCollider2D>().offset.y);
                hand.transform.position = new Vector2(transform.position.x + 0.5f, hand.transform.position.y);
                transform.GetComponent<SpriteRenderer>().sprite = playerAnimate[0];
                transform.GetComponent<Animator>().enabled = false;
            }
            else if (velocity.x < -0.1)
            {
                tieRope.GetComponent<BoxCollider2D>().offset = new Vector2(0.2f, tieRope.GetComponent<BoxCollider2D>().offset.y);
                hand.transform.position = new Vector2(transform.position.x - 0.5f, hand.transform.position.y);
                transform.GetComponent<Animator>().runtimeAnimatorController = animators[1];
                transform.GetComponent<Animator>().enabled = true;

            }
            else if (velocity.x < 0 && velocity.x > -0.5)
            {
                tieRope.GetComponent<BoxCollider2D>().offset = new Vector2(0.2f, tieRope.GetComponent<BoxCollider2D>().offset.y);
                hand.transform.position = new Vector2(transform.position.x - 0.5f, hand.transform.position.y);
                transform.GetComponent<SpriteRenderer>().sprite = playerAnimate[1];
                transform.GetComponent<Animator>().enabled = false;
            }

            if (hooking == true)
            {
                float dx = lightPosition.x - transform.position.x;
                float dy = lightPosition.y - transform.position.y;
                flyingSpeedX = dx / 10.0f;
                flyingSpeedY = dy / 10.0f;
                gravity = 0;
                velocity.y = 0;
                transform.position = new Vector2(transform.position.x + flyingSpeedX, transform.position.y + flyingSpeedY);
                if (lightPosition.y-0.05f <= transform.position.y)
                {
                   hookInScene.GetComponent<Animator>().enabled = false;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    hooking = false;
                    gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
                }
            }

            if (Input.GetMouseButtonDown(0) && equipingSlot == 2 && hook.GetComponent<Item>().count > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    print(hit.collider.name);
                    if (hit.collider.gameObject.name == "Light")
                    {
                        lightPosition = hit.collider.gameObject.transform.GetChild(0).position;
                        if (lightPosition.y > transform.position.y)
                        {
                            if ((velocity.x <= 0 && lightPosition.x < transform.position.x) || (velocity.x >= 0 && lightPosition.x > transform.position.x))
                            {
                                hookInScene.SetActive(true);
                                hookInScene.transform.position = hand.transform.position;
                                float dx = lightPosition.x - transform.position.x;
                                float dy = lightPosition.y - transform.position.y;
                                float angle;
                                if (transform.position.x < lightPosition.x)
                                {
                                    angle = Mathf.Atan(dy / dx);
                                }
                                else
                                {
                                    print(Mathf.Atan(dy / dx));
                                    angle = 3.14f + Mathf.Atan(dy / dx);
                                }
                                hookInScene.transform.rotation = new Quaternion(0, 0, angle, 1);
                                hookAnimation = true;
                                hookInScene.GetComponent<Animator>().enabled = true;
                            }
                        }

                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (rope.GetComponent<Item>().count > 0)
                {
                    equipingSlot = 0;

                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (rock.GetComponent<Item>().count > 0)
                {
                    equipingSlot = 1;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (hook.GetComponent<Item>().count > 0)
                {
                    equipingSlot = 2;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (trap.GetComponent<Item>().count > 0)
                {
                    equipingSlot = 3;
                }
            }




            if (Input.GetKeyDown(KeyCode.Z) && equipingSlot == 1 && rock.GetComponent<Item>().count > 0)
            {
                GameObject throwingRock;
                throwingRock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
                throwingRock.name = "ThrowingRock";
                throwingRock.transform.position = hand.transform.position;
                if (velocity.x > 0)
                    throwingRock.GetComponent<Rigidbody2D>().AddForce(new Vector2(400.0f, 0));
                else
                    throwingRock.GetComponent<Rigidbody2D>().AddForce(new Vector2(-400.0f, 0));
                rock.GetComponent<Item>().count--;
            }
            if (Input.GetKeyDown(KeyCode.Z) && equipingSlot == 0 && rope.GetComponent<Item>().count > 0)
            {
                tieRope.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.Z) && tieRope.activeInHierarchy==true)
            {
                tieRope.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Z) && equipingSlot == 3 && trap.GetComponent<Item>().count > 0)
            {
                GameObject trapInScene;
                trapInScene = Instantiate(trapProfab, transform.position, Quaternion.identity);
                trapInScene.name = "TrapInScene";
                trapInScene.transform.position = new Vector3(trapInScene.transform.position.x, trapInScene.transform.position.y - 0.45f,1.0f);
                trap.GetComponent<Item>().count--;
            }


            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            int wallDirX = (controller.collisions.left) ? -1 : 1;

            float targetVelocityX = input.x * playerSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref horizontalSmooth, (controller.collisions.below) ? accelTimeGround : accelTimeAir);

            bool wallSliding = false;
            if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0 && !controller.collisions.internalHit)
            {
                wallSliding = true;
                if (velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }

                if (wallClimbDeStickTime > 0)
                {
                    horizontalSmooth = 0;
                    velocity.x = 0;
                    if (input.x != wallDirX && input.x != 0)
                    {
                        wallClimbDeStickTime -= Time.deltaTime;
                    }
                    else
                    {
                        wallClimbDeStickTime = wallClimbStickTime;
                    }
                }
                else
                {
                    wallClimbDeStickTime = wallClimbStickTime;
                }
            }




            if (Input.GetKeyDown(KeyCode.W))
            {
                if (wallSliding)
                {
                    if (wallDirX == input.x)
                    {
                        velocity.x = -wallDirX * wallJumpUp.x;
                        velocity.y = wallJumpUp.y;
                    }
                    else if (input.x == 0)
                    {
                        velocity.x = -wallDirX * wallJumpOff.x;
                        velocity.y = wallJumpOff.y;
                    }
                    else
                    {
                        velocity.x = -wallDirX * WallJumpAcross.x;
                        velocity.y = WallJumpAcross.y;
                    }
                }

                if (controller.collisions.below)
                {
                    velocity.y = maxJumpSpeed;
                    secondJump = true;
                }
                /*else if (secondJump)
                {
                    velocity.y = jumpSpeed;
                    secondJump = false;
                }*/
            }
            if ((Input.GetKeyUp(KeyCode.W)))
            {
                if (velocity.y > minJumpSpeed)
                {
                    velocity.y = minJumpSpeed;
                }
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime, input);

            if (controller.collisions.above || controller.collisions.below)
            {
                velocity.y = 0;
            }
        }
        else
        {
            transform.GetComponent<Animator>().enabled = false;
            transform.GetComponent<SpriteRenderer>().sprite = playerAnimate[2];
            if (Input.GetKeyDown(KeyCode.DownArrow)){
                frozen = false;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name=="Keycard")
        {
            hasKeycard = true;
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Items")
        {
            switch (col.gameObject.name)
            {
                case "PickableRope":
                    rope.GetComponent<Item>().addItem();
                    break;
                case "PickableRock":
                    rock.GetComponent<Item>().addItem();

                    break;
                case "PickableHook":
                    hook.GetComponent<Item>().addItem();
                    break;
                case "PickableTrap":
                    trap.GetComponent<Item>().addItem();
                    break;
                case "USB":
                    hasUSB = true;
                    break;
                default:
                    break;
            }
            Destroy(col.gameObject);

        }
    }
}
