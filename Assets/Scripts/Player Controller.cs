using Unity.Jobs;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D theRB;
    public float jumpForce;
    public float runSpeed;
    private float activeSpeed;

    private bool isGrounded;
    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool canDoubleJump;

    public Animator anim;

    public float knockbackLength, knockbackSpeed;
    private float knockbackCounter;


    void Start()
    {
        // QUAN TRỌNG: Freeze rotation để player không bị xoay trên moving platforms
        if (theRB != null)
        {
            theRB.freezeRotation = true;
        }
    }


    void Update()
    {
        if (Time.timeScale > 0f)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);

            if (isGrounded)
            {
                canDoubleJump = true;
            }

            if (knockbackCounter <= 0)
            {
                activeSpeed = moveSpeed;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    activeSpeed = runSpeed;
                }

                theRB.linearVelocity = new Vector2(Input.GetAxisRaw("Horizontal") * activeSpeed, theRB.linearVelocity.y);

                if (Input.GetButtonDown("Jump"))
                {
                    if (isGrounded == true)
                    {
                        Jump();
                        canDoubleJump = true;
                        anim.SetBool("isDoubleJump", false);
                    }
                    else
                    {
                        if (canDoubleJump == true)
                        {
                            Jump();
                            canDoubleJump = false;
                            //anim.SetBool("isDoubleJump", true);
                            anim.SetTrigger("doDoubleJump");
                        }
                    }
                }

                if (theRB.linearVelocity.x >= 0)
                {
                    transform.localScale = new Vector3(4f, 4f, 1f);
                }
                else if (theRB.linearVelocity.x < 0)
                {
                    transform.localScale = new Vector3(-4f, 4f, 1f);
                }
            }
            else
            {
                knockbackCounter -= Time.deltaTime;
                theRB.linearVelocity = new Vector2(knockbackSpeed * -transform.localScale.x, theRB.linearVelocity.y);
            }
            // Animation Handler
            anim.SetFloat("speed", Mathf.Abs(theRB.linearVelocity.x));
            anim.SetBool("isGrounded", isGrounded);
            anim.SetFloat("ySpeed", theRB.linearVelocity.y);
        }
    }

    public void Jump()
    {
        theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, jumpForce);
        AudioManager.instance.PlaySFX(0);
    }    

    public void KnockBack()
    {
        theRB.linearVelocity = new Vector2(0f, jumpForce * .5f);
        anim.SetTrigger("isKnockingBack");

        knockbackCounter = knockbackLength;
    }    

    public void BouncePlayer(float bounceAmount)
    {
        theRB.linearVelocity = new Vector2(theRB.linearVelocity.x, bounceAmount);

        canDoubleJump = true;

        anim.SetBool("isGrounded", true);
    }    
}
