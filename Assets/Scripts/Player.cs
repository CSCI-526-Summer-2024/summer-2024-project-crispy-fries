using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class Player : MonoBehaviour
{
    public SpotLightManager spotLightManager;
    public LevelManager levelManager;
    bool isInShadow = false;
    private GameObject[] spotLightArray;

    private bool[] lightEnabledArray; 

    public LayerMask obstacleLayers;

    private bool isFacingRight = true;

    private float horizontal;
    private float vertical;

    private float speed = 8f;
    private float jumpingPower = 8f;

    private bool isWallSliding;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(3f, 8f);

    private float wallSlidingSpeed = 10f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        spotLightArray = spotLightManager.getSpotLightArray();
        lightEnabledArray = spotLightManager.getlightEnabledArray();
    }

    

    // Update is called once per frame
    void Update() {
        Move();
        spotLightArray = spotLightManager.getSpotLightArray();
        lightEnabledArray = spotLightManager.getlightEnabledArray();
        SetLights();
        for (int i = 0; i < spotLightArray.Length; i++)
        {
            if(spotLightArray[i].GetComponent<SpotLightController>().DoesIlluminate(gameObject.transform.position, obstacleLayers))
            {
                FindObjectOfType<LevelManager>().Restart();
            }             
        }
    }

    void Move()
    {
        
        horizontal = levelManager.pauseInput ? 0 : Input.GetAxisRaw("Horizontal");
        vertical = levelManager.pauseInput ? 0 : Input.GetAxisRaw("Vertical");

        if (!levelManager.pauseInput && IsGrounded())
        {
            if (Input.GetButtonDown("Jump"))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            }
        }
        if (IsWalled() && horizontal != 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, wallSlidingSpeed * vertical + 3.0f);
        }

        // Flip only when not wall sliding
        if (!isWallSliding)
        {
            Flip();
        }

    }


    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    void SetLights(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            spotLightManager.TurnOffLight(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)){
            spotLightManager.TurnOffLight(1);
        }
    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled() {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }


    //private void WallSlide()
    //{
    //    if (IsWalled() && !IsGrounded() && horizontal != 0f)
    //    {
    //        isWallSliding = true;
    //        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
    //    }
    //    else
    //    {
    //        isWallSliding = false;
    //    }
    //}


    //private void WallJump()
    //{
    //    if (isWallSliding)
    //    {
    //        isWallJumping = false;
    //        wallJumpingDirection = -transform.localScale.x;
    //        wallJumpingCounter = wallJumpingTime;

    //        CancelInvoke(nameof(StopWallJumping));
    //    }
    //    else
    //    {
    //        wallJumpingCounter -= Time.deltaTime;
    //    }

    //    if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
    //    {
    //        isWallJumping = true;
    //        rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
    //        wallJumpingCounter = 0f;

    //        if (transform.localScale.x != wallJumpingDirection)
    //        {
    //            isFacingRight = !isFacingRight;
    //            Vector3 localScale = transform.localScale;
    //            localScale.x *= -1f;
    //            transform.localScale = localScale;
    //        }

    //        Invoke(nameof(StopWallJumping), wallJumpingDuration);
    //    }
    //}

    //private void StopWallJumping()
    //{
    //    isWallJumping = false;
    //}


    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}

