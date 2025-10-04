using MagmaHeart.Core.Input;
using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private int m_movementSpeed;

        [Header("Camera Zoom")]
        [SerializeField] private float m_zoomSpeed;
        [SerializeField] private float m_minZoom;
        [SerializeField] private float m_maxZoom;
        [SerializeField][Range(0, 1.0f)]
        private float m_smoothTime;

        private ICameraBehaviour m_currentBehaviour;
        private ActionCameraBehaviour m_actionCameraBehaviour;
        private TurnBasedCameraBehaviour m_turnBasedCameraBehaviour;

        public TurnBasedCameraBehaviour TurnBasedCameraBehaviour => m_turnBasedCameraBehaviour;

        public void Initialize(Transform objectToTrack, ActionUserInput actionUserInput, TurnBasedUserInput turnBasedUserInput)
        {
            Camera camera = GetComponent<Camera>();
            CameraZoom zoom = new CameraZoom(camera, m_zoomSpeed, m_minZoom, m_maxZoom, m_smoothTime);

            m_actionCameraBehaviour = new ActionCameraBehaviour(actionUserInput, transform, objectToTrack, zoom);
            m_turnBasedCameraBehaviour = new TurnBasedCameraBehaviour(transform, turnBasedUserInput, m_movementSpeed, zoom);
            SwitchToActionCamera();
        }

        private void Update() => m_currentBehaviour?.Update();

        public void SwitchToActionCamera() => SwitchState(m_actionCameraBehaviour);
        public void SwitchToTurnBasedCamera() => SwitchState(m_turnBasedCameraBehaviour);
        private void SwitchState(ICameraBehaviour behaviour) => m_currentBehaviour = behaviour;
    }
}