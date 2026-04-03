namespace MagmaHeart.Core.CameraControls
{
    public class MeteorStrikeCameraBehaviour : ICameraBehaviour
    {
        private readonly CameraZoom m_cameraZoom;
        private readonly float m_zoomOutSpeed;

        public MeteorStrikeCameraBehaviour(CameraZoom cameraZoom, float zoomOutSpeed)
        {
            m_cameraZoom = cameraZoom;
            m_zoomOutSpeed = zoomOutSpeed;
        }

        public void Update()
        {
            m_cameraZoom.Zoom(-m_zoomOutSpeed);
        }
    }
}
