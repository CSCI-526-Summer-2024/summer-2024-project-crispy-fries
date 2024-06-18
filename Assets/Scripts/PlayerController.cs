using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private string URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSfOGYgVRRp-6DSuW2OLMH2o6p_P-htPqSgNTqotq8QVgaTIzg/formResponse";
    public enum PlayerState
    {
        Normal,
        ShadowDive,
        Dead,

        Win
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
    public float normalSpeed = 9f;
    public float shadowDiveSpeed = 6f;

    public float normalJumpForce = 15f;
    public float shadowJumpForce = 5f;
    public LayerMask tileLayer;

    public GameManager gameManager;
    

    private bool isFacingRight;

    public Sprite normalSprite;

    public Sprite shadowDiveSprite;

    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;
    public bool isGrounded;
    private bool isWalled;

    // Used when manually setting position to prevent rigid body issues
    private float positionOffset = 0.01f;

    public FloorType feetOn;
    public Transform groundCheck; // Ground check position
    public Transform wallCheck; // Wall check position

    public Transform frontGroundCheck; // Ground check position
    public float checkRadius = 0.2f;

    public float shadowDiveScale = 0.9f;
    private Vector2 playerSizeNormal = new Vector2(0.9f, 1.9f);
    private Vector2 playerSizeShadow;

    //For debugging isGrounded and feetOn
    public float verticalSpeed;

    private int sceneIndex;

    private int randomId;
    private int[] lightShadowData;

    private int[] lightOfftime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isFacingRight = true;
        feetOn = FloorType.Ground;
        playerSizeShadow = new Vector2(shadowDiveScale, shadowDiveScale);

        lightShadowData = new int[gameManager.spotLightManager.getSpotLightArray().Length];
        for (int i = 0; i < lightShadowData.Length; i++)
        {
            lightShadowData[i] = 0;
        }
        lightOfftime = new int[gameManager.spotLightManager.getSpotLightArray().Length];
        for (int i = 0; i < lightOfftime.Length; i++)
        {
            lightOfftime[i] = 0;
        }


        sceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        randomId = UnityEngine.Random.Range(100000, 999999);

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
                break;
            case PlayerState.Win:
                break;
        }
        verticalSpeed = rb.velocity.y;
    }

    

    void HandleNormalMovement()
    {
        CheckPlayerDeath();
        if(state == PlayerState.Dead) return;

        float move = Input.GetAxis("Horizontal");
        CheckFlip(move);
        CheckIfGrounded();

        if(feetOn == FloorType.None)
        {
            // need vertical velocity below some value so it doesnt get triggered immediately after jumping
            if(isGrounded && rb.velocity.y<=0.01f)
            {
                updateFeetOn(FloorType.Ground);
            }
        }

        //Move horizontal
        Vector2 targetVelocity = new Vector2(move * normalSpeed, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref targetVelocity, 0.0001f);


        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && feetOn == FloorType.Ground)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && isGrounded && feetOn == FloorType.Ground)
        {
            SetStateShadowDive();
        }
        CheckFlip(move);
        CheckIfGrounded();
        CheckIfWalled();
    }

    void HandleShadowDiveMovement()
    {
        CheckPlayerDeath();
        if(state == PlayerState.Dead) return;

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
        


       
        if (Input.GetKeyDown(KeyCode.UpArrow) && canTransformToNormal())
        {
            SetStateNormal();
            return;

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
            // Different jump heights based on player state
            if (state == PlayerState.Normal) {
                rb.velocity = new Vector2(rb.velocity.x, normalJumpForce);
            } else if (state == PlayerState.ShadowDive) {
                rb.velocity = new Vector2(rb.velocity.x, shadowJumpForce);
            }
        }
        else if(feetOn == FloorType.RightWall)
        {
            rb.velocity = new Vector2(-normalJumpForce, rb.velocity.y);
            //Discuss: Face player left on jumping from right wall and vice versa
            if(isFacingRight) Flip();
        }
        else if(feetOn == FloorType.LeftWall)
        {
            rb.velocity = new Vector2(normalJumpForce, rb.velocity.y);
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

    bool canTransformToNormal()
    {
         // Transform back to normal state only on ground
        if(isGrounded && feetOn == FloorType.Ground)
        {
            //Check that there is no platform right above player
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1f, tileLayer);
            if(hit.collider == null)
            {
                return true;
            }
        }
        return false;
    }

    void SetStateNormal()
    {
        state = PlayerState.Normal;
        rb.gravityScale = 3;

        spriteRenderer.sprite = normalSprite;
        transform.localScale = new Vector3(playerSizeNormal.x * (isFacingRight? 1: -1),playerSizeNormal.y,1);
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
        transform.localScale = new Vector3(playerSizeShadow.x * (isFacingRight? 1: -1),playerSizeShadow.y,1);
        // translate vertically by difference in centers so shadow state doesnt end up in the air
        transform.position += new Vector3(0, -(1.9f-0.9f)/2, 0);
        
    }

    void Die()
    {
        StartCoroutine(Post(randomId.ToString(), sceneIndex.ToString(), lightShadowData, true));
        state = PlayerState.Dead;
        rb.velocity = Vector2.zero;
        gameManager.levelManager.Restart();
        
    }

    public void Win()
    {
        StartCoroutine(Post(randomId.ToString(), sceneIndex.ToString(), lightShadowData, false));
        state = PlayerState.Win;
        rb.velocity = Vector2.zero;
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
        else
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

    void CheckPlayerDeath()
    {
        Vector2[] corners = GetPlayerCorners();
        GameObject[] lights = gameManager.spotLightManager.getSpotLightArray();
        for (int i = 0; i < lights.Length; i++) 
        {
            GameObject light = lights[i];
            bool count = false;

            if (!light.GetComponent<SpotLightController>().IsLightOn)
            {
                lightOfftime[i]++;
            }

            

            foreach (Vector2 corner in corners)
            {
                if(light.GetComponent<SpotLightController>().DoesIlluminate(corner, tileLayer))
                {
                    Die();
                    
                    return;
                } else if (!count & light.GetComponent<SpotLightController>().IfInTheShadow(corner, tileLayer)){
                    lightShadowData[i]++;
                    count = true;
                }
            }
            // Debug.Log($"Light {i}: Offtime = {lightOfftime[i]}, ShadowData = {lightShadowData[i]}");
        }
    }

    Vector2[] GetPlayerCorners()
    {
        Vector2 position = transform.position;
        Vector2 size = (state == PlayerState.Normal) ? playerSizeNormal : playerSizeShadow;

        return new Vector2[]
        {
            position + new Vector2(-size.x / 2, size.y / 2),  // Top-left
            position + new Vector2(size.x / 2, size.y / 2),   // Top-right
            position + new Vector2(-size.x / 2, -size.y / 2), // Bottom-left
            position + new Vector2(size.x / 2, -size.y / 2)   // Bottom-right
        };
    }




    private IEnumerator Post(string randomId, string sceneIndex, int[] light, bool isDead)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1662667842", randomId);
        form.AddField("entry.1637880345", sceneIndex);
        string productName = gameManager.buildName;
        string version = Application.version;  
        string buildName = productName + "_" + version;
        form.AddField("entry.754931394", buildName);
        form.AddField("entry.1060833998", isDead ? "Dead":"Win");
        String[] FormFieldForLight = new String[4];
        FormFieldForLight[0] = "entry.1465073703";
        FormFieldForLight[1] = "entry.1443173421";
        FormFieldForLight[2] = "entry.1597942742";
        FormFieldForLight[3] = "entry.2028998425";

        String[] FormFieldForTotalLightOff = new String[4];
        FormFieldForTotalLightOff[0] = "entry.423518393";
        FormFieldForTotalLightOff[1] = "entry.1390432814";
        FormFieldForTotalLightOff[2] = "entry.1780117184";
        FormFieldForTotalLightOff[3] = "entry.350910402";

        for (int i = 0; i< light.Length; i++)
        {
            form.AddField(FormFieldForLight[i], light[i].ToString());
            form.AddField(FormFieldForTotalLightOff[i], lightOfftime[i].ToString());
            
        }
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
