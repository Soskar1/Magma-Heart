using MagmaHeart.Core.CameraControls;
using MagmaHeart.Core.CombatSystem;
using MagmaHeart.Core.Input;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private int m_movementSpeed;

    [Header("Camera Zoom")]
    [SerializeField] private float m_zoomSpeed;
    [SerializeField] private float m_minZoom;
    [SerializeField] private float m_maxZoom;
    [SerializeField, Range(0, 1.0f)]
    private float m_smoothTime;

    [Header("Meteor Strike Camera Zoom")]
    [SerializeField] private float m_meteorStrikeZoomSpeed;
    [SerializeField] private float m_meteorStrikeMaxZoom;

    [Header("Shake")]
    [SerializeField] private float m_defaultShakeDuration = 0.2f;

    private BattleCameraBehaviour m_battleCamera;
    private MeteorStrikeCameraBehaviour m_meteorStrikeCamera;
    private ICameraBehaviour m_currentCameraBehaviour;

    private float m_shakeTimeRemaining;
    private float m_shakeMagnitude;
    private Vector3 m_currentShakeOffset;

    public void Initialize(Transform objectToTrack, UserInput userInput, Battle battle)
    {
        Camera camera = GetComponent<Camera>();
        CameraZoom cameraZoom = new CameraZoom(camera, m_zoomSpeed, m_minZoom, m_maxZoom, m_smoothTime);

        CameraTargetTracker tracker = new CameraTargetTracker();
        m_battleCamera = new BattleCameraBehaviour(transform, tracker, userInput, m_movementSpeed, battle, cameraZoom);

        CameraZoom meteorStrikeCameraZoom = new CameraZoom(camera, m_meteorStrikeZoomSpeed, m_minZoom, m_meteorStrikeMaxZoom, m_smoothTime);
        m_meteorStrikeCamera = new MeteorStrikeCameraBehaviour(meteorStrikeCameraZoom, m_meteorStrikeZoomSpeed);

        m_currentCameraBehaviour = m_battleCamera;
    }

    private void Update()
    {
        transform.position -= m_currentShakeOffset;
        m_currentShakeOffset = Vector3.zero;

        m_currentCameraBehaviour.Update();

        ApplyShake();
    }

    public void MoveTo(Vector2 position) =>
        transform.position = new Vector3(position.x, position.y, transform.position.z);

    public void EnableManualMovement(BoundsInt movementBounds)
    {
        m_battleCamera.Enable(movementBounds);
        m_currentCameraBehaviour = m_battleCamera;
    }

    public void DisableManualMovement() => m_battleCamera.Disable();

    public void EnableMeteorStrikeBehaviour() => m_currentCameraBehaviour = m_meteorStrikeCamera;

    public void Shake(float duration, float magnitude)
    {
        m_shakeTimeRemaining = Mathf.Max(m_shakeTimeRemaining, duration);
        m_shakeMagnitude = Mathf.Max(m_shakeMagnitude, magnitude);
    }

    private void ApplyShake()
    {
        if (m_shakeTimeRemaining <= 0f)
        {
            m_currentShakeOffset = Vector3.zero;
            return;
        }

        m_shakeTimeRemaining -= Time.deltaTime;

        float normalizedTime = Mathf.Clamp01(m_shakeTimeRemaining / Mathf.Max(0.0001f, m_defaultShakeDuration));
        float currentMagnitude = m_shakeMagnitude * normalizedTime;

        Vector2 offset2D = Random.insideUnitCircle * currentMagnitude;
        m_currentShakeOffset = new Vector3(offset2D.x, offset2D.y, 0f);

        transform.position += m_currentShakeOffset;

        if (m_shakeTimeRemaining <= 0f)
        {
            m_shakeTimeRemaining = 0f;
            m_shakeMagnitude = 0f;
        }
    }
}