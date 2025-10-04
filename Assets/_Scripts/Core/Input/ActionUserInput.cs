using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class ActionUserInput
    {
        private UserInput m_userInput;

        public Controls.ActionPlayerActions ActionPlayer => m_userInput.Controls.ActionPlayer;
        public Vector2 Movement => ActionPlayer.Move.ReadValue<Vector2>();
        public float MouseScroll => ActionPlayer.MouseScroll.ReadValue<float>();

        public ActionUserInput(UserInput userInput) => m_userInput = userInput;

        public void Enable() => ActionPlayer.Enable();
        public void Disable() => ActionPlayer.Disable();
    }
}