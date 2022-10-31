using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    //variables for movement and jumping
    public float speed = 5f;
    public float jumpSpeed = 8f;
    public static float direction = 0f;
    private Rigidbody rb;

    //variables used to control player jumping
    public static int jumpMax = 3; //how many jumps character can do before they have to land
    public static int jumpCount = 0; //how many jumps character has remaining

    public int extraJump = 0;

    //claw Attack variables
    public GameObject clawAttackObj;
    public float clawAtkTime = 1;

    //gets rigidbody values
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //gets a value that represents horizontal movement (usually -1, 0, or 1). Used to establish which way player is facing.
        direction = Input.GetAxis("Horizontal");
         
        
        //controls movement direction, velocity, and direction the player is facing
        if (direction > 0f)
        {
            rb.velocity = new Vector3(direction * speed, rb.velocity.y, 0.0f);
            transform.rotation = Quaternion.Euler(0,0,0);
        }
        else if (direction < 0f)
        {
            rb.velocity = new Vector3(direction * speed, rb.velocity.y, 0.0f);
            transform.rotation = Quaternion.Euler(0,180,0);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //Jumping. When space bar is pressed, checks if jumpCount < jumpMax or if player has extraJump from a pickup. If the player has an extra jump, this function will use
        //the extra jump up first before using the character's remaining jumpCount. 
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < jumpMax || extraJump > 0)
            {

                if (extraJump > 0)
                {
                    extraJump = extraJump - 1;
                }
                else
                {
                    jumpCount++;
                }

                rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, 0.0f);
            }
        }
        
        //"ctrl" button triggers Claw Attack through ClawAttack Coroutine below.
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ClawAttackCoroutine());
        }

    }

    //Checks for Player collisions with ExtraJump Pickups.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ExtraJump"))
        {
            extraJump++; //gives player an extra jump
            Destroy(other.gameObject); //destroys ExtraJump pickup.
        }
    }

    //Claw Attack. Sets ClawAttackObj GameObject, which is a child to Player GameObject, active for 1 sec. 
    //ClawAttackObj has a script that checks for targets and destroys them.
    IEnumerator ClawAttackCoroutine()
    {
        clawAttackObj.SetActive(true);
        yield return new WaitForSeconds(clawAtkTime);
        clawAttackObj.SetActive(false);
    }
}
