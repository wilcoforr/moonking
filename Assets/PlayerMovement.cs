using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle basic player movement.
/// </summary>
public class PlayerMovement : MonoBehaviour
{

    private float movementX = 0;

    private bool isJumping = false;
    private int jumpChargeCounter = 0;

    //private
    public static int LevelCount = 1; //keep track of what level to load



    public float MovementSpeed = 600;
    public int JumpCount = 10; //if jumpcount goes to 0, end game?
    public float JumpForce = 20.0f;

    public Rigidbody2D PlayerRigidBody;

    public Vector3 originalScale = new Vector3(1f, 1f, 1f);

    public Transform FeetTransform;
    public Transform HeadTransform;

    public LayerMask GroundLayerMask;
    public LayerMask DeathLayerMask;
    public LayerMask LevelCompleteLayerMask;

    //public Text debugText;
    public Text jumpCountText;

    private void ResetJumpForce()
    {
        JumpForce = 10.0f;
        jumpChargeCounter = 0;

        PlayerRigidBody.transform.localScale = originalScale;
    }

    void Start()
    {
        ResetJumpForce();

        jumpCountText.text = "" + JumpCount;
    }


    void Update()
    {
        movementX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Jump") && IsGrounded())
        {
            if (jumpChargeCounter < 1500)
            {
                jumpChargeCounter += 15;

                PlayerRigidBody.transform.localScale = new Vector3(
                        PlayerRigidBody.transform.localScale.x + 0.01f,
                        PlayerRigidBody.transform.localScale.y + 0.01f,
                        PlayerRigidBody.transform.localScale.z + 0.01f
                    );

                //PlayerRigidBody.transform.localScale = 
                //    new Vector3(PlayerRigidBody.transform.localScale.x + 0.0005f, 
                //    PlayerRigidBody.transform.localScale.y + 0.001f, 
                //    PlayerRigidBody.transform.localScale.z); 
            }

            isJumping = true;

            //debugText.text = "Charging.... " + jumpChargeCounter;
        }


        if (Input.GetButtonUp("Jump") && isJumping)
        {
            JumpForce += ((float)jumpChargeCounter) / 25.0f;
            //debugText.text = "Jump force: " + JumpForce;

            Jump();
            ResetJumpForce();
        }
    }

    private void Jump()
    {
        if (JumpCount == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }

        jumpCountText.text = "" + (JumpCount - 1);

        PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, JumpForce);
        isJumping = false;
        JumpCount--;
    }

    void FixedUpdate()
    {
        PlayerRigidBody.velocity = new Vector2(movementX * MovementSpeed * Time.fixedDeltaTime, PlayerRigidBody.velocity.y);

        if (HasHitDeathSurface())
        {
            LevelCount = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }

        if (HasHitLevelCompleteSurface())
        {
            LoadNextLevel();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            LevelCount = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }

    private void LoadNextLevel()
    {
        LevelCount++;

        UnityEngine.SceneManagement.SceneManager.LoadScene("NextLevel");

    }

    bool IsGrounded()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(FeetTransform.position, 0.5f, GroundLayerMask);

        return groundCheck?.gameObject != null;
    }


    bool HasHitDeathSurface()
    {
        Collider2D deathCheckHead = Physics2D.OverlapCircle(HeadTransform.position, 0.25f, DeathLayerMask);
        Collider2D deathCheckFeet = Physics2D.OverlapCircle(FeetTransform.position, 0.25f, DeathLayerMask);

        return deathCheckHead?.gameObject != null || deathCheckFeet?.gameObject != null;
    }

    bool HasHitLevelCompleteSurface()
    {
        Collider2D levelCompleteCheck = Physics2D.OverlapCircle(FeetTransform.position, 0.5f, LevelCompleteLayerMask);

        return levelCompleteCheck?.gameObject != null;
    }
}
