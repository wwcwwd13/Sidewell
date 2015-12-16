using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
		[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] private float m_CurrentSpeed = 0f;                 // 현재 X방향 속도
        [SerializeField] private EdgeCollider2D leftSideTrigger;            // 왼쪽 옆구리
        [SerializeField] private EdgeCollider2D rightSideTrigger;           // 오른쪽 옆구리

        private const float m_JumpForce = 800f;                  // Amount of force added when the player jumps.
        private const bool m_AirControl = true;                 // Whether or not a player can steer while jumping;
        private const float jumpUpVelocity = 14f;

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        private bool jumpPending = false;
        private float jumpPendElapsed = 0.0f;
        private const float jumpPendLimit= 0.1f;

        private bool onRightWall = false;
        private bool onLeftWall = false;


        //private float m_CurrentSpeed;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_CurrentSpeed = 0f;
        }


        private void FixedUpdate()
        {
            m_Grounded = false;
            
            if (jumpPending)
            {
                jumpPendElapsed += Time.fixedDeltaTime;
                if(jumpPendElapsed >= jumpPendLimit)
                    jumpPending = false;
            }

            
            onRightWall = Physics2D.IsTouchingLayers(rightSideTrigger, m_WhatIsGround);
            onLeftWall = Physics2D.IsTouchingLayers(leftSideTrigger, m_WhatIsGround);


            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }


        public void Move(float move, bool crouch, bool jump, bool jumpLong)
        {
            /*
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }
            */

            // Set whether or not the character is crouching in the animator
            // m_Anim.SetBool("Crouch", crouch);

            if(jump)
            {
                jumpPending = true;
                jumpPendElapsed = 0.0f;
            }

            if (jumpLong)
                m_Rigidbody2D.gravityScale = 3.5f;
            else
                m_Rigidbody2D.gravityScale = 6.0f;

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                // move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                m_CurrentSpeed = m_Rigidbody2D.velocity.x;
                m_CurrentSpeed *= Mathf.Pow(0.2f, Time.deltaTime);
                m_CurrentSpeed += move * Time.deltaTime * 40f;
                // 0.05f은 낮은만큼 더 미끄러진다.

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(m_CurrentSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jumpPending && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpUpVelocity);
                jumpPending = false;
            }
            else if(onRightWall && jumpPending)
            {
                m_Rigidbody2D.velocity = new Vector2(-3f, jumpUpVelocity);
                jumpPending = false;
            }
            else if (onLeftWall && jumpPending)
            {
                m_Rigidbody2D.velocity = new Vector2(10f, jumpUpVelocity);
                jumpPending = false;
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
