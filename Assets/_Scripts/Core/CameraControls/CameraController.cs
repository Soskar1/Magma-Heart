using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using MagmaHeart.Core.StateMachines;
using UnityEngine;

namespace MagmaHeart.Core.CameraControls
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour, ICombatStateListener
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
        private CombatCameraBehaviour m_turnBasedCameraBehaviour;

        public ITurnSwitchListener TurnSwitchListener => m_turnBasedCameraBehaviour;

        public void Initialize(Transform objectToTrack, ActionUserInput actionUserInput, CombatUserInput turnBasedUserInput)
        {
            Camera camera = GetComponent<Camera>();
            CameraZoom zoom = new CameraZoom(camera, m_zoomSpeed, m_minZoom, m_maxZoom, m_smoothTime);

            m_actionCameraBehaviour = new ActionCameraBehaviour(actionUserInput, transform, objectToTrack, zoom);
            m_turnBasedCameraBehaviour = new CombatCameraBehaviour(transform, turnBasedUserInput, m_movementSpeed, zoom);
            SwitchToActionCamera();
        }

        private void Update() => m_currentBehaviour?.Update();

        public void SwitchToActionCamera() => SwitchState(m_actionCameraBehaviour);
        public void SwitchToTurnBasedCamera() => SwitchState(m_turnBasedCameraBehaviour);
        private void SwitchState(ICameraBehaviour behaviour) => m_currentBehaviour = behaviour;

        public void EnterCombatState() => SwitchToTurnBasedCamera();
        public void ExitCombatState() => SwitchToActionCamera();
    }
}