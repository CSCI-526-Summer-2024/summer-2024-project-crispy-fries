using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed;
    private float Move;

    public float jump;
    public float boostedJump;

    public bool isJumping;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public Transform lightSource;
    public LayerMask obstacleLayers;

    public bool isInShadow;
    public bool isDiving;
    public float normalScale = 1.0f;
    public float shrinkScale = 0.1f;

    private Vector3 respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.localScale = new Vector3(normalScale, normalScale, normalScale);
        spriteRenderer.color = Color.white;
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(speed * Move, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && !isJumping && !isInShadow)
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump));
        }
        else if (Input.GetButtonDown("Jump") && !isJumping && isInShadow && !isDiving)
        {
            rb.AddForce(new Vector2(rb.velocity.x, boostedJump));
        }
        DetectInShadow();
        HandleDiveInput();
        AdjustScale();
        CheckOutOfBounds();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = true;
        }
    }

    void DetectInShadow()
    {
        Vector3 direction = lightSource.position - transform.position;

        // Check for an obstacle between the object and the light source
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, obstacleLayers);

        // Debug.DrawRay(transform.position, direction, Color.red);


        if (hit.collider != null)
        {
            // Debug.Log("Obstacle detected: " + hit.collider.name);
            isInShadow = true;
            spriteRenderer.color = Color.black;
        }
        else
        {
            // No obstacle detected
            // Debug.Log("Obstacle not detected");
            isInShadow = false;
            spriteRenderer.color = Color.white;
        }
    }

    void HandleDiveInput()
    {
        if (!isInShadow)
        {
            isDiving = false;
        }
        else if (isInShadow && !isDiving && !isJumping && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
        {
            isDiving = true;
        }
        else if (isInShadow && isDiving && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
        {
            isDiving = false;
        }

        
    }

    void AdjustScale()
    {
        if (isDiving)
        {
            transform.localScale = new Vector3(normalScale, shrinkScale, normalScale);

        }
        else
        {
            transform.localScale = new Vector3(normalScale, normalScale, normalScale);
        }
    }

    void CheckOutOfBounds()
    {
        if (IsBelowCamera(Camera.main))
        {
            transform.position = respawnPoint;
            rb.velocity = Vector2.zero;
        }
    }
    bool IsBelowCamera(Camera camera)
    {
        Vector3 viewportPoint = camera.WorldToViewportPoint(transform.position);
        return viewportPoint.y < 0;
    }

}
