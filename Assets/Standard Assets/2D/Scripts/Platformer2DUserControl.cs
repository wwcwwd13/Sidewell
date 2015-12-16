using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private bool m_Dash;
        private bool m_LR_on;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
            m_LR_on = false;
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                //m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
                m_Jump = Input.GetKeyDown(KeyCode.Z);
            }
            if (!m_Dash)
            {
                m_Dash = Input.GetKeyDown(KeyCode.X);
            }
            if (Input.GetKeyDown(KeyCode.P))
                m_LR_on = !m_LR_on;
        }


        private void FixedUpdate()
        {
            // Read the inputs.

            bool jumpLong = Input.GetKey(KeyCode.Z);
            float h;
            if (!m_LR_on)
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                    h = -1.0f;
                else
                    h = 1.0f;
                //h = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            }
            else
            {
                h = 1.0f;
            }

            // Pass all parameters to the character control script.
            m_Character.Move(h, m_Dash, m_Jump, jumpLong);
            m_Jump = false;
        }
    }
}
