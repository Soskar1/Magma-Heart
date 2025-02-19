using UnityEngine;

namespace MagmaHeart.Core
{
    public class UserInput
    {
        private Controls m_controls;
        public Vector2 Movement => m_controls.Player.Move.ReadValue<Vector2>();

        public UserInput()
        {
            m_controls = new Controls();
        }

        public void Enable() => m_controls.Enable();
        public void Disable() => m_controls.Disable();
    }
}