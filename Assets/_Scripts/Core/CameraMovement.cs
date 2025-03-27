using UnityEngine;

namespace MagmaHeart.Core
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform m_objectToTrack;

        public Transform ObjectToTrack { set => m_objectToTrack = value; }

        private void Update() 
        {
            if (m_objectToTrack == null)
            {
               Debug.LogWarning("CameraMovement does not have an object to track");
               return;
            }

            transform.position = new Vector3(m_objectToTrack.position.x, m_objectToTrack.position.y, transform.position.z);
        }
    }
}