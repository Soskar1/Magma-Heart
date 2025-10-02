using UnityEngine;

namespace MagmaHeart.Core.Entities.PlayableCharacters
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private ICameraBehaviour m_currentBehaviour;
        private ActionCameraBehaviour m_actionCameraBehaviour;
        private TurnBasedCameraBehaviour m_turnBasedCameraBehaviour;

        public void Initialize(Transform objectToTrack)
        {
            m_actionCameraBehaviour = new ActionCameraBehaviour(transform, objectToTrack);
            m_turnBasedCameraBehaviour = new TurnBasedCameraBehaviour();
            SwitchToActionCamera();
        }

        private void Update() => m_currentBehaviour?.Update();

        public void SwitchToActionCamera() => SwitchState(m_actionCameraBehaviour);
        public void SwitchToTurnBasedCamera() => SwitchState(m_turnBasedCameraBehaviour);
        private void SwitchState(ICameraBehaviour behaviour) => m_currentBehaviour = behaviour;
    }
}