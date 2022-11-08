using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementCC : MonoBehaviour
{
    //Variables for how the player starts moving, stops moving, and how fast they can go.
    [Header("Movement")]
    [SerializeField]
    float movementAcceleration = 5;
    [SerializeField]
    float maxSpeedX = 5;
    [SerializeField]
    float stopTime = 0.2f;
    //Variables for amount of jumps and how big the jump is.
    [Header("Jumping")]
    [SerializeField]
    float jumpForce = 10;
    [SerializeField]
    int jumpMax = 3; 

    //sets strength of gravity
    [Header("Other")]
    [SerializeField]
    float gravityAcceleration = -9.81f;
    //sets key for reverse gravity
    [SerializeField]
    KeyCode reverseGravityButton = KeyCode.R;

    //variables for the player state
    private int jumpCount = 0;
    private Vector2 currentVelocity = new Vector2();
    private Vector2 currentAcceleration = new Vector2();
    private float forwardRotation = 0;
    private float backwardRotation = 180;
    private float uprightRotation = 0;
    private float defaultGroundedGravity = -1f;
    private bool isReversed = false;

    //character controller reference
    private CharacterController cc;
    private GroundCheck gc;
    private HeadCheck hc;

    private void Start() 
    {
        cc = GetComponent<CharacterController>();
        gc = GetComponentInChildren<GroundCheck>();
        if (!gc)
        {
            Debug.LogError("No Groundcheck detected");
        }
        hc = GetComponentInChildren<HeadCheck>();
        if (!hc)
        {
            Debug.LogError("No Headcheck detected");
        }
    }
    private void Update()
    {
        print(gc.IsGrounded);
        if (Input.GetKeyDown(reverseGravityButton)) 
        {
            ReverseGravity();
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput < 0)
        {
            transform.eulerAngles = new Vector3(0, backwardRotation, uprightRotation);
        }
        else 
        {
            transform.eulerAngles = new Vector3(0, forwardRotation, uprightRotation);
        }
        //resets jumpcount when player touches ground
        if (gc.IsGrounded)
        {
            ResetJumps();
        }

        //sets acceleration based on gravity and input
        currentAcceleration = new Vector2(horizontalInput * movementAcceleration, gravityAcceleration);
       
        //Sets gravity to 0 if the player is grounded, this prevents the player from falling super fast if they are on a platform
        if (gc.IsGrounded) currentAcceleration.y = -1;

        //Adds acceleration to the velocity so the player increases or decreases speed over time.
        //Then it clamps the speed to confine it within the max speed.
        currentVelocity += currentAcceleration * Time.deltaTime;
        currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxSpeedX, maxSpeedX);

        //If there was no input, the player slows down. The speed in which the player scales on the stopTime variable
        if (horizontalInput == 0)
        {
            currentVelocity.x -= (currentVelocity.x / stopTime) * Time.deltaTime;
            float previousVelocity = currentVelocity.x;
            currentVelocity.x -= (currentVelocity.x / stopTime) * Time.deltaTime;

            //checks if the velocity changed from positive to negative or vice versa and sets velocity to 0 if so, this prevents the player from continuously shaking
            //back and forth
            if ((previousVelocity < 0 && currentVelocity.x > 0) || (previousVelocity > 0 && currentVelocity.x < 0))
            {
                currentVelocity.x = 0;
            } 
        }


        //If the player presses the jump button, the player jumps
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < jumpMax)
        {
            Jump();
        }

        //Checks if player collides with anything while moving and stops acceleration and velocity if there was a collision.
        //Note: This works because the CharacterController only looks for collisions when moving and only in the direction it is headed towards.
        CheckPhysicsCollision();

        //CharacterController built-in function that is used to move characters. It moves the character after checking all situations
        //and making modifications accordingly
        cc.Move(currentVelocity * Time.deltaTime);

    }

    private void CheckPhysicsCollision()
    {
        // Checks if there was a collision on the sides of the player.

        /*     
        Due to how CharacterController collision works, it only checks the side that it is moving towards. So if the character is
        moving forward, the collision will only check in front of it.
        */
        if ((cc.collisionFlags & CollisionFlags.Sides) != 0)
        {
            //If there is an object that collided, the player shouldn't be accelerating and shouldn't be moving at all.
            currentAcceleration.x = 0;
            currentVelocity.x = 0;
        }
        print("Head Touched: " + hc.IsHeadTouched);
        print ("Velocity toward head more than 0: " + (isReversed ? currentVelocity.y < 0 : currentVelocity.y > 0));
        if (hc.IsHeadTouched && (isReversed ? currentVelocity.y < 0 : currentVelocity.y > 0))
        {
            //If the player jumps and hits a ceiling, the velocity resets to zero.
            //This is for preventing the player from floating because the velocity is still pushing them upwards.
            currentVelocity.y = 0;
        }
    }

    private void Jump() 
    {
        //Sets upward/downward velocity to 0 so that the jump will always have the same height.
        currentVelocity.y = 0f;

        //Adds the velocity so that the player jumps
        currentVelocity += new Vector2(0, jumpForce);

        //counts towards the max jumps
        jumpCount++;
    }
    private void ResetJumps()
    {
        jumpCount = 0;
    }

    //reverses all necessary variables, and changes rotation values, so that the player can successfully move upside down
    private void ReverseGravity()
    {
        isReversed = !isReversed;
        //currentVelocity.y = 0;
        gravityAcceleration = -gravityAcceleration;
        jumpForce = -jumpForce;
        defaultGroundedGravity = -defaultGroundedGravity;
        float x = forwardRotation;
        forwardRotation = backwardRotation;
        backwardRotation = x;
        if (uprightRotation == 0) uprightRotation = 180;
        else uprightRotation = 0;
    } 
}
