using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Normal,
        ShadowDive,
        Dead
    }

    public enum FloorType
    {
        None,
        Ground,
        LeftWall,
        RightWall,
        Ceiling
    }

    public PlayerState state = PlayerState.Normal;
    public float normalSpeed = 15f;
    public float shadowDiveSpeed = 15f;

    public float jumpForce = 10f;
    public LayerMask tileLayer;
    // public LayerMask lightLayer;
    // public LayerMask groundLayer; // Ensure this includes floating platforms

    public float shadowDiveScale = 0.9f;

    private bool isFacingRight;

    public Sprite normalSprite;

    public Sprite shadowDiveSprite;

    private Rigidbody2D rb;
    private Collider2D col;

    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool isWalled;

    // Used when manually setting position to prevent rigid body issues
    private float positionOffset = 0.01f;

    private FloorType feetOn;
    public Transform groundCheck; // Ground check position
    public Transform wallCheck; // Wall check position

    public Transform frontGroundCheck; // Ground check position
    public float checkRadius = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isFacingRight = true;
        feetOn = FloorType.Ground;
    }

    void Update()
    {
        switch (state)
        {
            case PlayerState.Normal:
                HandleNormalMovement();
                break;
            case PlayerState.ShadowDive:
                HandleShadowDiveMovement();
                break;
            case PlayerState.Dead:
                // Handle death state (e.g., restart level)
                break;
        }
    }

    void HandleNormalMovement()
    {
        //Move horizontal
        float move = Input.GetAxis("Horizontal");
        Vector2 targetVelocity = new Vector2(move * normalSpeed, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref targetVelocity, 0.0001f);


        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
        {
            SetStateShadowDive();
        }
        CheckFlip(move);
        CheckIfGrounded();
        CheckIfWalled();
    }

    void HandleShadowDiveMovement()
    {
        //Get desired move and update all checks before calculating what to do
        float move = Input.GetAxis("Horizontal");
        CheckFlip(move);
        CheckIfGrounded();
        CheckIfWalled();

        // Handle Landing 
        // So we are currently in the air (jumping) and we touch the floor or the wall
        if(feetOn == FloorType.None)
        {
            // need vertical velocity below some value so it doesnt get triggered immediately after jumping
            if(isGrounded && rb.velocity.y<=0)
            {
                updateFeetOn(FloorType.Ground);
            }

            // Same for jumping off walls because we rotate sprite to vertical, so make sure sprite is moving towards wall
            else if(isWalled)
            {
                if(isFacingRight && rb.velocity.x>=0)
                    updateFeetOn(FloorType.RightWall);
                else if(!isFacingRight && rb.velocity.x<=0)
                    updateFeetOn(FloorType.LeftWall);
            }
        }
        else // feet better be on some surface
        {
            // Is facing a corner
            if(isGrounded && isWalled)
            {
                if(feetOn == FloorType.Ground)
                {
                    if(isFacingRight)
                    {
                        GoToRightWall();
                    }
                    else
                    {
                        GoToLeftWall();
                    }
                }
                else if(feetOn == FloorType.RightWall)
                {
                    if(isFacingRight)
                    {
                        GoToCeiling();
                    }
                    else
                    {
                        GoToFloor();
                    }
                }
                else if(feetOn == FloorType.LeftWall)
                {
                    if(isFacingRight)
                    {
                        GoToFloor();
                    }
                    else
                    {
                        GoToCeiling();
                    }
                }
                else if(feetOn == FloorType.Ceiling)
                {
                    if(isFacingRight)
                    {
                        GoToLeftWall();
                    }
                    else
                    {
                        GoToRightWall();
                    }
                }
                
            }
            //Feet are on surface but is not grounded. It means its on an edge.
            else if(!isGrounded)
            {
                if(feetOn == FloorType.Ground)
                {
                    if(isFacingRight)
                    {
                        GoToLeftWall();
                    }
                    else
                    {
                        GoToRightWall();
                    }
                }
                else if(feetOn == FloorType.RightWall)
                {
                    if(isFacingRight)
                    {
                        GoToFloor();
                    }
                    else
                    {
                        GoToCeiling();
                    }
                }
                else if(feetOn == FloorType.LeftWall)
                {
                    if(isFacingRight)
                    {
                        GoToCeiling();
                    }
                    else
                    {
                        GoToFloor();
                    }
                }
                else if(feetOn == FloorType.Ceiling)
                {
                    if(isFacingRight)
                    {
                        GoToRightWall();
                    }
                    else
                    {
                        GoToLeftWall();
                    }
                }
            }
        }
        

        CheckFlip(move);
        CheckIfGrounded();
        CheckIfWalled();



        // All state transitions are handeled, now handle movement

        // Handle Jumping
        if (isGrounded && feetOn!= FloorType.None && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // TODO/Discuss: Slow movement in air
        if(feetOn == FloorType.None)
        {
            Vector2 targetVelocity = new Vector2(move * shadowDiveSpeed, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref targetVelocity, 0.02f);
        }
        else if(feetOn == FloorType.Ground)
        {
            Vector2 targetVelocity = new Vector2(move * shadowDiveSpeed, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref targetVelocity, 0.0001f);
        }
        else if(feetOn == FloorType.RightWall)
        {
            Vector2 targetVelocity = new Vector2(rb.velocity.x,move * shadowDiveSpeed);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref targetVelocity, 0.0001f);
        }
        else if(feetOn == FloorType.LeftWall)
        {
            Vector2 targetVelocity = new Vector2(rb.velocity.x,-move * shadowDiveSpeed);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref targetVelocity, 0.0001f);
        }
        else if(feetOn == FloorType.Ceiling)
        {
            //Discuss: Inverted movement on ceiling or not
            Vector2 targetVelocity = new Vector2(-move * shadowDiveSpeed, rb.velocity.y);
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref targetVelocity, 0.0001f);
        }
        


        // Transform back to normal state only on ground
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded && feetOn == FloorType.Ground)
        {
            SetStateNormal();
        }

        if(isGrounded)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 3;
        }

    }
    // On jumping
    // Set feetOn to None (which also sets sprite to vertical)
    // Jump perpendicular to current surface
    // Set direction in case of wall jumps
    void Jump()
    {
        if(feetOn == FloorType.Ground)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if(feetOn == FloorType.RightWall)
        {
            rb.velocity = new Vector2(-jumpForce, rb.velocity.y);
            //Discuss: Face player left on jumping from right wall and vice versa
            if(isFacingRight) Flip();
        }
        else if(feetOn == FloorType.LeftWall)
        {
            rb.velocity = new Vector2(jumpForce, rb.velocity.y);
            if(!isFacingRight) Flip();
        }
        updateFeetOn(FloorType.None);
    }
    void CheckFlip(float moveInput)
    {
        if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight))
        {
            Flip();
        }
    }
    
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void SetStateNormal()
    {
        state = PlayerState.Normal;
        rb.gravityScale = 3;

        spriteRenderer.sprite = normalSprite;
        transform.localScale = new Vector3(0.9f * (isFacingRight? 1: -1),1.9f,1);
        // translate vertically by difference in centers so normal state doesnt end up in ground
        transform.position += new Vector3(0, (1.9f-0.9f)/2, 0);
        if(!isFacingRight) Flip();


        // Additional transition logic (animations, effects)
    }

    void SetStateShadowDive()
    {
        state = PlayerState.ShadowDive;
        rb.gravityScale = 0;

        spriteRenderer.sprite = shadowDiveSprite;
        transform.localScale = new Vector3(shadowDiveScale * (isFacingRight? 1: -1),shadowDiveScale,1);
        // translate vertically by difference in centers so shadow state doesnt end up in the air
        transform.position += new Vector3(0, -(1.9f-0.9f)/2, 0);
        
    }

    void Die()
    {
        state = PlayerState.Dead;
        // Handle death logic (animations, effects, restart level)
    }

    void updateFeetOn(FloorType newFloor)
    {
        feetOn = newFloor;
        if(feetOn == FloorType.None || feetOn == FloorType.Ground)
        {
            transform.eulerAngles = new Vector3(0,0,0);
        }
        else if (feetOn == FloorType.RightWall)
        {
            transform.eulerAngles = new Vector3(0,0,90);
        }
        else if(feetOn == FloorType.LeftWall)
        {
            transform.eulerAngles = new Vector3(0,0,-90);
        }
        else //feetOn == FloorType.Ceiling
        {
            transform.eulerAngles = new Vector3(0,0,180);
        }
    }

    // Go to wall
    // Function to "smoothly" turn corners
    // - Should still be grounded
    // - Check if walled (maybe) for cause of 1 height gaps
    // - Change feet on and rotate body
    // - isFacing Right should stay same

    // 4 cases for each, coming from  

    void GoToFloor()
    {
        float newX = transform.position.x;
        float newY = transform.position.y;

        // Concave transition
        if(isGrounded)
        {
            newX = transform.position.x + (isFacingRight? 1:-1)*shadowDiveScale/2;
            newY = Mathf.Round(transform.position.y - shadowDiveScale/2) + shadowDiveScale/2 + positionOffset;
        }
        // Convex transition
        else
        {
            if(feetOn ==FloorType.LeftWall)
            {
                newX = transform.position.x - shadowDiveScale/2;
            }
            else if(feetOn ==FloorType.RightWall)
            {
                newX = transform.position.x + shadowDiveScale/2;

            }
            newY = Mathf.Round(transform.position.y) + shadowDiveScale/2 + positionOffset;
        }

        transform.position = new Vector2(newX, newY);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        updateFeetOn(FloorType.Ground);
    }
    void GoToRightWall()
    {

        float newX = transform.position.x;
        float newY = transform.position.y;
        // Covcave Transition
        if(isGrounded)
        {
            newX = Mathf.Round(transform.position.x + shadowDiveScale/2) - shadowDiveScale/2;
            newY = transform.position.y;
        }
        // Convex Transition
        else
        {
            if(feetOn ==FloorType.Ground)
            {
                newY = transform.position.y - shadowDiveScale/2;
            }
            else if(feetOn ==FloorType.Ceiling)
            {
                newY = transform.position.y + shadowDiveScale/2;
            }
            newX = Mathf.Round(transform.position.x) - shadowDiveScale/2;
        }

        transform.position = new Vector2(newX, newY);
        rb.velocity = new Vector2(0, rb.velocity.y);
        updateFeetOn(FloorType.RightWall);
    }
    void GoToLeftWall()
    {
        float newX = transform.position.x;
        float newY = transform.position.y;
        // Covcave Transition
        if(isGrounded)
        {
            newX = Mathf.Round(transform.position.x - shadowDiveScale/2) + shadowDiveScale/2;
            newY = transform.position.y;
        }
        // Convex Transition
        else
        {
            if(feetOn ==FloorType.Ground)
            {
                newY = transform.position.y - shadowDiveScale/2;
            }
            else if(feetOn ==FloorType.Ceiling)
            {
                newY = transform.position.y + shadowDiveScale/2;
            }
            newX = Mathf.Round(transform.position.x) + shadowDiveScale/2;
        }

        transform.position = new Vector2(newX, newY);
        rb.velocity = new Vector2(0, rb.velocity.y);
        updateFeetOn(FloorType.LeftWall);

    }
    void GoToCeiling()
    {
        float newX = transform.position.x;
        float newY = transform.position.y;

        // Concave transition
        if(isGrounded)
        {
            newX = transform.position.x + (isFacingRight? 1:-1)*shadowDiveScale/2;
            newY = Mathf.Round(transform.position.y - shadowDiveScale/2) + shadowDiveScale/2 + positionOffset;
        }
        // Convex transition
        else
        {
            if(feetOn ==FloorType.LeftWall)
            {
                newX = transform.position.x - shadowDiveScale/2;
            }
            else if(feetOn ==FloorType.RightWall)
            {
                newX = transform.position.x + shadowDiveScale/2;

            }
            newY = Mathf.Round(transform.position.y) - shadowDiveScale/2 + positionOffset;
        }

        transform.position = new Vector2(newX, newY);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        updateFeetOn(FloorType.Ceiling);
    }
    

    void CheckIfGrounded()
    {
        //TODO: Change front ground check to raycast 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, tileLayer) || Physics2D.OverlapCircle(frontGroundCheck.position, checkRadius, tileLayer);
    }

    void CheckIfWalled()
    {   
        isWalled = Physics2D.OverlapCircle(wallCheck.position, checkRadius, tileLayer);
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (state == PlayerState.ShadowDive ())
    //     {
    //         // Logic for transitioning to shadow dive on another wall
    //         if (collision.contacts[0].normal.y == 0) // Check if the collision is with a vertical surface
    //         {
    //             SetStateShadowDive();
    //         }
    //     }
    // }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Experimental.GlobalIllumination;
// using UnityEngine.Rendering;
// using UnityEngine.Rendering.Universal;


// public class PlayerController : MonoBehaviour
// {
//     public enum PlayerState
//     {
//         Normal,
//         ShadowDive,
//         Dead
//     }

//     public PlayerState state = PlayerState.Normal;
//     public float normalSpeed = 5f;
//     public float shadowDiveSpeed = 3f;
//     private bool isGrounded;
    
//     public SpotLightManager spotLightManager;
//     public LevelManager levelManager;
//     bool isInShadow = false;
//     private GameObject[] spotLightArray;

//     private bool[] lightEnabledArray; 

//     public LayerMask obstacleLayers;

//     private bool isFacingRight = true;

//     private float horizontal;
//     private float vertical;

//     private float speed = 8f;
//     private float jumpingPower = 8f;

//     private bool isWallSliding;

//     private bool isWallJumping;
//     private float wallJumpingDirection;
//     private float wallJumpingTime = 0.2f;
//     private float wallJumpingCounter;
//     private float wallJumpingDuration = 0.4f;
//     private Vector2 wallJumpingPower = new Vector2(3f, 8f);

//     private float wallSlidingSpeed = 10f;
//     [SerializeField] private Rigidbody2D rb;
//     [SerializeField] private Transform wallCheck;
//     [SerializeField] private LayerMask wallLayer;
//     [SerializeField] private Transform groundCheck;
//     [SerializeField] private LayerMask groundLayer;

//     // Start is called before the first frame update
//     void Start()
//     {
//         spotLightArray = spotLightManager.getSpotLightArray();
//         lightEnabledArray = spotLightManager.getlightEnabledArray();
//     }

    

//     // Update is called once per frame
//     void Update() {
//         Move();
//         spotLightArray = spotLightManager.getSpotLightArray();
//         lightEnabledArray = spotLightManager.getlightEnabledArray();
//         SetLights();
//         for (int i = 0; i < spotLightArray.Length; i++)
//         {
//             if(spotLightArray[i].GetComponent<SpotLightController>().DoesIlluminate(gameObject.transform.position, obstacleLayers))
//             {
//                 FindObjectOfType<LevelManager>().Restart();
//             }             
//         }
//     }
//     void HandleNormalMovement()
//     {
//         float move = Input.GetAxis("Horizontal");
//         Vector2 targetVelocity = new Vector2(move * normalSpeed, rb.velocity.y);
//         rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref targetVelocity, 0.05f);

//         if (IsInShadow() && Input.GetKeyDown(KeyCode.DownArrow) && isGrounded)
//         {
//             SetStateShadowDive();
//         }
//     }
//     void Move()
//     {
        
//         horizontal = levelManager.pauseInput ? 0 : Input.GetAxisRaw("Horizontal");
//         vertical = levelManager.pauseInput ? 0 : Input.GetAxisRaw("Vertical");

//         if (!levelManager.pauseInput && IsGrounded())
//         {
//             if (Input.GetButtonDown("Jump"))
//             {
//                 rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
//             }
//         }
//         if (IsWalled() && horizontal != 0f)
//         {
//             rb.velocity = new Vector2(rb.velocity.x, wallSlidingSpeed * vertical + 3.0f);
//         }

//         // Flip only when not wall sliding
//         if (!isWallSliding)
//         {
//             Flip();
//         }

//     }


//     private void FixedUpdate()
//     {
//         if (!isWallJumping)
//         {
//             rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
//         }
//     }

//     void SetLights(){
//         if(Input.GetKeyDown(KeyCode.Alpha1)){
//             spotLightManager.TurnOffLight(0);
//         }
//         else if (Input.GetKeyDown(KeyCode.Alpha2)){
//             spotLightManager.TurnOffLight(1);
//         }
//     }


//     private bool IsGrounded()
//     {
//         return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
//     }

//     private bool IsWalled() {
//         return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
//     }


//     //private void WallSlide()
//     //{
//     //    if (IsWalled() && !IsGrounded() && horizontal != 0f)
//     //    {
//     //        isWallSliding = true;
//     //        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
//     //    }
//     //    else
//     //    {
//     //        isWallSliding = false;
//     //    }
//     //}


//     //private void WallJump()
//     //{
//     //    if (isWallSliding)
//     //    {
//     //        isWallJumping = false;
//     //        wallJumpingDirection = -transform.localScale.x;
//     //        wallJumpingCounter = wallJumpingTime;

//     //        CancelInvoke(nameof(StopWallJumping));
//     //    }
//     //    else
//     //    {
//     //        wallJumpingCounter -= Time.deltaTime;
//     //    }

//     //    if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
//     //    {
//     //        isWallJumping = true;
//     //        rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
//     //        wallJumpingCounter = 0f;

//     //        if (transform.localScale.x != wallJumpingDirection)
//     //        {
//     //            isFacingRight = !isFacingRight;
//     //            Vector3 localScale = transform.localScale;
//     //            localScale.x *= -1f;
//     //            transform.localScale = localScale;
//     //        }

//     //        Invoke(nameof(StopWallJumping), wallJumpingDuration);
//     //    }
//     //}

//     //private void StopWallJumping()
//     //{
//     //    isWallJumping = false;
//     //}


//     private void Flip()
//     {
//         if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
//         {
//             isFacingRight = !isFacingRight;
//             Vector3 localScale = transform.localScale;
//             localScale.x *= -1f;
//             transform.localScale = localScale;
//         }
//     }
// }

