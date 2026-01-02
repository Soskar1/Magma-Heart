using MagmaHeart.Core.CombatSystem;
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

        private BattleCameraBehaviour m_combatCameraBehaviour;

        private CameraZoom m_cameraZoom;
        private float m_currentMouseScroll;

        public void Initialize(Transform objectToTrack, UserInput userInput, Battle battle)
        {
            Camera camera = GetComponent<Camera>();
            m_cameraZoom = new CameraZoom(camera, m_zoomSpeed, m_minZoom, m_maxZoom, m_smoothTime);

            userInput.OnMouseScroll += HandleOnMouseScroll;

            CameraTargetTracker tracker = new CameraTargetTracker();
            m_combatCameraBehaviour = new BattleCameraBehaviour(transform, tracker, userInput, m_movementSpeed, battle);
        }

        private void Update()
        {
            m_combatCameraBehaviour.Update();
            m_cameraZoom.Zoom(m_currentMouseScroll);
        }

        public void MoveTo(Vector2 position) => transform.position = new Vector3(position.x, position.y, transform.position.z);
        public void EnableManualMovement(BoundsInt movementBounds) => m_combatCameraBehaviour.Enable(movementBounds);
        public void DisableManualMovement() => m_combatCameraBehaviour.Disable();

        private void HandleOnMouseScroll(object obj, OnMouseScrollEventArgs args) => m_currentMouseScroll = args.MouseScroll;
    }
}