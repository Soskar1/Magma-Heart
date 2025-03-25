using UnityEngine;

namespace MagmaHeart.Core
{
    public class UserInput
    {
        public Controls Controls { get; private set;}
        public Vector2 Movement => Controls.Player.Move.ReadValue<Vector2>();

        public UserInput() => Controls = new Controls();

        public void Enable() => Controls.Enable();
        public void Disable() => Controls.Disable();
    }
}