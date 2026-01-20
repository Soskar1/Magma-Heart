using UnityEngine;

namespace MagmaHeart.Core.Entities
{
    public class Facing : MonoBehaviour
    {
        [SerializeField] private bool m_facingRight;

        public void TryUpdateFacing(in float xDirection)
        {
            if (xDirection < 0 && m_facingRight || xDirection > 0 && !m_facingRight)
            {
                transform.Rotate(new Vector3(0, 180, 0));
                m_facingRight = !m_facingRight;
            }
        }
    }
}