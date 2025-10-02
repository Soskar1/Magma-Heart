using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    public class ActionCameraBehaviour : ICameraBehaviour
    {
        private readonly Transform m_transform;
        private readonly Transform m_objectToTrack;

        public ActionCameraBehaviour(Transform transform, Transform objectToTrack)
        {
            m_transform = transform;
            m_objectToTrack = objectToTrack;
        }

        public void Update() => m_transform.position = new Vector3(m_objectToTrack.position.x, m_objectToTrack.position.y, m_transform.position.z);
    }
}