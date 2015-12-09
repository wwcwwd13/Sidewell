using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;

        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

		private float m_FixedY;
        private float m_FixedZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;


		private float vertExtent;
		private float horzExtent;


        // Use this for initialization
        private void Start()
		{
			vertExtent = Camera.main.orthographicSize;    
			horzExtent = vertExtent * Screen.width / Screen.height;

			m_FixedY = transform.position.y;
			m_FixedZ = transform.position.z;

			transform.position = new Vector3(target.position.x, m_FixedY, m_FixedZ);
			/*
            transform.parent = null;
            */
        }


        // Update is called once per frame
        private void Update()
        {
			/*
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            transform.position = newPos;

            m_LastTargetPosition = target.position;
            */
			if (target.position.x > transform.position.x)
				transform.position = new Vector3(target.position.x, m_FixedY, m_FixedZ);
			else if(target.position.x < transform.position.x - horzExtent * 0.22f)
				transform.position = new Vector3(target.position.x + horzExtent * 0.22f, m_FixedY, m_FixedZ);
        }
    }
}
