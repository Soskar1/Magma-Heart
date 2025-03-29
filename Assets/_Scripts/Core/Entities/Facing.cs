using System;
using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Facing
    {
        private bool m_facingRight;
        private Transform m_transform;

        public Facing(in Transform transform, in bool facingRight)
        {
            m_transform = transform;
            m_facingRight = facingRight;
        }

        public void TryUpdateFacing(in float xMovement)
        {
            if (xMovement < 0 && m_facingRight || xMovement > 0 && !m_facingRight)
            {
                m_transform.Rotate(new Vector3(0, 180, 0));
                m_facingRight = !m_facingRight;
            }
        }
    }
}