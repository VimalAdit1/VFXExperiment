using UnityEngine;

public class CharacterMove : MonoBehaviour
{

    [SerializeField]
    float moveSpeed = 500;
    [SerializeField]
    float jumpHeight = 10;
    [SerializeField]
    float gravityScale;
    [SerializeField]
    float longJumpWindow;
    [Tooltip("The factor at which a downward force is applied when jump is cancelled")]
    [SerializeField, Range(0, 1)]
    float jumpCancelMultiplier = 0.2f;
    [SerializeField]
    float fallingGravityScale;
    [SerializeField]
    Transform groundCheck;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    Animator animator;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    
    private Material rimLightShader;
    bool hasLight;

    
    float horizontalInput;

    float jumpForce;
    float jumpHeldTime;
    bool isJumping;
    bool isFacingRight;

    bool isKnockedback;
    float knockbackTimer;

    public bool isReady;
    void Start()
    {
        InitializePlayerParameters();
        rimLightShader = spriteRenderer.material;
        RemoveLight();
    }

    private void InitializePlayerParameters()
    {
        isFacingRight = true;
        isReady = false;
        animator.SetBool("isMoving", false);
    }


    void Update()
    {
        UpdateGravityScale();
        HandleMovement();
        HandleJump();
    }



    private void UpdateGravityScale()
    {
        if (rb.linearVelocity.y >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else if (!IsGrounded())
        {
            rb.gravityScale = fallingGravityScale;
        }
    }

    void HandleMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (!isReady && horizontalInput != 0)
        {
            isReady = true;
        }
        if (horizontalInput != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        //FlipSpriteOrientation();
    }

    private void FlipSpriteOrientation()
    {
        if (rb.linearVelocity.x > 0 && !isFacingRight || rb.linearVelocity.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void HandleJump()
    {
        //Jumpforce calculated to maintain constant jumpHeight based on the gravity
        jumpForce = Mathf.Sqrt(jumpHeight * Physics2D.gravity.y * gravityScale * -2) * rb.mass;
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Debug.Log("Jumping");
            rb.gravityScale = gravityScale;
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
            jumpHeldTime = 0;
        }
        if (isJumping)
        {
            jumpHeldTime += Time.deltaTime;
            if (jumpHeldTime < longJumpWindow && Input.GetButtonUp("Jump"))
            {
                rb.gravityScale = fallingGravityScale;
                rb.AddForce(transform.up * (-1 * jumpForce * jumpCancelMultiplier), ForceMode2D.Impulse);
            }
            if (rb.linearVelocity.y < 0)
            {
                isJumping = false;
            }
        }
    }
    void FixedUpdate()
    {
        if (!isKnockedback)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed * Time.deltaTime, rb.linearVelocity.y);
        }
    }
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }


    public void SetLightProperties(Vector3 transformPosition, Color lightColor, float lightThicknes)
    {
        if (hasLight)
        {
            return;
        }
        SetLightThickness(lightThicknes);
        rimLightShader.SetVector("_lightPosition", transformPosition);
        rimLightShader.SetColor("_lightColor", lightColor);
        hasLight = true;
    }

    public void SetLightThickness(float thickness)
    {
        rimLightShader.SetFloat("_lightThickness", thickness);
    }

    public void RemoveLight()
    {
        rimLightShader.SetFloat("_lightThickness", 0);
        hasLight = false;
    }
}


