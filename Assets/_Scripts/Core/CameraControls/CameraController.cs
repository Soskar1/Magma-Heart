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

        private CameraZoom m_cameraZoom;
        private float m_currentMouseScroll;

        public ITurnSwitchListener TurnSwitchListener => m_turnBasedCameraBehaviour;

        public void Initialize(Transform objectToTrack, UserInput userInput)
        {
            Camera camera = GetComponent<Camera>();
            m_cameraZoom = new CameraZoom(camera, m_zoomSpeed, m_minZoom, m_maxZoom, m_smoothTime);

            userInput.OnMouseScroll += HandleOnMouseScroll;

            CameraTargetTracker tracker = new CameraTargetTracker(transform);
            m_actionCameraBehaviour = new ActionCameraBehaviour(tracker, objectToTrack);
            m_turnBasedCameraBehaviour = new CombatCameraBehaviour(tracker, userInput, m_movementSpeed);
            SwitchToActionCamera();
        }

        private void Update()
        {
            m_currentBehaviour?.Update();
            m_cameraZoom.Zoom(m_currentMouseScroll);
        }

        public void SwitchToActionCamera() => SwitchState(m_actionCameraBehaviour);
        public void SwitchToTurnBasedCamera() => SwitchState(m_turnBasedCameraBehaviour);
        private void SwitchState(ICameraBehaviour behaviour)
        {
            m_currentBehaviour?.Disable();
            m_currentBehaviour = behaviour;
            m_currentBehaviour.Enable();
        }

        public void EnterCombatState() => SwitchToTurnBasedCamera();
        public void ExitCombatState() => SwitchToActionCamera();

        private void HandleOnMouseScroll(object obj, OnMouseScrollEventArgs args) => m_currentMouseScroll = args.MouseScroll;
    }
}