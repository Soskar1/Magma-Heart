using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Facing : MonoBehaviour
    {
        [SerializeField] private bool m_facingRight;

        public void TryUpdateFacing(in float xMovement)
        {
            if (xMovement < 0 && m_facingRight || xMovement > 0 && !m_facingRight)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                m_facingRight = !m_facingRight;
            }
        }
    }
}